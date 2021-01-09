namespace Bjerg.CatalogSearching
{
    public enum ItemDownscaleCurve
    {
        /// <summary>
        ///     Let the searcher determine how to calculate match strength from key strength and the item strength multiplier.
        /// </summary>
        Default = 0,

        /// <summary>
        ///     The match strength is k * m, where k is the key strength and m is the item strength multiplier.
        /// </summary>
        Linear,

        /// <summary>
        ///     The match strength is k * m ^ (1 - k), where k is the key strength and m is the item strength multiplier. This
        ///     biases the curve such that the multiplier downscales key strength less and less as k approaches 100%; at 100% there
        ///     is no downscaling (i.e. a perfect match remains a perfect match).
        /// </summary>
        Biased,
    }
}
