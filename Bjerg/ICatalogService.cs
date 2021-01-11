using System;
using System.Threading.Tasks;

namespace Bjerg
{
    public interface ICatalogService : IDisposable
    {
        /// <summary>
        ///     Fetch or create a catalog in a thread-safe manner.
        /// </summary>
        /// <param name="locale">The catalog's locale.</param>
        /// <param name="version">The catalog's version.</param>
        /// <returns>A comprehensive catalog.</returns>
        Task<Catalog> GetCatalog(Locale locale, Version version);

        /// <summary>
        ///     Fetch or create a catalog for the home locale en-US in a thread-safe manner.
        /// </summary>
        /// <param name="version">The catalog's version.</param>
        /// <returns>A comprehensive catalog.</returns>
        async Task<Catalog> GetHomeCatalog(Version version)
        {
            return await GetCatalog(Locale.Home, version);
        }
    }
}
