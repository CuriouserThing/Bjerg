using Bjerg.DataDragon;
using System.Globalization;

namespace Bjerg.Lor
{
    public class LorKeyword
    {
        public string Name { get; }

        public string Description { get; }

        public LorKeyword(string name, string description)
        {
            Name = name;
            Description = description;
        }

        internal static bool TryFromDataDragon(DdVocabTerm ddKeyword, TextInfo textInfo, out LorKeyword? keyword)
        {
            if (ddKeyword.Name is null || ddKeyword.Description is null)
            {
                keyword = null;
                return false;
            }
            else
            {
                string name = textInfo.ToTitleCase(ddKeyword.Name.Trim());
                string description = ddKeyword.Description.Trim();
                keyword = new LorKeyword(name, description);
                return true;
            }
        }
    }
}
