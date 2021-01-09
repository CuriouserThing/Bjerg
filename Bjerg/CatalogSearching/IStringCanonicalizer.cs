using System.Globalization;

namespace Bjerg.CatalogSearching
{
    public interface IStringCanonicalizer
    {
        string Canonicalize(string str, CultureInfo cultureInfo);
    }
}
