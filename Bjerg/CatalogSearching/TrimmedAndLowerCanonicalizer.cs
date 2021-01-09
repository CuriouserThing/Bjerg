using System.Globalization;

namespace Bjerg.CatalogSearching
{
    public class TrimmedAndLowerCanonicalizer : IStringCanonicalizer
    {
        public string Canonicalize(string str, CultureInfo cultureInfo)
        {
            return str
                .Trim()
                .ToLower(cultureInfo);
        }
    }
}
