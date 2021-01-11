using System.Collections.Generic;

namespace Bjerg.CatalogSearching
{
    public sealed class PassthroughItemMatchSorter<T> : IItemMatchSorter<T>
    {
        public bool ShouldSortMatches(IReadOnlyList<ItemMatch<T>> matches)
        {
            return false;
        }

        public int Compare(ItemMatch<T>? x, ItemMatch<T>? y)
        {
            return 0;
        }
    }
}
