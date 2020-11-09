using System;
using System.Collections.Generic;
using System.Text;

namespace Bjerg.DeckCoding
{
    internal static class Base32Helper
    {
        private const string _alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
        private const string _padding = "=";
        private const int _mask = 0b11111;
        private const int _shift = 5;

        private static readonly Dictionary<char, int> _charMap = new Dictionary<char, int>();

        static Base32Helper()
        {
            for (int i = 0; i < _alphabet.Length; i++)
            {
                _charMap[_alphabet[i]] = i;
            }
        }

        public static string Encode(byte[] data)
        {
            // Return empty for empty
            if (data.Length == 0)
            {
                return string.Empty;
            }

            int outLength = (data.Length * 8 + _shift - 1) / _shift;
            var result = new StringBuilder(outLength);

            int buffer = data[0];
            int next = 1;
            int left = 8;
            while (left > 0 || next < data.Length)
            {
                if (left < _shift)
                {
                    if (next < data.Length)
                    {
                        buffer <<= 8;
                        buffer |= data[next++] & 0xff;
                        left += 8;
                    }
                    else
                    {
                        int pad = _shift - left;
                        buffer <<= pad;
                        left += pad;
                    }
                }
                int index = _mask & (buffer >> (left - _shift));
                left -= _shift;
                _ = result.Append(_alphabet[index]);
            }

            return result.ToString();
        }

        public static byte[] Decode(string encoded)
        {
            // Remove whitespace and padding
            encoded = encoded.Trim().Replace(_padding, string.Empty);

            // Return empty for empty
            if (encoded.Length == 0)
            {
                return Array.Empty<byte>();
            }

            // Canonicalize to upper
            encoded = encoded.ToUpper();

            int outLength = encoded.Length * _shift / 8;
            byte[] result = new byte[outLength];

            int buffer = 0;
            int next = 0;
            int left = 0;
            foreach (char c in encoded)
            {
                if (!_charMap.TryGetValue(c, out int n))
                {
                    throw new ArgumentException($"Character {c} in encoded string isn't present in RFC 4648 Base32 alphabet. Can't decode.");
                }
                buffer <<= _shift;
                buffer |= n & _mask;
                left += _shift;
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
