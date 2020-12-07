using System.Collections.Generic;
using System.Linq;
using Bjerg.Lor;

namespace Bjerg.CatalogSearching
{
    /// <summary>
    ///     A no-fuss, batteries-included card selector with criteria that may change from version to version.
    /// </summary>
    public class BasicKeywordSelector : IItemSelector<LorKeyword>
    {
        public IReadOnlyList<LorKeyword> Reduce(IEnumerable<LorKeyword> items)
        {
            // Return only keywords with distinct, non-whitespace descriptions
            return items
                .Where(k => !string.IsNullOrWhiteSpace(k.Description))
                .GroupBy(k => k.Description)
                .Select(g => g.OrderBy(k => k.Key).First())
                .ToArray();
        }

        public IReadOnlyList<LorKeyword> Expand(LorKeyword item)
        {
            // Passthrough
            return new[] {item};
        }
    }
}
