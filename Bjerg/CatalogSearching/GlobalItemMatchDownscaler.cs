using System;

namespace Bjerg.CatalogSearching
{
    public class GlobalItemMatchDownscaler<T> : IItemMatchScaler<T>
    {
        public GlobalItemMatchDownscaler(float globalFactor)
        {
            if (globalFactor < 0f || globalFactor > 1f)
            {
                throw new ArgumentOutOfRangeException(nameof(globalFactor));
            }

            GlobalFactor = globalFactor;
        }

        public float GlobalFactor { get; }

        /// <summary>
        ///     If true, raise the downscaling factor by a power of (1 - m), where m is the match pct. This biases the curve such
        ///     that the factor downscales match pct less and less as m approaches 100%; at 100% there is no downscaling (i.e. a
        ///     perfect match remains a perfect match). If false, downscaling is linear.
        /// </summary>
        public bool PreserveStrongMatches { get; init; } = false;

        public float ScaleMatchPct(T item, float matchPct)
        {
            float f = GetFactor(item);
            if (PreserveStrongMatches)
            {
                f = (float)Math.Pow(f, 1 - matchPct);
            }

            return f * matchPct;
        }

        protected virtual float GetFactor(T item)
        {
            return GlobalFactor;
        }
    }
}
