using System.Collections.Generic;
using System.Linq;

namespace Bjerg.CatalogSearching
{
    public class PassthroughItemSelector<T> : IItemSelector<T>
    {
        public IReadOnlyList<T> Reduce(IEnumerable<T> items)
        {
            return items.ToArray();
        }

        public IReadOnlyList<T> Expand(T item)
        {
            return new[] { item };
        }
    }
}
