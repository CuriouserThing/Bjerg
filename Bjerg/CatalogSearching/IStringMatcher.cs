namespace Bjerg.CatalogSearching
{
    /// <summary>
    ///     Interface for fuzzy-matching a single source string to any target string.
    /// </summary>
    public interface IStringMatcher
    {
        /// <summary>
        ///     Fuzzy-match the source string to a target string, returning a strength value between 0% and 100%. Not guaranteed to
        ///     be thread-safe.
        /// </summary>
        /// <param name="target">The string to match the source string to.</param>
        /// <returns>A strength value ranging from 100% (perfect match) to 0% (no match).</returns>
        float GetMatchStrength(string target);
    }
}
