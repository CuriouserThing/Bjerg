namespace Bjerg.CatalogSearching
{
    public interface IItemStrengthDownscaler<in T>
    {
        float GetMultiplier(T item);
    }
}
