namespace Bjerg.CatalogSearching
{
    public enum KeyConflictResolution
    {
        /// <summary>
        ///     Let the searcher determine how to resolve each group of same-key items.
        /// </summary>
        Default = 0,

        /// <summary>
        ///     Create one item match for each group of same-key items, flattening the multiple items into one.
        /// </summary>
        Flattened,

        /// <summary>
        ///     Create separate item matches for each item in each group of same-key items, allowing the search to return multiple
        ///     items with the same key.
        /// </summary>
        Separated,
    }
}
