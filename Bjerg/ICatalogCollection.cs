using System.Threading.Tasks;

namespace Bjerg
{
    public interface ICatalogCollection
    {
        /// <summary>
        /// Fetch or create a catalog in a thread-safe manner.
        /// </summary>
        /// <param name="locale">The catalog's locale.</param>
        /// <param name="version">The catalog's version.</param>
        /// <returns>A comprehensive catalog. If non-null, the function succeeded. If null, the function failed.</returns>
        Task<Catalog?> GetCatalog(Locale locale, Version version);
    }
}
