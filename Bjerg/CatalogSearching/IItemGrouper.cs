using System.Collections.Generic;

namespace Bjerg.CatalogSearching
{
    /// <summary>
    ///     Interface for selecting a collection of items from a catalog and selecting a key from each item.
    /// </summary>
    /// <typeparam name="TItem">The item type.</typeparam>
    /// <typeparam name="TKey">The item-group key type.</typeparam>
    public interface IItemGrouper<TItem, out TKey>
    {
        /// <summary>
        ///     Select all items to be grouped from a catalog.
        /// </summary>
        IEnumerable<TItem> SelectAllItems(Catalog catalog);

        /// <summary>
        ///     Select the item key to use for grouping selected items.
        /// </summary>
        TKey SelectItemKey(TItem item);
    }
}
