namespace Bjerg.CatalogSearching
{
    public interface IStringMatcherFactory
    {
        IStringMatcher CreateStringMatcher(string source);
    }
}
