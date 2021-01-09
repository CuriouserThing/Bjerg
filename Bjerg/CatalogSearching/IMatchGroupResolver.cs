using System.Collections.Generic;
using System.Linq;

namespace Bjerg.CatalogSearching
{
    public interface IMatchGroupResolver<T>
    {
        IEnumerable<T> OrderMatchItems(IEnumerable<T> items, string key, float keyStrength);

        T FlattenMatchItems(IEnumerable<T> items, string key, float keyStrength)
        {
            return OrderMatchItems(items, key, keyStrength).First();
        }
    }
}
