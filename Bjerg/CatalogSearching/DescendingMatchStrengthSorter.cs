using System.Collections.Generic;

namespace Bjerg.CatalogSearching
{
    public class DescendingMatchStrengthSorter<T> : IItemMatchSorter<T>
    {
        public virtual bool ShouldSortMatches(IReadOnlyList<ItemMatch<T>> matches)
        {
            return true;
        }

        public int Compare(ItemMatch<T> x, ItemMatch<T> y)
        {
            float mx = x.MatchStrength;
            float my = y.MatchStrength;
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
