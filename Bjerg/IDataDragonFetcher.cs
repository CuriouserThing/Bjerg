using Bjerg.DataDragon;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bjerg
{
    public interface IDataDragonFetcher : IDisposable
    {
        Task<DdGlobals?> FetchGlobals(Locale locale, Version version);

        Task<IReadOnlyList<DdCard>?> FetchSetCards(Locale locale, Version version, int setNumber);
    }
}
