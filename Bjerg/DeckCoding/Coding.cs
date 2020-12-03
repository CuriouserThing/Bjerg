using System;
using System.Collections.Generic;
using System.Linq;
using static Bjerg.DeckCoding.VarintHelper;

namespace Bjerg.DeckCoding
{
    public static class Coding
    {
        private const int _currentFormat = 1;

        private const int _currentVersion = 2;

        private const int _maxVersion = 2;

        private const int _maxCountGroup = 3; // 3x, 2x, 1x

        private const byte _formatAndVersion = (_currentFormat << 4) & _currentVersion;

        public static IReadOnlyList<RawCardAndCount> GetDeckCardsFromCode(string code)
        {
            byte[] bytes;
            try
            {
                bytes = Base32Helper.Decode(code);
            }
            catch
            {
                throw new ArgumentException("Deck code isn't valid Base32. Can't decode.");
            }
            return GetDeckCardsFromCodeBytes(bytes);
        }

        private static IReadOnlyList<RawCardAndCount> GetDeckCardsFromCodeBytes(byte[] bytes)
        {
            var span = new ReadOnlySpan<byte>(bytes);

            _ = span[0] >> 4; // format (unused)
            int version = span[0] & 0b1111; // version
            span = span[1..];

            if (version > _maxVersion)
            {
                throw new ArgumentException($"Can't decode deck code of version {version}. This coder only recognizes a max version of {_maxVersion}.");
            }

            try
            {
                return GetDeckCardsFromCodeBytes(span);
            }
            catch (Exception ex)
            {
                throw new ArgumentException("Can't decode a list of cards from deck code.", ex);
            }
        }

        private static IReadOnlyList<RawCardAndCount> GetDeckCardsFromCodeBytes(ReadOnlySpan<byte> span)
        {
            var cards = new List<RawCardAndCount>();

            for (int c = _maxCountGroup; c > 0; c--) // card count
            {
                int g = PopVarint(ref span); // # of set-faction groups in card count group
                for (int i = 0; i < g; i++)
                {
                    int h = PopVarint(ref span); // # of cards in set-faction group
                    int s = PopVarint(ref span); // card set
                    int f = PopVarint(ref span); // card faction
                    for (int j = 0; j < h; j++)
                    {
                        int n = PopVarint(ref span); // card number
                        cards.Add(new RawCardAndCount(s, f, n, c));
                    }
                }
            }

            while (span.Length > 0)
            {
                int c = PopVarint(ref span); // card count
                int s = PopVarint(ref span); // card set
                int f = PopVarint(ref span); // card faction
                int n = PopVarint(ref span); // card number
                cards.Add(new RawCardAndCount(s, f, n, c));
            }

            return cards;
        }

        public static string GetCodeFromDeckCards(IReadOnlyList<RawCardAndCount> cards)
        {
            byte[] bytes = GetCodeBytesFromDeckCards(cards);
            return Base32Helper.Encode(bytes);
        }

        private static byte[] GetCodeBytesFromDeckCards(IReadOnlyList<RawCardAndCount> cards)
        {
            // Group cards by their count; the zeroth group collects all cards above the max count-group threshold
            var countGroups = new List<RawCardAndCount>[_maxCountGroup + 1];
            for (int i = 0; i < countGroups.Length; i++)
            {
                countGroups[i] = new List<RawCardAndCount>();
            }
            foreach (RawCardAndCount rcc in cards)
            {
                if (rcc.Count > _maxCountGroup)
                {
                    countGroups[0].Add(rcc);
                }
                else
                {
                    countGroups[rcc.Count].Add(rcc);
                }
            }

            // Initialize bytes with format + version byte
            var bytes = new List<byte> { _formatAndVersion };

            // Push the count-groups in descending order
            for (int c = _maxCountGroup; c > 0; c--)
            {
                // Group cards by set + faction
                RawCardAndCount[][] sfCardLists = countGroups[c]
                    .GroupBy(rcc => (rcc.Set, rcc.Faction))
                    .Select(g => g.ToArray())
                    .ToArray();

                // Sort each card list separately, *then* sort the list of lists
                foreach (RawCardAndCount[] sfCards in sfCardLists)
                {
                    Array.Sort(sfCards, CompareCards);
                }
                Array.Sort(sfCardLists, CompareCardLists);

                // Push the card lists
                PushVarint(bytes, sfCardLists.Length);
                foreach (RawCardAndCount[] sfCards in sfCardLists)
                {
                    PushVarint(bytes, sfCards.Length);
                    PushVarint(bytes, sfCards[0].Set);
                    PushVarint(bytes, sfCards[0].Faction);
                    foreach (RawCardAndCount rcc in sfCards)
                    {
                        PushVarint(bytes, rcc.Number);
                    }
                }
            }

            // Push the other cards individually, first sorted by set + faction + number
            IEnumerable<RawCardAndCount> xNCardsSorted = countGroups[0]
                .OrderBy(rcc => rcc.Set)
                .ThenBy(rcc => rcc.Faction)
                .ThenBy(rcc => rcc.Number);
            foreach (RawCardAndCount rcc in xNCardsSorted)
            {
                PushVarint(bytes, rcc.Count);
                PushVarint(bytes, rcc.Set);
                PushVarint(bytes, rcc.Faction);
                PushVarint(bytes, rcc.Number);
            }

            // Done!
            return bytes.ToArray();
        }

        private static int CompareCards(RawCardAndCount x, RawCardAndCount y)
        {
            // Sort by card number
            return x.Number - y.Number;
        }

        private static int CompareCardLists(RawCardAndCount[] xs, RawCardAndCount[] ys)
        {
            // Sort primarily by list length; secondarily by card number of the first card (assumes the cards are sorted already!)
            int d = xs.Length - ys.Length;
            return d != 0 ? d : xs[0].Number - ys[0].Number;
        }
    }
}
