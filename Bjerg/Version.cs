using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bjerg
{
    public class Version : IEquatable<Version>
    {
        public IReadOnlyList<int> Numbers { get; }

        public Version(params int[] numbers)
        {
            if (numbers.Length == 0)
            {
                throw new ArgumentException(null, nameof(Numbers));
            }
            Numbers = numbers;
        }

        public override string ToString()
        {
            var sb = new StringBuilder(Numbers[0]);
            for (int i = 1; i < Numbers.Count; i++)
            {
                _ = sb.Append($".{Numbers[i]}");
            }
            return sb.ToString();
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
                && this.Numbers.SequenceEqual(other.Numbers);
        }

        public override bool Equals(object? obj)
        {
            return obj is Version other && this.Equals(other);
        }

        public static bool operator ==(Version a, Version b) => a.Equals(b);

        public static bool operator !=(Version a, Version b) => !a.Equals(b);

        #endregion
    }
}
