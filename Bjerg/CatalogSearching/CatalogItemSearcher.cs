using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Bjerg.CatalogSearching
{
    public class CatalogItemSearcher<T> where T : class
    {
        private readonly CultureInfo _cultureInfo;

        public CatalogItemSearcher(Catalog catalog, IItemGrouper<T, string> itemGrouper)
        {
            Catalog = catalog;
            ItemGrouper = itemGrouper;

            _cultureInfo = Catalog.Locale.GetCultureInfo();
        }

        public Catalog Catalog { get; }

        public IItemGrouper<T, string> ItemGrouper { get; }

        public IStringCanonicalizer TermTargetCanonicalizer { get; init; } = new PassthroughCanonicalizer();

        public IStringMatcherFactory TermMatcherFactory { get; init; } = new ExactStringMatcher.Factory();

        public float KeyStrengthThreshold { get; init; } = 0.5f;

        public KeyConflictResolution KeyConflictResolution { get; init; } = KeyConflictResolution.Flattened;

        public IMatchGroupResolver<T> MatchGroupResolver { get; init; } = new PassthroughMatchGroupResolver<T>();

        public IReadOnlyList<IItemStrengthDownscaler<T>> ItemStrengthDownscalers { get; init; } = Array.Empty<IItemStrengthDownscaler<T>>();

        public ItemDownscaleCurve ItemDownscaleCurve { get; init; } = ItemDownscaleCurve.Linear;

        public IItemMatchSorter<T> ItemMatchSorter { get; init; } = new PassthroughItemMatchSorter<T>();

        public SearchResult<T> Search(string term)
        {
            term = TermTargetCanonicalizer.Canonicalize(term, _cultureInfo);
            IStringMatcher matcher = TermMatcherFactory.CreateStringMatcher(term);
            var matches = new List<ItemMatch<T>>();

            foreach (IGrouping<string, T> itemGroup in ItemGrouper.SelectAllItems(Catalog).GroupBy(SelectItemKey))
            {
                string key = itemGroup.Key;
                float keyStrength = matcher.GetMatchStrength(key);
                if (keyStrength < KeyStrengthThreshold) { continue; }

                T[] items = itemGroup.ToArray();
                if (items.Length > 1)
                {
                    if (KeyConflictResolution == KeyConflictResolution.Separated)
                    {
                        items = MatchGroupResolver.OrderMatchItems(items, key, keyStrength).ToArray();
                    }
                    else
                    {
                        items = new[] { MatchGroupResolver.FlattenMatchItems(items, key, keyStrength) };
                    }
                }

                foreach (T item in items)
                {
                    var m = 1f;
                    foreach (var downscaler in ItemStrengthDownscalers)
                    {
                        m *= downscaler.GetMultiplier(item);
                    }

                    if (ItemDownscaleCurve == ItemDownscaleCurve.Biased)
                    {
                        m = (float)Math.Pow(m, 1 - keyStrength);
                    }

                    float matchStrength = keyStrength * m;
                    matches.Add(new ItemMatch<T>(key, keyStrength, item, m, matchStrength));
                }
            }

            if (ItemMatchSorter.ShouldSortMatches(matches))
            {
                matches.Sort(ItemMatchSorter.Compare);
            }

            return new SearchResult<T>(term, Catalog, matches);
        }

        private string SelectItemKey(T item)
        {
            string key = ItemGrouper.SelectItemKey(item);
            return TermTargetCanonicalizer.Canonicalize(key, _cultureInfo);
        }
    }
}
