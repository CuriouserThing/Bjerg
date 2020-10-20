using Bjerg.DataDragon;
using System.Globalization;

namespace Bjerg.Lor
{
    public class LorVocabTerm
    {
        public string Name { get; }

        public string Description { get; }

        public LorVocabTerm(string name, string description)
        {
            Name = name;
            Description = description;
        }

        internal static bool TryFromDataDragon(DdVocabTerm ddVocabTerm, TextInfo textInfo, out LorVocabTerm? vocabTerm)
        {
            if (ddVocabTerm.Name is null || ddVocabTerm.Description is null)
            {
                vocabTerm = null;
                return false;
            }
            else
            {
                string name = textInfo.ToTitleCase(ddVocabTerm.Name.Trim());
                string description = ddVocabTerm.Description.Trim();
                vocabTerm = new LorVocabTerm(name, description);
                return true;
            }
        }
    }
}
