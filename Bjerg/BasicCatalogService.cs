using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bjerg.DataDragon;
using Bjerg.Lor;
using Microsoft.Extensions.Logging;

namespace Bjerg
{
    public class BasicCatalogService : ICatalogService
    {
        private readonly object _syncRoot = new();

        public BasicCatalogService(IDataDragonFetcher ddFetcher, ILogger<BasicCatalogService> logger)
        {
            DdFetcher = ddFetcher;
            Logger = logger;
        }

        private IDataDragonFetcher DdFetcher { get; }

        private ILogger Logger { get; }

        private Dictionary<(Locale, Version), Catalog> CatCache { get; } = new();

        private static Version Set2ReleaseVersion { get; } = new(1, 0, 0);

        private static Version Set3ReleaseVersion { get; } = new(1, 8, 0);

        private static Version FirstSetDtoVersion { get; } = new(1, 8, 0);

        private static Dictionary<string, int> RegionIndices { get; } = new()
        {
            ["Demacia"] = 0,
            ["Freljord"] = 1,
            ["Ionia"] = 2,
            ["Noxus"] = 3,
            ["PiltoverZaun"] = 4,
            ["ShadowIsles"] = 5,
            ["Bilgewater"] = 6,
            ["Shurima"] = 7,
            ["Targon"] = 9,
            ["BandleCity"] = 10,
        };

        public async Task<Catalog> GetCatalog(Locale locale, Version version)
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
                throw new InvalidOperationException("Couldn't fetch Data Dragon globals. Therefore, can't create a catalog.");
            }

            if (globals.Sets is null)
            {
                Logger.LogInformation("No set DTO array found in Data Dragon globals. Attempting to patch in an array manually.");
                globals.Sets = await GetSetsAsync(locale, version);
                if (globals.Sets is null)
                {
                    throw new InvalidOperationException("Couldn't patch in a set array. Therefore, can't create a catalog.");
                }
            }

            // Shim to add Bandle City during spoiler season.
            globals.Regions = globals.Regions!.Append(new DdRegion
            {
                Name = "Bandle City", // en-US only
                NameRef = "BandleCity",
                Abbreviation = "BC",
                IconAbsolutePath = "https://dd.b.pvp.net/2_14_0/core/en_us/img/regions/icon-bandlecity.png", // en-US only
            }).ToArray();

            // Shim to add set 5 during spoiler season.
            globals.Sets = globals.Sets.Append(new DdSet
            {
                Name    = "Beyond the BandleWood", // en-US only
                NameRef = "Set5",
                IconAbsolutePath = "http://dd.b.pvp.net/2_14_0/core/en_us/img/sets/set4.png", // en-US only
            }).ToArray();

            var setIndices = new Dictionary<string, int>();
            var cards = new List<DdCard>();
            foreach (var set in globals.Sets)
            {
                int? setNumber;
                if (set.NameRef is not null &&
                    set.NameRef.StartsWith("Set") &&
                    int.TryParse(set.NameRef.Substring(3), out int s))
                {
                    setNumber = s;
                }
                else
                {
                    setNumber = null;
                }

                if (setNumber.HasValue)
                {
                    setIndices.Add(set.NameRef!, setNumber.Value);
                }

                IReadOnlyList<DdCard>? setCardList = await DdFetcher.FetchSetCards(locale, version, set);
                if (setCardList is null)
                {
                    // There may be "special" sets listed in the manifest that we don't "care" about (e.g. "EventSet").
                    // It's only an exception if we can't fetch cards for a normal, numbered set.
                    if (setNumber is not null)
                    {
                        throw new InvalidOperationException($"Couldn't fetch Data Dragon cards for {set.NameRef}. Therefore, can't create a catalog.");
                    }
                    else
                    {
                        Logger.LogInformation($"Couldn't fetch Data Dragon cards for {set.NameRef}.");
                    }
                }
                else
                {
                    foreach (DdCard card in setCardList)
                    {
                        card.Set = set.NameRef; // we may or may not need to patch in the set name -- do it regardless
                        cards.Add(card);
                    }
                }
            }

            lock (_syncRoot)
            {
                // Since we're not locking the entire method, check if another caller got here in the time between locks.
                if (!CatCache.TryGetValue((locale, version), out Catalog? cat))
                {
                    var catMaker = new CatalogMaker(locale, version, globals, cards, RegionIndices, setIndices, Logger);
                    cat = catMaker.MakeCatalog();
                    CatCache.Add((locale, version), cat);
                }

                return cat;
            }
        }

        private async Task<DdSet[]?> GetSetsAsync(Locale locale, Version version)
        {
            if (!version.IsEarlierThan(FirstSetDtoVersion))
            {
                Logger.LogError($"Version {version} isn't earlier than version {FirstSetDtoVersion}, when Riot introduced set DTOs. This version should have set DTOs, so it's unclear how to get sets for this version.");
                return null;
            }

            Catalog setsCatalog;
            using (Logger.BeginScope(locale))
            using (Logger.BeginScope(version))
            {
                try
                {
                    setsCatalog = await GetCatalog(locale, FirstSetDtoVersion);
                }
                catch
                {
                    Logger.LogError($"Couldn't get a catalog for version {FirstSetDtoVersion}, when Riot introduced set DTOs. Therefore, can't find a catalog with sets to reference for version {version}.");
                    return null;
                }
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

            var ddSets = new DdSet[setCount];
            for (var i = 0; i < setCount; i++)
            {
                string setNameRef = $"Set{i + 1}";
                if (setsCatalog.Sets.TryGetValue(setNameRef, out LorSet? set))
                {
                    ddSets[i] = new DdSet
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

        ~BasicCatalogService()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
