using System;
using System.Collections.Generic;

namespace Bjerg.DataDragon
{
    public class DdCard
    {
        public object[]? AssociatedCards { get; set; }

        public string[]? AssociatedCardRefs { get; set; }

        public DdAssets[]? Assets { get; set; }

        public string? Region { get; set; }

        public string? RegionRef { get; set; }

        public int Attack { get; set; }

        public int Cost { get; set; }

        public int Health { get; set; }

        public string? Description { get; set; }

        public string? DescriptionRaw { get; set; }

        public string? LevelupDescription { get; set; }

        public string? LevelupDescriptionRaw { get; set; }

        public string? FlavorText { get; set; }

        public string? ArtistName { get; set; }

        public string? Name { get; set; }

        public string? CardCode { get; set; }

        public string[]? Keywords { get; set; }

        public string[]? KeywordRefs { get; set; }

        public string? SpellSpeed { get; set; }

        public string? SpellSpeedRef { get; set; }

        public string? Rarity { get; set; }

        public string? RarityRef { get; set; }

        public string? Subtype { get; set; }

        public string[]? Subtypes { get; set; }

        public string? Supertype { get; set; }

        public string? Type { get; set; }

        public bool Collectible { get; set; }

        public string? Set { get; set; }

        public override string ToString()
        {
            return $"{Name} ({CardCode})";
        }

        /// <summary>
        /// Get all distinct, non-null, non-whitespace subtypes between <see cref="DdCard.Subtype"/> and <see cref="DdCard.Subtypes"/>, as Riot's usage of these fields is inconsistent.
        /// </summary>
        public IReadOnlyList<string> GetDistinctSubtypes()
        {
            if (string.IsNullOrWhiteSpace(Subtype))
            {
                return Subtypes ?? Array.Empty<string>();
            }
            else if (Subtypes is null)
            {
                return new[] { Subtype };
            }
            else
            {
                var allSubtypes = new List<string>(capacity: 1 + Subtypes.Length) { Subtype };
                for (int i = 0; i < Subtypes.Length; i++)
                {
                    if (!string.IsNullOrWhiteSpace(Subtypes[0]) && Subtypes[0] != Subtype)
                    {
                        allSubtypes.Add(Subtypes[0]);
                    }
                }
                return allSubtypes;
            }
        }
    }
}
