namespace Bjerg.Lor
{
    public abstract class IndexedLorTerm : LorTerm
    {
        /// <summary>
        /// A numeric index to this term. Consistent across all locales.
        /// </summary>
        /// <value></value>
        public int Index { get; }

        protected IndexedLorTerm(string key, int index) : base(key)
        {
            Index = index;
        }
    }
}
