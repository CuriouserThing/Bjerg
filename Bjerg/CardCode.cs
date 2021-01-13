using System;

namespace Bjerg
{
    public class CardCode : IEquatable<CardCode>, IComparable<CardCode>
    {
        private readonly string _code;

        private CardCode(string code, int set, string faction, int number, int tNumber)
        {
            _code = code;
            Set = set;
            Faction = faction;
            Number = number;
            TNumber = tNumber;
        }

        public int Set { get; }

        public string Faction { get; }

        public int Number { get; }

        // ReSharper disable once InconsistentNaming
        public int TNumber { get; }

        public int CompareTo(CardCode? other)
        {
            return string.Compare(_code, other?._code, StringComparison.Ordinal);
        }

        public static bool TryFromString(string code, out CardCode? outCode)
        {
            // Card code constants
            const int fStart = 2;
            const int nStart = 4;
            const char tnDelimiter = 'T';

            outCode = null;

            // Try to parse the set
            if (code.Length < fStart) { return false; }

            if (!int.TryParse(code[..fStart], out int set)) { return false; }

            // Try to parse the faction
            if (code.Length < nStart) { return false; }

            string faction = code[fStart..nStart];

            // Try to parse the remainder (number and possibly T number)
            if (code.Length <= nStart) { return false; }

            string remainder = code[nStart..];
            string nString;
            var tNumber = 0;
            int d = remainder.IndexOf(tnDelimiter);
            if (d == -1)
            {
                // e.g. 01IO048
                nString = remainder;
            }
            else
            {
                // e.g. 01IO048T1
                nString = remainder[..d];

                if (remainder.Length > d + 1)
                {
                    string tnString = remainder[(d + 1)..];
                    if (!int.TryParse(tnString, out tNumber)) { return false; }
                }
            }

            if (!int.TryParse(nString, out int number)) { return false; }

            outCode = new CardCode(code, set, faction, number, tNumber);
            return true;
        }

        public override string ToString()
        {
            return _code;
        }

        public static implicit operator string(CardCode code)
        {
            return code.ToString();
        }

        #region Equality

        public override int GetHashCode()
        {
            return Hasher.Start
                .HashNullable(_code);
        }

        public bool Equals(CardCode? other)
        {
            return !(other is null)
                   && _code.Equals(other._code);
        }

        public override bool Equals(object? obj)
        {
            return obj is CardCode other && Equals(other);
        }

        public static bool operator ==(CardCode a, CardCode b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(CardCode a, CardCode b)
        {
            return !a.Equals(b);
        }

        #endregion
    }
}
