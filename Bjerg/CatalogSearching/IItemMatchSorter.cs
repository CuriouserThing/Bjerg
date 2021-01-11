using System.Collections.Generic;

namespace Bjerg.CatalogSearching
{
    public interface IItemMatchSorter<T>
    {
        bool ShouldSortMatches(IReadOnlyList<ItemMatch<T>> matches);

        int Compare(ItemMatch<T> x, ItemMatch<T> y);
    }
}
