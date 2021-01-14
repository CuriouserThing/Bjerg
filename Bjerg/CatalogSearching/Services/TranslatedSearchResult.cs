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

        private static TranslatedSearchResult<CatalogItemUnion> FromBaseResult<TItem>(
            IReadOnlyList<ItemMatch<CatalogItemUnion>>              matches,
            IReadOnlyDictionary<CatalogItemUnion, CatalogItemUnion> translationMap,
            TranslatedSearchResult<TItem>                           baseResult)
            where TItem : class
        {
            var result = new SearchResult<CatalogItemUnion>(
                baseResult.SearchTerm,
                baseResult.SearchLocale,
                baseResult.SearchVersion,
                matches);
            return new TranslatedSearchResult<CatalogItemUnion>(
                result,
                baseResult.TranslationLocale,
                translationMap);
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

            if (matches.Count == 0)
            {
                return FromBaseResult(matches, map, cardResult);
            }

            if (sorter.ShouldSortMatches(matches))
            {
                matches.Sort(sorter.Compare);
            }

            // Use fields from whichever type provided the best match
            return matches[0].Item.T switch
            {
                CatalogItemUnion.Type.Card    => FromBaseResult(matches, map, cardResult),
                CatalogItemUnion.Type.Keyword => FromBaseResult(matches, map, keywordResult),
                CatalogItemUnion.Type.Deck    => FromBaseResult(matches, map, deckResult),
                _                             => FromBaseResult(matches, map, cardResult),
            };
        }
    }
}
