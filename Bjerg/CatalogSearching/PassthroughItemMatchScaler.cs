namespace Bjerg.CatalogSearching
{
    public sealed class PassthroughItemMatchScaler<T> : IItemMatchScaler<T>
    {
        public float ScaleMatchPct(T item, float matchPct)
        {
            return matchPct;
        }
    }
}
