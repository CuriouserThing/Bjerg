using System;
using System.Collections.Generic;
using Bjerg.Lor;

namespace Bjerg
{
    public class CodeOnlyCard : ICard
    {
        public CodeOnlyCard(CardCode code, Locale locale, Version version, LorFaction? region = null, LorSet? set = null)
        {
            Code = code;
            Locale = locale;
            Version = version;
            Region = region;
            Set = set;
        }

        public CardCode Code { get; }

        public Locale Locale { get; }

        public Version Version { get; }

        public LorFaction? Region { get; }

        public LorSet? Set { get; }

        public string? Name => default;
        public LorSupertype? Supertype => default;
        public LorType? Type => default;
        public IReadOnlyList<LorSubtype> Subtypes { get; } = Array.Empty<LorSubtype>();
        public LorSpellSpeed? SpellSpeed => default;
        public IReadOnlyList<LorKeyword> Keywords { get; } = Array.Empty<LorKeyword>();
        public LorRarity? Rarity => default;
        public int Cost => default;
        public int Attack => default;
        public int Health => default;
        public bool Collectible => default;
        public string? ArtistName => default;
        public string? Description => default;
        public string? DescriptionRaw => default;
        public string? LevelupDescription => default;
        public string? LevelupDescriptionRaw => default;
        public string? FlavorText => default;
        public Uri? GameArtPath => default;
        public Uri? FullArtPath => default;
        public IReadOnlyList<ICard> AssociatedCards { get; } = Array.Empty<BasicCard>();

        public override string ToString()
        {
            return $"[Unknown card] ({Code}, {Locale}, v{Version})";
        }
    }
}
