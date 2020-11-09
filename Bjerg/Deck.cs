using Bjerg.DeckCoding;
using Bjerg.Lor;
using System.Collections.Generic;
using System.Linq;

namespace Bjerg
{
    public class Deck
    {
        private readonly string _code;

        public Locale Locale { get; }

        public Version Version { get; }

        public IReadOnlyList<CardAndCount> Cards { get; }

        public Deck(string code, Locale locale, Version version, IReadOnlyList<CardAndCount> cards)
        {
            _code = code;
            Locale = locale;
            Version = version;
            Cards = cards;
        }

        public static bool TryFromCode(string code, Catalog catalog, out Deck? deck)
        {
            IReadOnlyList<RawCardAndCount> rccs = Coding.GetDeckCardsFromCode(code);
            var ccs = new CardAndCount[rccs.Count];

            for (int i = 0; i < rccs.Count; i++)
            {
                RawCardAndCount rcc = rccs[i];
                LorFaction? region = catalog.Regions.Values.SingleOrDefault(r => r.Index == rcc.Faction);
                if (region is null)
                {
                    deck = null;
                    return false;
                }

                string ss = rcc.Set.ToString().PadLeft(2, '0');
                string ff = region.Abbreviation;
                string nnn = rcc.Number.ToString().PadLeft(3, '0');
                string cardCode = $"{ss}{ff}{nnn}";

                if (!catalog.Cards.TryGetValue(cardCode, out ICard? card))
                {
                    if (!CardCode.TryFromString(cardCode, out CardCode? outCardCode))
                    {
                        deck = null;
                        return false;
                    }
                    LorSet? set = catalog.Sets.Values.SingleOrDefault(s => s.Index == rcc.Set);
                    card = new CodeOnlyCard(outCardCode!, catalog.Locale, catalog.Version, region, set);
                }
                ccs[i] = new CardAndCount(card, rcc.Count);;
            }

            deck = new Deck(code, catalog.Locale, catalog.Version, ccs);
            return true;
        }

        public override string ToString()
        {
            return _code;
        }
    }
}
