namespace Bjerg.Lor
{
    public abstract class LorTerm
    {
        public string Key { get; }

        protected LorTerm(string key)
        {
            Key = key;
        }
    }
}
