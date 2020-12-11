using System;
using System.Collections.Generic;
using Bjerg.Lor;

namespace Bjerg
{
    public class Catalog
    {
        internal Catalog(Locale locale, Version version)
        {
            Locale = locale;
            Version = version;
        }

        public Locale Locale { get; }

        public Version Version { get; }

        public IReadOnlyDictionary<string, LorVocabTerm> VocabTerms { get; init; } = new Dictionary<string, LorVocabTerm>(0);

        public IReadOnlyDictionary<string, LorKeyword> Keywords { get; init; } = new Dictionary<string, LorKeyword>(0);

        public IReadOnlyDictionary<string, LorFaction> Regions { get; init; } = new Dictionary<string, LorFaction>(0);

        public IReadOnlyDictionary<string, LorSpellSpeed> SpellSpeeds { get; init; } = new Dictionary<string, LorSpellSpeed>(0);

        public IReadOnlyDictionary<string, LorRarity> Rarities { get; init; } = new Dictionary<string, LorRarity>(0);

        public IReadOnlyDictionary<string, LorSet> Sets { get; init; } = new Dictionary<string, LorSet>(0);

        public IReadOnlyList<LorSupertype> Supertypes { get; init; } = Array.Empty<LorSupertype>();

        public IReadOnlyList<LorType> Types { get; init; } = Array.Empty<LorType>();

        public IReadOnlyList<LorSubtype> Subtypes { get; init; } = Array.Empty<LorSubtype>();

        public IReadOnlyDictionary<string, ICard> Cards { get; init; } = new Dictionary<string, ICard>(0);
    }
}
