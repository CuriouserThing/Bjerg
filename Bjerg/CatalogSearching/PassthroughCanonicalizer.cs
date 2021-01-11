using System.Globalization;

namespace Bjerg.CatalogSearching
{
    public sealed class PassthroughCanonicalizer : IStringCanonicalizer
    {
        public string Canonicalize(string str, CultureInfo cultureInfo)
        {
            return str;
        }
    }
}
