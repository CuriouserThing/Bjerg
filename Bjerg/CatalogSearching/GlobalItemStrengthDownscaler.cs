using System;

namespace Bjerg.CatalogSearching
{
    public class GlobalItemStrengthDownscaler<T> : IItemStrengthDownscaler<T>
    {
        public GlobalItemStrengthDownscaler(float multiplier)
        {
            if (multiplier < 0f || multiplier > 1f)
            {
                throw new ArgumentOutOfRangeException(nameof(multiplier));
            }

            Multiplier = multiplier;
        }

        public float Multiplier { get; }

        public float GetMultiplier(T item)
        {
            return Multiplier;
        }
    }
}
