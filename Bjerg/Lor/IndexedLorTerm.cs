namespace Bjerg.Lor
{
    public abstract class IndexedLorTerm : LorTerm
    {
        protected IndexedLorTerm(string key, int index) : base(key)
        {
            Index = index;
        }

        /// <summary>
        ///     A numeric index to this term. Consistent across all locales.
        /// </summary>
        /// <value></value>
        public int Index { get; }
    }
}
