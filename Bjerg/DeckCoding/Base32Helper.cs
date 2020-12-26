using System;
using System.Collections.Generic;
using System.Text;

namespace Bjerg.DeckCoding
{
    internal static class Base32Helper
    {
        private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
        private const string Padding = "=";
        private const int Mask = 0b11111;
        private const int Shift = 5;

        private static readonly Dictionary<char, int> CharMap = new();

        static Base32Helper()
        {
            for (var i = 0; i < Alphabet.Length; i++)
            {
                CharMap[Alphabet[i]] = i;
            }
        }

        public static string Encode(byte[] data)
        {
            // Return empty for empty
            if (data.Length == 0)
            {
                return string.Empty;
            }

            int outLength = (data.Length * 8 + Shift - 1) / Shift;
            var result = new StringBuilder(outLength);

            int buffer = data[0];
            var next = 1;
            var left = 8;
            while (left > 0 || next < data.Length)
            {
                if (left < Shift)
                {
                    if (next < data.Length)
                    {
                        buffer <<= 8;
                        buffer |= data[next++] & 0xff;
                        left += 8;
                    }
                    else
                    {
                        int pad = Shift - left;
                        buffer <<= pad;
                        left += pad;
                    }
                }

                int index = Mask & (buffer >> (left - Shift));
                left -= Shift;
                _ = result.Append(Alphabet[index]);
            }

            return result.ToString();
        }

        public static byte[] Decode(string encoded)
        {
            // Remove whitespace and padding
            encoded = encoded.Trim().Replace(Padding, string.Empty);

            // Return empty for empty
            if (encoded.Length == 0)
            {
                return Array.Empty<byte>();
            }

            // Canonicalize to upper
            encoded = encoded.ToUpper();

            int outLength = encoded.Length * Shift / 8;
            byte[] result = new byte[outLength];

            var buffer = 0;
            var next = 0;
            var left = 0;
            foreach (char c in encoded)
            {
                if (!CharMap.TryGetValue(c, out int n))
                {
                    throw new ArgumentException($"Character {c} in encoded string isn't present in RFC 4648 Base32 alphabet. Can't decode.");
                }

                buffer <<= Shift;
                buffer |= n & Mask;
                left += Shift;
                if (left >= 8)
                {
                    result[next++] = (byte)(buffer >> (left - 8));
                    left -= 8;
                }
            }

            return result;
        }
    }
}
