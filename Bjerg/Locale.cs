using System;
using System.Globalization;

namespace Bjerg
{
    public class Locale : IEquatable<Locale>
    {
        public Locale(string language, string country)
        {
            Language = language.ToLower();
            Country = country.ToUpper();
        }

        /// <summary>
        ///     ISO-639-1 (always lower-case)
        /// </summary>
        public string Language { get; }

        /// <summary>
        ///     ISO 3166-1 alpha-2 (always upper-case)
        /// </summary>
        public string Country { get; }

        private string IsoName => $"{Language}-{Country}";

        public CultureInfo GetCultureInfo()
        {
            try
            {
                return new CultureInfo(IsoName, false);
            }
            catch (CultureNotFoundException)
            {
                // Fallback on an English-like culture with no fuss
                return CultureInfo.InvariantCulture;
            }
        }

        public override string ToString()
        {
            return IsoName;
        }

        #region Equality

        public override int GetHashCode()
        {
            return Hasher.Start
                .HashNullable(Language)
                .HashNullable(Country);
        }

        public bool Equals(Locale? other)
        {
            return !(other is null)
                   && Language.Equals(other.Language)
                   && Country.Equals(other.Country);
        }

        public override bool Equals(object? obj)
        {
            return obj is Locale other && Equals(other);
        }

        public static bool operator ==(Locale a, Locale b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Locale a, Locale b)
        {
            return !a.Equals(b);
        }

        #endregion
    }
}
