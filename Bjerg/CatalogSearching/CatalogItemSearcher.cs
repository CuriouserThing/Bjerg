using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Bjerg.CatalogSearching
{
    public class CatalogItemSearcher<T> where T : class
    {
        public CatalogItemSearcher(Catalog catalog, IItemGrouper<T, string> itemGrouper)
        {
            Catalog = catalog;
            ItemGrouper = itemGrouper;
        }

        public Catalog Catalog { get; }

        public IItemGrouper<T, string> ItemGrouper { get; }

        /// <summary>
        ///     Optional (by default, looks only for exact string matches).
        /// </summary>
        public IStringMatcherFactory StringMatcherFactory { get; init; } = new ExactStringMatcher.Factory();

        /// <summary>
        ///     Optional (by default, passes the item(s) through unmodified).
        /// </summary>
        public IItemSelector<T> ItemSelector { get; init; } = new PassthroughItemSelector<T>();

        /// <summary>
        ///     Optional (by default, passes the item match through unmodified).
        /// </summary>
        public IItemMatchScaler<T> ItemMatchScaler { get; init; } = new PassthroughItemMatchScaler<T>();

        /// <summary>
        ///     The absolute match pct threshold below which to reject all matches and above (or equal to) which to accept all
        ///     matches.
        /// </summary>
        public float MatchThreshold { get; init; } = 0.5f;

        public IReadOnlyList<ItemMatch<T>> Search(string lookup)
        {
            CultureInfo cultureInfo = Catalog.Locale.CultureInfo;
            lookup = lookup.ToLower(cultureInfo);
            IStringMatcher matcher = StringMatcherFactory.CreateStringMatcher(lookup);
            var matches = new List<ItemMatch<T>>();

            foreach (IGrouping<string, T> itemGroup in ItemGrouper.SelectAllItems(Catalog)
                .GroupBy(ItemGrouper.SelectItemKey))
            {
                string name = itemGroup.Key.ToLower(cultureInfo);
                float m = matcher.GetMatchPct(name);
                if (m < MatchThreshold) { continue; }

                IReadOnlyList<T> items = ItemSelector.Reduce(itemGroup);
                if (items.Count == 0) { continue; }

                // If there are multiple conflicting (i.e. same-key) items remaining after reduction, join them into one single match, with the highest scaled match pct taking front billing.
                (float, T)[] itemsSorted = items
                    .Select(t => (ItemMatchScaler.ScaleMatchPct(t, m), t))
                    .OrderByDescending(tm => tm.Item1)
                    .ToArray();
                IReadOnlyList<T> expansion = itemsSorted
                    .SelectMany(tm => ItemSelector.Expand(tm.Item2))
                    .ToArray();
                matches.Add(new ItemMatch<T>(name, itemsSorted[0].Item1, itemsSorted[0].Item2, expansion));
            }

            matches.Sort(CompareDescending);
            return matches;
        }

        private static int CompareDescending(ItemMatch<T> x, ItemMatch<T> y)
        {
            float mx = x.MatchPct;
            float my = y.MatchPct;
            if (mx < my)
            {
                return +1;
            }
            else if (mx > my)
            {
                return -1;
            }
            else
            {
                return 0;
            }
        }
    }
}
