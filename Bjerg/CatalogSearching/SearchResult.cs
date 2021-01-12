using System;
using System.Collections.Generic;

namespace Bjerg.CatalogSearching
{
    public class SearchResult<T> where T : class
    {
        public SearchResult(string searchTerm, Locale searchLocale, Version searchVersion, IReadOnlyList<ItemMatch<T>>? matches = null)
        {
            SearchTerm = searchTerm;
            SearchLocale = searchLocale;
            SearchVersion = searchVersion;
            Matches = matches ?? Array.Empty<ItemMatch<T>>();
        }

        public string SearchTerm { get; }

        public Locale SearchLocale { get; }

        public Version SearchVersion { get; }

        public IReadOnlyList<ItemMatch<T>> Matches { get; }
    }
}
