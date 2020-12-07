using System.Collections.Generic;

namespace Bjerg.CatalogSearching
{
    /// <summary>
    ///     Interface for a) reducing a list of items into a possibly-smaller list and b) expanding a single item into a
    ///     possibly-larger list.
    /// </summary>
    public interface IItemSelector<T>
    {
        /// <summary>
        ///     Select a subset (but not necessarily a strict subset) of an item list.
        /// </summary>
        /// <param name="items">The items to reduce.</param>
        /// <returns>
        ///     A subset of the original list. Can contain any number (including none or all) of the original items in any
        ///     order.
        /// </returns>
        IReadOnlyList<T> Reduce(IEnumerable<T> items);

        /// <summary>
        ///     Select a superset (but not necessarily a strict superset) of a single item.
        /// </summary>
        /// <param name="item">The item to expand.</param>
        /// <returns>A list of items containing the original item (possibly just the original) in any position.</returns>
        IReadOnlyList<T> Expand(T item);
    }
}
