namespace Bjerg.CatalogSearching
{
    public class ItemMatch<T>
    {
        public ItemMatch(string key, float keyStrength, T item, float itemStrengthMultiplier, float matchStrength)
        {
            Key = key;
            KeyStrength = keyStrength;
            Item = item;
            ItemStrengthMultiplier = itemStrengthMultiplier;
            MatchStrength = matchStrength;
        }

        public string Key { get; }

        public float KeyStrength { get; }

        public T Item { get; }

        public float ItemStrengthMultiplier { get; }

        public float MatchStrength { get; }
    }
}
