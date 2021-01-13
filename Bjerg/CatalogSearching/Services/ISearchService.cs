using System.Threading.Tasks;
using Bjerg.Lor;

namespace Bjerg.CatalogSearching.Services
{
    public interface ISearchService
    {
        private static Task<TranslatedSearchResult<T>> Empty<T>(SearchParameters parameters) where T : class
        {
            var result = TranslatedSearchResult<T>.FromNoSearch(parameters);
            return Task.FromResult(result);
        }

        Task<TranslatedSearchResult<ICard>> FindCard(SearchParameters parameters)
        {
            return Empty<ICard>(parameters);
        }

        Task<TranslatedSearchResult<LorKeyword>> FindKeyword(SearchParameters parameters)
        {
            return Empty<LorKeyword>(parameters);
        }

        Task<TranslatedSearchResult<Deck>> FindDeck(SearchParameters parameters)
        {
            return Empty<Deck>(parameters);
        }

        Task<TranslatedSearchResult<CatalogItemUnion>> FindAnything(SearchParameters parameters)
        {
            return Empty<CatalogItemUnion>(parameters);
        }
    }
}
