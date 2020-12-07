namespace Bjerg.CatalogSearching
{
    /// <summary>
    ///     Interface for scaling up or down the percentage that a searcher matches an item.
    /// </summary>
    /// <typeparam name="T">The item type.</typeparam>
    public interface IItemMatchScaler<in T>
    {
        /// <summary>
        ///     Scale up or down (or not at all) the original match pct of a search on an item, keeping within [0%, 100%] bounds.
        /// </summary>
        /// <param name="item">THe matched item.</param>
        /// <param name="matchPct">The original match percentage.</param>
        /// <returns>A percentage ranging from 100% (perfect match) to 0% (no match).</returns>
        public float ScaleMatchPct(T item, float matchPct);
    }
}
