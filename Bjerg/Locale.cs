using System;
using System.Globalization;

namespace Bjerg
{
    public class Locale : IEquatable<Locale>
    {
        /// <summary>
        /// ISO-639-1 (always lower-case)
        /// </summary>
        public string Language { get; }

        /// <summary>
        /// ISO 3166-1 alpha-2 (always upper-case)
        /// </summary>
        public string Country { get; }

        public Locale(string language, string country)
        {
            Language = language.ToLower();
            Country = country.ToUpper();
        }

        private string IsoName => $"{Language}-{Country}";

        public CultureInfo CultureInfo
        {
            get
            {
                try
                {
                    return new CultureInfo(IsoName, useUserOverride: false);
                }
                catch (CultureNotFoundException)
                {
                    // Fallback on an English-like culture with no fuss
                    return CultureInfo.InvariantCulture;
                }
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
                && this.Language.Equals(other.Language)
                && this.Country.Equals(other.Country);
        }

        public override bool Equals(object? obj)
        {
            return obj is Locale other && this.Equals(other);
        }

        public static bool operator ==(Locale a, Locale b) => a.Equals(b);

        public static bool operator !=(Locale a, Locale b) => !a.Equals(b);

        #endregion
    }
}