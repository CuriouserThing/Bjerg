using Bjerg.DataDragon;
using System.Globalization;

namespace Bjerg.Lor
{
    public class LorKeyword : LorTerm
    {
        public string Name { get; }

        public string Description { get; }

        public LorKeyword(string key, string name, string description) : base(key)
        {
            Name = name;
            Description = description;
        }

        internal static bool TryFromDataDragon(DdVocabTerm ddKeyword, TextInfo textInfo, out LorKeyword? keyword)
        {
            if (string.IsNullOrWhiteSpace(ddKeyword.NameRef)  || ddKeyword.Name is null || ddKeyword.Description is null)
            {
                keyword = null;
                return false;
            }
            else
            {
                string name = textInfo.ToTitleCase(textInfo.ToLower(ddKeyword.Name.Trim()));
                string description = ddKeyword.Description.Trim();
                keyword = new LorKeyword(ddKeyword.NameRef, name, description);
                return true;
            }
        }

        public override string ToString()
        {
            return $"{Name} ({Key})";
        }
    }
}
