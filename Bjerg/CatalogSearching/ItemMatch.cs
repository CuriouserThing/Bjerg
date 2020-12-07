using System.Collections.Generic;

namespace Bjerg.CatalogSearching
{
    public class ItemMatch<T>
    {
        public ItemMatch(string matchKey, float matchPct, T item, IReadOnlyList<T> itemExpansion)
        {
            MatchKey = matchKey;
            MatchPct = matchPct;
            Item = item;
            ItemExpansion = itemExpansion;
        }

        public string MatchKey { get; }

        public float MatchPct { get; }

        public T Item { get; }

        public IReadOnlyList<T> ItemExpansion { get; }
    }
}
