using Bjerg.DataDragon;
using System.Globalization;

namespace Bjerg.Lor
{
    public class LorSpellSpeed
    {
        public string Name { get; }

        public LorSpellSpeed(string name)
        {
            Name = name;
        }

        internal static bool TryFromDataDragon(DdTerm ddSpellSpeed, TextInfo textInfo, out LorSpellSpeed? spellSpeed)
        {
            if (ddSpellSpeed.Name is null)
            {
                spellSpeed = null;
                return false;
            }
            else
            {
                string name = textInfo.ToTitleCase(ddSpellSpeed.Name.Trim());
                spellSpeed = new LorSpellSpeed(name);
                return true;
            }
        }
    }
}
