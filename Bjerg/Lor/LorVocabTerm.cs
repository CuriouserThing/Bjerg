using Bjerg.DataDragon;
using System.Globalization;

namespace Bjerg.Lor
{
    public class LorVocabTerm : LorTerm
    {
        public string Name { get; }

        public string Description { get; }

        public LorVocabTerm(string key, string name, string description) : base(key)
        {
            Name = name;
            Description = description;
        }

        internal static bool TryFromDataDragon(DdVocabTerm ddVocabTerm, TextInfo textInfo, out LorVocabTerm? vocabTerm)
        {
            if (string.IsNullOrWhiteSpace(ddVocabTerm.NameRef)  || ddVocabTerm.Name is null || ddVocabTerm.Description is null)
            {
                vocabTerm = null;
                return false;
            }
            else
            {
                string name = textInfo.ToTitleCase(textInfo.ToLower(ddVocabTerm.Name.Trim()));
                string description = ddVocabTerm.Description.Trim();
                vocabTerm = new LorVocabTerm(ddVocabTerm.NameRef, name, description);
                return true;
            }
        }

        public override string ToString()
        {
            return $"{Name} ({Key})";
        }
    }
}
