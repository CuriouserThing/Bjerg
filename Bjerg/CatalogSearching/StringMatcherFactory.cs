using System;

namespace Bjerg.CatalogSearching
{
    public class StringMatcherFactory
    {
        public StringMatcherFactory(Func<string, IStringMatcher> matcherDel)
        {
            MatcherDel = matcherDel;
        }

        private Func<string, IStringMatcher> MatcherDel { get; }

        public IStringMatcher CreateStringMatcher(string source)
        {
            return MatcherDel(source);
        }
    }
}
