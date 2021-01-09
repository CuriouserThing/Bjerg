using System.Collections.Generic;

namespace Bjerg.CatalogSearching
{
    public class PassthroughMatchGroupResolver<T> : IMatchGroupResolver<T>
    {
        public IEnumerable<T> OrderMatchItems(IEnumerable<T> items, string key, float keyStrength)
        {
            return items;
        }
    }
}
