using Bjerg.DataDragon;
using System;
using System.Globalization;

namespace Bjerg.Lor
{
    public class LorFaction
    {
        public string Name { get; }

        public string Abbreviation { get; }

        public Uri IconPath { get; }

        public LorFaction(string name, string abbreviation, Uri iconPath)
        {
            Name = name;
            Abbreviation = abbreviation;
            IconPath = iconPath;
        }

        internal static bool TryFromDataDragon(DdRegionTerm ddRegion, TextInfo textInfo, out LorFaction? faction)
        {
            if (ddRegion.Name is null || ddRegion.Abbreviation is null || ddRegion.IconAbsolutePath is null)
            {
                faction = null;
                return false;
            }
            else
            {
                string name = textInfo.ToTitleCase(ddRegion.Name.Trim());
                string description = ddRegion.Abbreviation.Trim();
                faction = new LorFaction(name, description, new Uri(ddRegion.IconAbsolutePath));
                return true;
            }
        }
    }
}
