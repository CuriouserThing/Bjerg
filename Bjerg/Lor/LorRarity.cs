using Bjerg.DataDragon;
using System.Globalization;

namespace Bjerg.Lor
{
    public class LorRarity : LorTerm
    {
        public string Name { get; }

        public LorRarity(string key, string name) : base(key)
        {
            Name = name;
        }

        internal static bool TryFromDataDragon(DdTerm ddRarity, TextInfo textInfo, out LorRarity? rarity)
        {
            if (ddRarity.NameRef is null || ddRarity.Name is null)
            {
                rarity = null;
                return false;
            }
            else
            {
                string name = textInfo.ToTitleCase(ddRarity.Name.Trim());
                rarity = new LorRarity(ddRarity.NameRef, name);
                return true;
            }
        }
    }
}
