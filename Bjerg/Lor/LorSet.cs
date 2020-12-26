using System;
using System.Globalization;
using Bjerg.DataDragon;

namespace Bjerg.Lor
{
    public class LorSet : IndexedLorTerm
    {
        public LorSet(string key, int index, string name, Uri iconPath) : base(key, index)
        {
            Name = name;
            IconPath = iconPath;
        }

        public string Name { get; }

        public Uri IconPath { get; }

        internal static bool TryFromDataDragon(DdSet ddSet, int index, TextInfo textInfo, out LorSet? set)
        {
            if (string.IsNullOrWhiteSpace(ddSet.NameRef) || ddSet.Name is null || ddSet.IconAbsolutePath is null)
            {
                set = null;
                return false;
            }
            else
            {
                string name = textInfo.ToTitleCase(textInfo.ToLower(ddSet.Name.Trim()));
                set = new LorSet(ddSet.NameRef, index, name, new Uri(ddSet.IconAbsolutePath));
                return true;
            }
        }

        public override string ToString()
        {
            return $"{Name} ({Key})";
        }
    }
}
