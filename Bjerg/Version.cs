using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bjerg
{
    public class Version : IEquatable<Version>
    {
        public Version(params int[] numbers)
        {
            if (numbers.Length == 0)
            {
                throw new ArgumentException(null, nameof(Numbers));
            }

            Numbers = numbers;
        }

        public IReadOnlyList<int> Numbers { get; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder().Append(Numbers[0]);
            for (var i = 1; i < Numbers.Count; i++)
            {
                _ = sb.Append($".{Numbers[i]}");
            }

            return sb.ToString();
        }

        private int GetTrimmedLength()
        {
            var n = 0;
            for (var i = 0; i < Numbers.Count; i++)
            {
                if (Numbers[i] != 0)
                {
                    n = i + 1;
                }
            }

            return n;
        }

        /// <summary>
        ///     Determine whether two versions are equal after trimming trailing zeros from both.
        /// </summary>
        public bool IsEquivalentTo(Version other)
        {
            int ta = GetTrimmedLength();
            int tb = other.GetTrimmedLength();

            if (ta != tb)
            {
                return false;
            }

            for (var i = 0; i < ta; i++)
            {
                if (Numbers[i] != other.Numbers[i])
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        ///     Determine whether this version is earlier than another version.
        /// </summary>
        public bool IsEarlierThan(Version other)
        {
            int ta = GetTrimmedLength();
            int tb = other.GetTrimmedLength();
            int tm = Math.Min(ta, tb);

            for (var i = 0; i < tm; i++)
            {
                if (Numbers[i] < other.Numbers[i])
                {
                    return true;
                }

                if (Numbers[i] > other.Numbers[i])
                {
                    return false;
                }
            }

            return tb > ta; // if the other version has additional non-zero numbers it must be later
        }

        /// <summary>
        ///     Determine whether this version is later than another version.
        /// </summary>
        public bool IsLaterThan(Version other)
        {
            int ta = GetTrimmedLength();
            int tb = other.GetTrimmedLength();
            int tm = Math.Min(ta, tb);

            for (var i = 0; i < tm; i++)
            {
                if (Numbers[i] > other.Numbers[i])
                {
                    return true;
                }

                if (Numbers[i] < other.Numbers[i])
                {
                    return false;
                }
            }

            return ta > tb; // if this version has additional non-zero numbers it must be later
        }

        #region Equality

        public override int GetHashCode()
        {
            return Hasher.Start
                .HashSequence(Numbers);
        }

        public bool Equals(Version? other)
        {
            return !(other is null)
                   && Numbers.SequenceEqual(other.Numbers);
        }

        public override bool Equals(object? obj)
        {
            return obj is Version other && Equals(other);
        }

        public static bool operator ==(Version a, Version b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Version a, Version b)
        {
            return !a.Equals(b);
        }

        #endregion
    }
}
