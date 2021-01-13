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

        public static SearchResult<T> FromSingleItem(string term, Locale locale, Version version, T item)
        {
            var match = new ItemMatch<T>(term, 1f, item, 1f, 1f);
            return new SearchResult<T>(term, locale, version, new[] { match });
        }
    }
}
