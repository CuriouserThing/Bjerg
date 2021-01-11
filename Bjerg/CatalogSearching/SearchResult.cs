using System;
using System.Collections.Generic;

namespace Bjerg.CatalogSearching
{
    public class SearchResult<T> where T : class
    {
        public SearchResult(string searchTerm, Catalog searchCatalog, IReadOnlyList<ItemMatch<T>>? matches = null)
        {
            SearchTerm = searchTerm;
            SearchCatalog = searchCatalog;
            Matches = matches ?? Array.Empty<ItemMatch<T>>();
        }

        public string SearchTerm { get; }

        public Catalog SearchCatalog { get; }

        public IReadOnlyList<ItemMatch<T>> Matches { get; }
    }
}
