using System.Collections.Generic;

namespace Bjerg.CatalogSearching
{
    public class CardNameGrouper : IItemGrouper<ICard, string>
    {
        public IEnumerable<ICard> SelectAllItems(Catalog catalog)
        {
            return catalog.Cards.Values;
        }

        public string SelectItemKey(ICard item)
        {
            return item.Name ?? string.Empty;
        }
    }
}
