using System;
using System.Collections.Generic;
using Bjerg.Lor;

namespace Bjerg
{
    internal class BasicCard : ICard
    {
        private LorFaction? _region;

        public BasicCard(CardCode code, Locale locale, Version version, string? name)
        {
            Code    = code;
            Locale  = locale;
            Version = version;
            Name    = name;
        }

        public CardCode Code { get; }

        public Locale Locale { get; }

        public Version Version { get; }

        public string? Name { get; }

        public LorFaction? Region
        {
            get
            {
                if (Regions is not null && Regions.Count > 0)
                {
                    return Regions[0];
                }
                else
                {
                    return _region;
                }
            }
            internal set => _region = value;
        }

        public IReadOnlyList<LorFaction>? Regions { get; internal set; }

        public LorSupertype? Supertype { get; internal set; }

        public LorType? Type { get; internal set; }

        public IReadOnlyList<LorSubtype> Subtypes { get; internal set; } = Array.Empty<LorSubtype>();

        public LorSpellSpeed? SpellSpeed { get; internal set; }

        public IReadOnlyList<LorKeyword> Keywords { get; internal set; } = Array.Empty<LorKeyword>();

        public LorSet? Set { get; internal set; }

        public LorRarity? Rarity { get; internal set; }

        public int Cost { get; internal set; }

        public int Attack { get; internal set; }

        public int Health { get; internal set; }

        public bool Collectible { get; internal set; }

        public string? ArtistName { get; internal set; }

        public string? Description { get; internal set; }

        public string? DescriptionRaw { get; internal set; }

        public string? LevelupDescription { get; internal set; }

        public string? LevelupDescriptionRaw { get; internal set; }

        public string? FlavorText { get; internal set; }

        public Uri? GameArtPath { get; internal set; }

        public Uri? FullArtPath { get; internal set; }

        public IReadOnlyList<ICard> AssociatedCards { get; internal set; } = Array.Empty<BasicCard>();

        public override string ToString()
        {
            string name = Name ?? "[Unnamed card]";
            return $"{name} ({Code}, {Locale}, v{Version})";
        }
    }
}
