namespace Bjerg.CatalogSearching.Services
{
    public class SearchParameters
    {
        public SearchParameters(string searchTerm, Locale searchLocale, Version version)
        {
            SearchTerm = searchTerm;
            SearchLocale = searchLocale;
            Version = version;
        }

        public string SearchTerm { get; }

        public Locale SearchLocale { get; }

        public Version Version { get; }

        public Locale? TranslationLocale { get; init; }
    }
}
