using System.Collections.Generic;
using Bjerg.Lor;

namespace Bjerg.CatalogSearching.Services
{
    public class TranslatedSearchResult<T> : SearchResult<T> where T : class
    {
        private TranslatedSearchResult(SearchResult<T> result)
            : base(result.SearchTerm, result.SearchLocale, result.SearchVersion, result.Matches)
        {
            TranslationMap = new Dictionary<T, T>(0);
        }

        private TranslatedSearchResult(SearchResult<T> result, Locale? translationLocale, IReadOnlyDictionary<T, T> translationMap)
            : base(result.SearchTerm, result.SearchLocale, result.SearchVersion, result.Matches)
        {
            TranslationLocale = translationLocale;
            TranslationMap = translationMap;
        }

        public Locale? TranslationLocale { get; }

        public IReadOnlyDictionary<T, T> TranslationMap { get; }

        public static TranslatedSearchResult<T> FromNoSearch(SearchParameters parameters)
        {
            var result = new SearchResult<T>
            (
                parameters.SearchTerm,
                parameters.SearchLocale,
                parameters.Version
            );
            return new TranslatedSearchResult<T>(result);
        }

        public static TranslatedSearchResult<T> FromUntranslatedSearch(SearchResult<T> result)
        {
            return new(result);
        }

        public static TranslatedSearchResult<T> FromTranslatedSearch(SearchResult<T> result, Locale translationLocale, IReadOnlyDictionary<T, T> translationMap)
        {
            return new(result, translationLocale, translationMap);
        }

        private static ItemMatch<CatalogItemUnion> GetMatch<TItem>(ItemMatch<TItem> original, CatalogItemUnion union)
        {
            return new(
                original.Key,
                original.KeyStrength,
                union,
                original.ItemStrengthMultiplier,
                original.MatchStrength);
        }

        public static TranslatedSearchResult<CatalogItemUnion> MergeSearchResults
        (
            TranslatedSearchResult<ICard>      cardResult,
            TranslatedSearchResult<LorKeyword> keywordResult,
            TranslatedSearchResult<Deck>       deckResult,
            IItemMatchSorter<CatalogItemUnion> sorter
        )
        {
            var matches = new List<ItemMatch<CatalogItemUnion>>();
            var map = new Dictionary<CatalogItemUnion, CatalogItemUnion>();

            foreach (var m in cardResult.Matches)
            {
                matches.Add(GetMatch(m, CatalogItemUnion.AsCard(m.Item)));
                if (cardResult.TranslationMap.TryGetValue(m.Item, out ICard? tItem))
                {
                    map.Add(CatalogItemUnion.AsCard(m.Item), CatalogItemUnion.AsCard(tItem));
                }
            }

            foreach (var m in keywordResult.Matches)
            {
                matches.Add(GetMatch(m, m.Item));
                if (keywordResult.TranslationMap.TryGetValue(m.Item, out LorKeyword? tItem))
                {
                    map.Add(m.Item, tItem);
                }
            }

            foreach (var m in deckResult.Matches)
            {
                matches.Add(GetMatch(m, m.Item));
                if (deckResult.TranslationMap.TryGetValue(m.Item, out Deck? tItem))
                {
                    map.Add(m.Item, tItem);
                }
            }

            if (sorter.ShouldSortMatches(matches))
            {
                matches.Sort(sorter.Compare);
            }

            var result = new SearchResult<CatalogItemUnion>(
                cardResult.SearchTerm,
                cardResult.SearchLocale,
                cardResult.SearchVersion,
                matches);
            return new TranslatedSearchResult<CatalogItemUnion>(
                result,
                cardResult.TranslationLocale,
                map);
        }
    }
}
