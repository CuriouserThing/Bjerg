using Bjerg.DataDragon;
using System.Globalization;

namespace Bjerg.Lor
{
    public class LorSpellSpeed : LorTerm
    {
        public string Name { get; }

        public LorSpellSpeed(string key, string name) : base(key)
        {
            Name = name;
        }

        internal static bool TryFromDataDragon(DdTerm ddSpellSpeed, TextInfo textInfo, out LorSpellSpeed? spellSpeed)
        {
            if (ddSpellSpeed.NameRef is null || ddSpellSpeed.Name is null)
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
