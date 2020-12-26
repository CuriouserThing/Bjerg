using System;
using System.Globalization;
using Bjerg.DataDragon;

namespace Bjerg.Lor
{
    public class LorFaction : IndexedLorTerm
    {
        public LorFaction(string key, int index, string name, string abbreviation, Uri iconPath) : base(key, index)
        {
            Name = name;
            Abbreviation = abbreviation;
            IconPath = iconPath;
        }

        public string Name { get; }

        public string Abbreviation { get; }

        public Uri IconPath { get; }

        internal static bool TryFromDataDragon(DdRegion ddRegion, int index, TextInfo textInfo, out LorFaction? faction)
        {
            if (string.IsNullOrWhiteSpace(ddRegion.NameRef) || ddRegion.Name is null || ddRegion.Abbreviation is null || ddRegion.IconAbsolutePath is null)
            {
                faction = null;
                return false;
            }
            else
            {
                string name = textInfo.ToTitleCase(textInfo.ToLower(ddRegion.Name.Trim()));
                string description = ddRegion.Abbreviation.Trim();
                faction = new LorFaction(ddRegion.NameRef, index, name, description, new Uri(ddRegion.IconAbsolutePath));
                return true;
            }
        }

        public override string ToString()
        {
            return $"{Name} ({Key})";
        }
    }
}
