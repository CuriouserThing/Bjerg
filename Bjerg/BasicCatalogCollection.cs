using Bjerg.DataDragon;
using Bjerg.Lor;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Bjerg
{
    public class BasicCatalogCollection : ICatalogCollection
    {
        private readonly object _syncRoot = new object();

        private IDataDragonFetcher DdFetcher { get; }

        private ILogger Logger { get; }

        private Dictionary<(Locale, Version), Catalog> CatCache { get; } = new Dictionary<(Locale, Version), Catalog>();

        public BasicCatalogCollection(IDataDragonFetcher ddFetcher, ILogger logger)
        {
            DdFetcher = ddFetcher;
            Logger = logger;
        }

        private static Version Set2ReleaseVersion { get; } = new Version(1, 0, 0);

        private static Version Set3ReleaseVersion { get; } = new Version(1, 8, 0);

        private static Version FirstSetDtoVersion { get; } = new Version(1, 8, 0);

        private async Task<DdIconTerm[]?> GetSetsAsync(Locale locale, Version version)
        {
            if (!version.IsEarlierThan(FirstSetDtoVersion))
            {
                Logger.LogError($"Version {version} isn't earlier than version {FirstSetDtoVersion}, when Riot introduced set DTOs. This version should have set DTOs, so it's unclear how to get sets for this version.");
                return null;
            }

            Catalog? setsCatalog;
            using (Logger.BeginScope(locale))
            using (Logger.BeginScope(version))
            {
                setsCatalog = await GetCatalog(locale, FirstSetDtoVersion);
            }

            if (setsCatalog is null)
            {
                Logger.LogError($"Couldn't get a catalog for version {FirstSetDtoVersion}, when Riot introduced set DTOs. Therefore, can't find a catalog with sets to reference for version {version}.");
                return null;
            }

            int setCount;
            if (!version.IsEarlierThan(Set3ReleaseVersion))
            {
                setCount = 3;
            }
            else if (!version.IsEarlierThan(Set2ReleaseVersion))
            {
                setCount = 2;
            }
            else
            {
                setCount = 1;
            }

            var ddSets = new DdIconTerm[setCount];
            for (int i = 0; i < setCount; i++)
            {
                string setNameRef = $"Set{i + 1}";
                if (setsCatalog.Sets.TryGetValue(setNameRef, out LorSet? set))
                {
                    ddSets[i] = new DdIconTerm
                    {
                        NameRef = setNameRef,
                        Name = set.Name,
                        IconAbsolutePath = set.IconPath.ToString(),
                    };
                }
                else
                {
                    Logger.LogError($"Version {FirstSetDtoVersion}, when Riot introduced set DTOs, doesn't have a reference to {setNameRef}, but it should.");
                    return null;
                }
            }
            return ddSets;
        }

        public async Task<Catalog?> GetCatalog(Locale locale, Version version)
        {
            lock (_syncRoot)
            {
                if (CatCache.TryGetValue((locale, version), out Catalog? cat))
                {
                    return cat;
                }
            }

            DdGlobals? globals = await DdFetcher.FetchGlobals(locale, version);
            if (globals is null)
            {
                Logger.LogError($"Couldn't fetch Data Dragon globals. Therefore, can't create a catalog.");
                return null;
            }

            if (globals.Sets is null)
            {
                Logger.LogInformation($"No set DTO array found in Data Dragon globals. Attempting to patch in an array manually.");
                globals.Sets = await GetSetsAsync(locale, version);
                if (globals.Sets is null)
                {
                    Logger.LogError($"Couldn't patch in a set array. Therefore, can't create a catalog.");
                    return null;
                }
            }

            int setCount = globals.Sets.Length;
            var setCardTasks = new Task<IReadOnlyList<DdCard>?>[setCount];
            for (int i = 0; i < setCount; i++)
            {
                setCardTasks[i] = DdFetcher.FetchSetCards(locale, version, i + 1);
            }

            IReadOnlyList<DdCard>?[] setCardLists = await Task.WhenAll(setCardTasks);
            var cards = new List<DdCard>();
            for (int i = 0; i < setCount; i++)
            {
                IReadOnlyList<DdCard>? setCardList = setCardLists[i];
                if (setCardList is null)
                {
                    Logger.LogError($"Couldn't fetch Data Dragon cards for set number {i + 1}. Therefore, can't create a catalog.");
                    return null;
                }
                string nameRef = $"Set{i + 1}";
                foreach (DdCard card in setCardList)
                {
                    card.Set = nameRef; // we may or may not need to patch in the set name -- do it regardless
                    cards.Add(card);
                }
            }

            lock (_syncRoot)
            {
                // Since we're not locking the entire method, check if another caller got here in the time between locks.
                if (!CatCache.TryGetValue((locale, version), out Catalog? cat))
                {
                    var catMaker = new CatalogMaker(locale, version, globals, cards, Logger);
                    cat = catMaker.MakeCatalog();
                    CatCache.Add((locale, version), cat);
                }
                return cat;
            }
        }

        #region Disposable

        private bool _disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                }
                _disposedValue = true;
            }
        }

        ~BasicCatalogCollection()
        {
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            System.GC.SuppressFinalize(this);
        }

        #endregion
    }
}
