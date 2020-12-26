using System.Globalization;
using Bjerg.DataDragon;

namespace Bjerg.Lor
{
    public class LorSpellSpeed : LorTerm
    {
        public LorSpellSpeed(string key, string name) : base(key)
        {
            Name = name;
        }

        public string Name { get; }

        internal static bool TryFromDataDragon(DdSpellSpeed ddSpellSpeed, TextInfo textInfo, out LorSpellSpeed? spellSpeed)
        {
            if (string.IsNullOrWhiteSpace(ddSpellSpeed.NameRef) || ddSpellSpeed.Name is null)
            {
                spellSpeed = null;
                return false;
            }
            else
            {
                string name = textInfo.ToTitleCase(textInfo.ToLower(ddSpellSpeed.Name.Trim()));
                spellSpeed = new LorSpellSpeed(ddSpellSpeed.NameRef, name);
                return true;
            }
        }

        public override string ToString()
        {
            return $"{Name} ({Key})";
        }
    }
}
