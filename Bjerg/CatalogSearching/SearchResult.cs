using System;
using System.Collections.Generic;

namespace Bjerg.CatalogSearching
{
    public class SearchResult<T>
    {
        public SearchResult(string lookup, IReadOnlyList<ItemMatch<T>> matches)
        {
            Lookup = lookup;
            Matches = matches;
        }

        public string Lookup { get; }

        public IReadOnlyList<ItemMatch<T>> Matches { get; }

        public static SearchResult<T> FromFailure(string lookup)
        {
            return new(lookup, Array.Empty<ItemMatch<T>>());
        }
    }
}
