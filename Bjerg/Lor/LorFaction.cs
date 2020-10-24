using Bjerg.DataDragon;
using System;
using System.Globalization;

namespace Bjerg.Lor
{
    public class LorFaction : LorTerm
    {
        public string Name { get; }

        public string Abbreviation { get; }

        public Uri IconPath { get; }

        public LorFaction(string key, string name, string abbreviation, Uri iconPath) : base(key)
        {
            Name = name;
            Abbreviation = abbreviation;
            IconPath = iconPath;
        }

        internal static bool TryFromDataDragon(DdRegionTerm ddRegion, TextInfo textInfo, out LorFaction? faction)
        {
            if (ddRegion.NameRef is null || ddRegion.Name is null || ddRegion.Abbreviation is null || ddRegion.IconAbsolutePath is null)
            {
                faction = null;
                return false;
            }
            else
            {
                string name = textInfo.ToTitleCase(ddRegion.Name.Trim());
                string description = ddRegion.Abbreviation.Trim();
                faction = new LorFaction(ddRegion.NameRef, name, description, new Uri(ddRegion.IconAbsolutePath));
                return true;
            }
        }

        public override string ToString()
        {
            return $"{Name} ({Key})";
        }
    }
}
