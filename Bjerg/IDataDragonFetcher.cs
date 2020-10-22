using Bjerg.DataDragon;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bjerg
{
    public interface IDataDragonFetcher : IDisposable
    {
        /// <summary>
        /// Fetch a Data Dragon globals DTO in a thread-safe manner.
        /// </summary>
        /// <param name="locale">The globals locale.</param>
        /// <param name="version">The globals version.</param>
        /// <returns>The globals DTO. If non-null, the function succeeded. If null, the function failed.</returns>
        Task<DdGlobals?> FetchGlobals(Locale locale, Version version);

        /// <summary>
        /// Fetch an array of Data Dragon card DTOs for a particular set in a thread-safe manner.
        /// </summary>
        /// <param name="locale">The cards' locale.</param>
        /// <param name="version">The cards' version.</param>
        /// <param name="setNumber">The cards' set.</param>
        /// <returns>The card DTOs. If non-null, the function succeeded. If null, the function failed.</returns>
        Task<IReadOnlyList<DdCard>?> FetchSetCards(Locale locale, Version version, int setNumber);
    }
}
