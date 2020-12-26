namespace Bjerg.Lor
{
    public abstract class LorTerm
    {
        protected LorTerm(string key)
        {
            Key = key;
        }

        /// <summary>
        ///     A name reference to this term. Consistent across all locales and never null, empty, or whitespace.
        /// </summary>
        public string Key { get; }
    }
}
