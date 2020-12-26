using System;
using System.Collections.Generic;

namespace Bjerg.CatalogSearching
{
    /// <summary>
    ///     Levenshtein string matcher that tapers off the penalty for missing characters on the bookends of the source string,
    ///     allowing a fuzzy search for substrings within target strings.
    /// </summary>
    public class LevenshteinSubstringMatcher : IStringMatcher
    {
        public LevenshteinSubstringMatcher(string source, float bookend, float bookendTaper)
        {
            if (bookend < 0f || bookend > 1f)
            {
                throw new ArgumentException("", nameof(bookend));
            }

            if (bookendTaper < 0f || bookendTaper > 1f)
            {
                throw new ArgumentException("", nameof(bookendTaper));
            }

            Source = source;
            Bookend = bookend;
            BookendTaper = bookendTaper;

            DicCaches = new[] {new Dictionary<int, float[,]>(), new Dictionary<int, float[,]>()};
        }

        public string Source { get; }

        public float Bookend { get; }

        public float BookendTaper { get; }

        private Dictionary<int, float[,]>[] DicCaches { get; }

        public float GetMatchPct(string target)
        {
            FindDistance(Source, target, Bookend, BookendTaper, out float distance, out float distanceMax);
            return 1f - (distance / distanceMax);
        }

        private void FindDistance(string source, string target, float bookend, float bookendTaper, out float distance, out float distanceMax)
        {
            // A Wagner-Fischer implementation that tapers off the weight of insertions before + after the source string.

            float[,] d = GetDic(0, target.Length);
            float[,] w = GetDic(1, target.Length);

            FillDistances(source, target, bookend, bookendTaper, d, w);

            // The distance
            distance = d[source.Length, target.Length];

            // The max distance between arbitrary permutations of characters the lengths of the source and the target
            distanceMax = source.Length;
            int delta = target.Length - source.Length;
            if (delta > 0)
            {
                distanceMax += d[0, delta];
            }
        }

        private float[,] GetDic(int index, int targetLength)
        {
            if (!DicCaches[index].TryGetValue(targetLength, out float[,]? dic))
            {
                dic = new float[Source.Length + 1, targetLength + 1];
                DicCaches[index].Add(targetLength, dic);
            }

            return dic;
        }

        private static void FillDistances(string source, string target, float bookend, float bookendTaper, float[,] d, float[,] w)
        {
            for (int i = 0; i <= source.Length; i++)
            {
                d[i, 0] = i;
                w[i, 0] = 1f;
            }

            // Overwrite loop values for first (pre-source bookend) and last (post-source bookend) rows
            w[0, 0] = bookend;
            w[source.Length, 0] = bookend;

            for (int j = 1; j <= target.Length; j++)
            {
                float iw = w[0, j - 1];
                d[0, j] = d[0, j - 1] + iw;
                w[0, j] = iw * bookendTaper;
            }

            for (int i = 1; i <= source.Length; i++)
            {
                char ic = source[i - 1];
                bool end = i == source.Length;
                float wMult = end ? bookendTaper : 1f;
                for (int j = 1; j <= target.Length; j++)
                {
                    char jc = target[j - 1];
                    float ins = d[i, j - 1] + (end ? w[i, j - 1] : 1f);
                    float del = d[i - 1, j] + 1f;
                    float sub = d[i - 1, j - 1] + (ic == jc ? 0f : 1f);

                    if (ins <= del && ins <= sub)
                    {
                        d[i, j] = ins;
                        w[i, j] = w[i, j - 1] * wMult;
                    }
                    else if (del <= sub)
                    {
                        d[i, j] = del;
                        w[i, j] = w[i - 1, j];
                    }
                    else
                    {
                        d[i, j] = sub;
                        w[i, j] = w[i - 1, j - 1];
                    }
                }
            }
        }
    }
}
