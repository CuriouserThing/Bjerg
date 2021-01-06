namespace Bjerg.CatalogSearching
{
    public class ExactStringMatcher : IStringMatcher
    {
        public ExactStringMatcher(string source)
        {
            Source = source;
        }

        public string Source { get; }

        public float GetMatchPct(string target)
        {
            return string.Equals(Source, target) ? 1.0f : 0.0f;
        }

        public class Factory : IStringMatcherFactory
        {
            public IStringMatcher CreateStringMatcher(string source)
            {
                return new ExactStringMatcher(source);
            }
        }
    }
}
