using System.Collections.Generic;
using System.Linq;
using Bjerg.Lor;

namespace Bjerg.CatalogSearching
{
    public class KeywordNameGrouper : IItemGrouper<LorKeyword, string>
    {
        /// <summary>
        ///     Whether or not to include vocab terms (e.g. Allegiance) disguised as keywords.
        /// </summary>
        public bool IncludeVocabTerms { get; init; } = false;

        /// <summary>
        ///     Whether or not to include keywords with empty or whitespace <see cref="LorKeyword.Description" /> values.
        /// </summary>
        public bool IncludeDescriptionlessKeywords { get; init; } = false;

        public IEnumerable<LorKeyword> SelectAllItems(Catalog catalog)
        {
            IEnumerable<LorKeyword> keywords = catalog.Keywords.Values;

            if (IncludeVocabTerms)
            {
                IEnumerable<LorKeyword> vocabTerms = catalog.VocabTerms.Values
                    .Select(vt => new LorKeyword(vt.Key, vt.Name, vt.Description));

                keywords = keywords.Concat(vocabTerms);
            }

            if (!IncludeDescriptionlessKeywords)
            {
                keywords = keywords.Where(k => !string.IsNullOrWhiteSpace(k.Description));
            }

            return keywords;
        }

        public string SelectItemKey(LorKeyword item)
        {
            return item.Name;
        }
    }
}
