using System;
using System.Collections.Generic;

namespace Bjerg.DeckCoding
{
    internal static class VarintHelper
    {
        public static void PushVarint(List<byte> bytes, int value)
        {
            if (value == 0)
            {
                bytes.Add(0);
                return;
            }

            var v = (uint)value;
            while (true)
            {
                uint n = v & 0b01111111;
                v >>= 7;
                if (v == 0)
                {
                    bytes.Add((byte)n);
                    return;
                }
                else
                {
                    bytes.Add((byte)(n | 0b10000000));
                }
            }
        }

        public static int PopVarint(ref ReadOnlySpan<byte> bytes)
        {
            uint varint = 0;
            var shift = 0;

            int i;
            for (i = 0; i < bytes.Length; i++)
            {
                byte b = bytes[i];
                int n = b & 0b01111111; // numeric portion
                int f = b >> 7;         // flag: is there another octet?

                varint |= (uint)n << shift;
                if (f == 0)
                {
                    bytes = bytes[(i + 1)..];
                    return (int)varint;
                }

                shift += 7;
            }

            if (i == 0)
            {
                throw new ArgumentException("Varint array is empty.");
            }
            else
            {
                throw new ArgumentException("Varint array is not terminated with a zero flag.");
            }
        }
    }
}
