using Bjerg.DataDragon;
using System;
using System.Globalization;

namespace Bjerg.Lor
{
    public class LorSet : LorTerm
    {
        public string Name { get; }

        public Uri IconPath { get; }

        public LorSet(string key, string name, Uri iconPath) : base(key)
        {
            Name = name;
            IconPath = iconPath;
        }

        internal static bool TryFromDataDragon(DdIconTerm ddSet, TextInfo textInfo, out LorSet? set)
        {
            if (ddSet.NameRef is null || ddSet.Name is null || ddSet.IconAbsolutePath is null)
            {
                set = null;
                return false;
            }
            else
            {
                string name = textInfo.ToTitleCase(ddSet.Name.Trim());
                set = new LorSet(ddSet.NameRef, name, new Uri(ddSet.IconAbsolutePath));
                return true;
            }
        }
    }
}