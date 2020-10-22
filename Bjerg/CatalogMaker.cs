using Bjerg.DataDragon;
using Bjerg.Lor;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Bjerg
{
    public class CatalogMaker
    {
        private Locale Locale { get; }

        private TextInfo TextInfo { get; }

        private Version Version { get; }

        private DdGlobals Globals { get; }

        private IReadOnlyList<DdCard> DdCards { get; }

        private ILogger Logger { get; }

        public CatalogMaker(Locale locale, Version version, DdGlobals globals, IReadOnlyList<DdCard> ddCards, ILogger logger)
        {
            Locale = locale;
            TextInfo = locale.CultureInfo.TextInfo;
            Version = version;
            Globals = globals;
            DdCards = ddCards;
            Logger = logger;
        }

        private delegate bool LorItemConverter<TDd, TLor>(TDd ddItem, TextInfo textInfo, out TLor? lorItem)
            where TDd : DdTerm
            where TLor : class;

        private Dictionary<string, TLor> ConvertDdItems<TDd, TLor>(IReadOnlyList<TDd>? ddItems, string ddArrayName, LorItemConverter<TDd, TLor> itemConverter)
            where TDd : DdTerm
            where TLor : class
        {
            var lorItems = new Dictionary<string, TLor>();
            if (ddItems is null)
            {
                Logger.LogWarning($"This Data Dragon globals DTO does not contain a {ddArrayName} array.");
            }
            else
            {
                foreach (TDd ddItem in ddItems)
                {
                    if (ddItem.NameRef is null)
                    {
                        Logger.LogWarning($"Encountered a DTO in the Data Dragon {ddArrayName} array without a nameRef. Skipping this object.");
                        continue;
                    }

                    if (itemConverter(ddItem, TextInfo, out TLor? lorItem))
                    {
                        lorItems.Add(ddItem.NameRef, lorItem!);
                    }
                    else
                    {
                        Logger.LogWarning($"Could not convert a {ddArrayName} DTO with nameRef {ddItem.NameRef}. It may be missing a necessary field. Skipping this object.");
                    }
                }
            }
            return lorItems;
        }

        private void LogUnrecognizedPropWarning(BasicCard card, string propName, string propRef)
        {
            Logger.LogWarning($"{card} references an unrecognized {propName} '{propRef}'. Ignoring this {propName}.");
        }

        private void AddPropToCard<T>(BasicCard card, string? propRef, string propName, Dictionary<string, T> valueDic, Action<BasicCard, T> valueSetter) where T : class
        {
            if (propRef != null)
            {
                if (valueDic.TryGetValue(propRef, out T? value))
                {
                    valueSetter(card, value);
                }
                else
                {
                    LogUnrecognizedPropWarning(card, propName, propRef);
                }
            }
        }

        private void AddPropListToCard<T>(BasicCard card, IReadOnlyList<string>? propRefs, string propName, Dictionary<string, T> valueDic, Action<BasicCard, IReadOnlyList<T>> valuesSetter) where T : class
        {
            if (propRefs != null)
            {
                var values = new List<T>();
                foreach (string propRef in propRefs)
                {
                    if (valueDic.TryGetValue(propRef, out T? value))
                    {
                        values.Add(value);
                    }
                    else
                    {
                        LogUnrecognizedPropWarning(card, propName, propRef);
                    }
                }
                valuesSetter(card, values);
            }
        }

        public Catalog MakeCatalog()
        {
            // Convert collections of items from Data Dragon globals DTOs

            Dictionary<string, LorVocabTerm> vocabTerms = ConvertDdItems<DdVocabTerm, LorVocabTerm>(Globals.VocabTerms, "vocabTerms", LorVocabTerm.TryFromDataDragon);

            Dictionary<string, LorKeyword> keywords = ConvertDdItems<DdVocabTerm, LorKeyword>(Globals.Keywords, "keywords", LorKeyword.TryFromDataDragon);

            Dictionary<string, LorFaction> regions = ConvertDdItems<DdRegionTerm, LorFaction>(Globals.Regions, "regions", LorFaction.TryFromDataDragon);

            Dictionary<string, LorSpellSpeed> spellSpeeds = ConvertDdItems<DdTerm, LorSpellSpeed>(Globals.SpellSpeeds, "spellSpeeds", LorSpellSpeed.TryFromDataDragon);

            Dictionary<string, LorRarity> rarities = ConvertDdItems<DdTerm, LorRarity>(Globals.Rarities, "rarities", LorRarity.TryFromDataDragon);

            Dictionary<string, LorSet> sets = ConvertDdItems<DdIconTerm, LorSet>(Globals.Sets, "sets", LorSet.TryFromDataDragon);

            // Populate collections of [super/sub]types manually from list of cards

            IEnumerable<string> supertypeRefs = DdCards
                .Select(c => c.Supertype)
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => s!)
                .Distinct();
            var supertypes = new Dictionary<string, LorSupertype>();
            foreach (string supertypeRef in supertypeRefs)
            {
                string name = TextInfo.ToTitleCase(supertypeRef);
                supertypes.Add(supertypeRef, new LorSupertype(name));
            }

            IEnumerable<string> typeRefs = DdCards
                .Select(c => c.Type)
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => s!)
                .Distinct();
            var types = new Dictionary<string, LorType>();
            foreach (string typeRef in typeRefs)
            {
                string name = TextInfo.ToTitleCase(typeRef);
                types.Add(typeRef, new LorType(name));
            }

            IEnumerable<string> subtypeRefs = DdCards
                .SelectMany(c => c.GetDistinctSubtypes())
                .Distinct();
            var subtypes = new Dictionary<string, LorSubtype>();
            foreach (string subtypeRef in subtypeRefs)
            {
                string name = TextInfo.ToTitleCase(subtypeRef);
                subtypes.Add(subtypeRef, new LorSubtype(name));
            }

            // First card pass to initialize cards and add them to lookup

            var cards = new Dictionary<string, ICard>();
            foreach (DdCard ddCard in DdCards)
            {
                if (ddCard.CardCode is null)
                {
                    string name = ddCard.Name ?? "[Unnamed card]";
                    Logger.LogWarning($"{name} does not have a card code. Ignoring card.");
                    continue;
                }
                if (!CardCode.TryFromString(ddCard.CardCode, out CardCode? cardCode))
                {
                    Logger.LogWarning($"Could not parse the card code '{cardCode}'. Ignoring card.");
                    continue;
                }

                var card = new BasicCard(cardCode!, Locale, Version, ddCard.Name);
                if (!cards.TryAdd(ddCard.CardCode, card))
                {
                    Logger.LogWarning($"Multiple cards have the card code '{cardCode}'. Ignoring this duplicate card.");
                    continue;
                }

                AddPropToCard(card, ddCard.RegionRef, "RegionRef", regions, (c, v) => c.Region = v);
                AddPropToCard(card, ddCard.Supertype, "Supertype", supertypes, (c, v) => c.Supertype = v);
                AddPropToCard(card, ddCard.Type, "Type", types, (c, v) => c.Type = v);
                AddPropToCard(card, ddCard.SpellSpeedRef, "SpellSpeedRef", spellSpeeds, (c, v) => c.SpellSpeed = v);
                AddPropToCard(card, ddCard.Set, "Set", sets, (c, v) => c.Set = v);
                AddPropToCard(card, ddCard.RarityRef, "RarityRef", rarities, (c, v) => c.Rarity = v);

                AddPropListToCard(card, ddCard.Subtypes, "Subtype", subtypes, (c, vs) => c.Subtypes = vs);
                AddPropListToCard(card, ddCard.KeywordRefs, "KeywordRef", keywords, (c, vs) => c.Keywords = vs);

                card.Cost = ddCard.Cost;
                card.Attack = ddCard.Attack;
                card.Health = ddCard.Health;
                card.Collectible = ddCard.Collectible;
                card.Description = ddCard.Description;
                card.DescriptionRaw = ddCard.DescriptionRaw;
                card.LevelupDescription = ddCard.LevelupDescription;
                card.LevelupDescriptionRaw = ddCard.LevelupDescriptionRaw;
                card.FlavorText = ddCard.FlavorText;
                card.ArtistName = ddCard.ArtistName;

                if (ddCard.Assets != null && ddCard.Assets.Length > 0)
                {
                    string? gamePath = ddCard.Assets[0].GameAbsolutePath;
                    if (gamePath != null) { card.GameArtPath = new Uri(gamePath); }

                    string? fullPath = ddCard.Assets[0].FullAbsolutePath;
                    if (fullPath != null) { card.FullArtPath = new Uri(fullPath); }
                }
            }

            // Second card pass to fill in associated card references

            foreach (DdCard ddCard in DdCards)
            {
                if (ddCard.AssociatedCardRefs is null) { continue; }

                if (ddCard.CardCode is null) { continue; }
                string code = ddCard.CardCode!;

                if (!cards.TryGetValue(code, out ICard? card)) { continue; }

                var assoCards = new List<ICard>();
                foreach (string assoCode in ddCard.AssociatedCardRefs)
                {
                    if (cards.TryGetValue(assoCode, out ICard? assoCard))
                    {
                        assoCards.Add(assoCard);
                    }
                    else
                    {
                        Logger.LogWarning($"{card} has an associated card with the code '{assoCode}' that isn't in the card collection. Ignoring this associated card.");
                    }
                }

                // Small hack to cast the card to its known BasicCard type since C# can't make the cards dictionary covariant
                (card as BasicCard)!.AssociatedCards = assoCards;
            }

            // Return the finished product

            var catalog = new Catalog(Locale, Version)
            {
                VocabTerms = vocabTerms,
                Keywords = keywords,
                Regions = regions,
                SpellSpeeds = spellSpeeds,
                Rarities = rarities,
                Sets = sets,
                Supertypes = supertypes,
                Types = types,
                Subtypes = subtypes,
                Cards = cards,
            };
            return catalog;
        }
    }
}
