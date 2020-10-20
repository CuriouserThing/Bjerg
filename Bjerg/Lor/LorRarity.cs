using Bjerg.DataDragon;
using System.Globalization;

namespace Bjerg.Lor
{
    public class LorRarity
    {
        public string Name { get; }

        public LorRarity(string name)
        {
            Name = name;
        }

        internal static bool TryFromDataDragon(DdTerm ddRarity, TextInfo textInfo, out LorRarity? rarity)
        {
            if (ddRarity.Name is null)
            {
                rarity = null;
                return false;
            }
            else
            {
                string name = textInfo.ToTitleCase(ddRarity.Name.Trim());
                rarity = new LorRarity(name);
                return true;
            }
        }
    }
}
