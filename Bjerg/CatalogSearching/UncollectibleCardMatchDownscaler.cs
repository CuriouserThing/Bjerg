using System;

namespace Bjerg.CatalogSearching
{
    public class UncollectibleCardMatchDownscaler : GlobalItemMatchDownscaler<ICard>
    {
        public UncollectibleCardMatchDownscaler(float uncollectibleFactor, float globalFactor = 1f) : base(globalFactor)
        {
            if (uncollectibleFactor < 0f || uncollectibleFactor > 1f)
            {
                throw new ArgumentOutOfRangeException(nameof(uncollectibleFactor));
            }

            UncollectibleFactor = uncollectibleFactor;
        }

        public float UncollectibleFactor { get; }

        protected override float GetFactor(ICard item)
        {
            float cf = item.Collectible ? 1f : UncollectibleFactor;
            return cf * base.GetFactor(item);
        }
    }
}
