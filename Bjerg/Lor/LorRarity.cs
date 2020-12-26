using System.Globalization;
using Bjerg.DataDragon;

namespace Bjerg.Lor
{
    public class LorRarity : LorTerm
    {
        public LorRarity(string key, string name) : base(key)
        {
            Name = name;
        }

        public string Name { get; }

        internal static bool TryFromDataDragon(DdTerm ddRarity, TextInfo textInfo, out LorRarity? rarity)
        {
            if (string.IsNullOrWhiteSpace(ddRarity.NameRef) || ddRarity.Name is null)
            {
                rarity = null;
                return false;
            }
            else
            {
                string name = textInfo.ToTitleCase(textInfo.ToLower(ddRarity.Name.Trim()));
                rarity = new LorRarity(ddRarity.NameRef, name);
                return true;
            }
        }

        public override string ToString()
        {
            return $"{Name} ({Key})";
        }
    }
}
