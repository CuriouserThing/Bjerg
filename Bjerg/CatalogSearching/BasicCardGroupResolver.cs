using System.Collections.Generic;
using System.Linq;

namespace Bjerg.CatalogSearching
{
    public class BasicCardGroupResolver : IMatchGroupResolver<ICard>
    {
        public IEnumerable<ICard> OrderMatchItems(IEnumerable<ICard> items, string key, float keyStrength)
        {
            return items
                .OrderByDescending(c => c.Collectible) // favor collectibles
                .ThenBy(c => c.Code.TNumber)           // favor "root" cards without a T number (0)
                .ThenBy(c => c.Code);                  // simple non-arbitrary tiebreaker
        }
    }
}
