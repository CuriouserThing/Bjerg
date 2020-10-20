using Bjerg.Lor;
using System.Collections.Generic;

namespace Bjerg
{
    public class Catalog
    {
        public Locale Locale { get; }

        public Version Version { get; }

        internal Catalog(Locale locale, Version version)
        {
            Locale = locale;
            Version = version;
        }

        public IReadOnlyDictionary<string, LorVocabTerm> VocabTerms { get; internal set; } = new Dictionary<string, LorVocabTerm>(capacity: 0);

        public IReadOnlyDictionary<string, LorKeyword> Keywords { get; internal set; } = new Dictionary<string, LorKeyword>(capacity: 0);

        public IReadOnlyDictionary<string, LorFaction> Regions { get; internal set; } = new Dictionary<string, LorFaction>(capacity: 0);

        public IReadOnlyDictionary<string, LorSpellSpeed> SpellSpeeds { get; internal set; } = new Dictionary<string, LorSpellSpeed>(capacity: 0);

        public IReadOnlyDictionary<string, LorRarity> Rarities { get; internal set; } = new Dictionary<string, LorRarity>(capacity: 0);

        public IReadOnlyDictionary<string, LorSet> Sets { get; internal set; } = new Dictionary<string, LorSet>(capacity: 0);

        public IReadOnlyDictionary<string, LorSupertype> Supertypes { get; internal set; } = new Dictionary<string, LorSupertype>(capacity: 0);

        public IReadOnlyDictionary<string, LorType> Types { get; internal set; } = new Dictionary<string, LorType>(capacity: 0);

        public IReadOnlyDictionary<string, LorSubtype> Subtypes { get; internal set; } = new Dictionary<string, LorSubtype>(capacity: 0);

        public IReadOnlyDictionary<string, ICard> Cards { get; internal set; } = new Dictionary<string, ICard>(capacity: 0);
    }
}
