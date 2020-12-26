namespace Bjerg
{
    public class CardCode
    {
        private readonly string _code;

        public int Set { get; }

        public string Faction { get; }

        public int Number { get; }

        // ReSharper disable once InconsistentNaming
        public int TNumber { get; }

        private CardCode(string code, int set, string faction, int number, int tNumber)
        {
            _code = code;
            Set = set;
            Faction = faction;
            Number = number;
            TNumber = tNumber;
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
    }
}
