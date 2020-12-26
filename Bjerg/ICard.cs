using System;
using System.Collections.Generic;
using Bjerg.Lor;

namespace Bjerg
{
    public interface ICard
    {
        CardCode Code { get; }

        Locale Locale { get; }

        Version Version { get; }

        string? Name { get; }

        LorFaction? Region { get; }

        LorSupertype? Supertype { get; }

        LorType? Type { get; }

        IReadOnlyList<LorSubtype> Subtypes { get; }

        LorSpellSpeed? SpellSpeed { get; }

        IReadOnlyList<LorKeyword> Keywords { get; }

        LorSet? Set { get; }

        LorRarity? Rarity { get; }

        int Cost { get; }

        int Attack { get; }

        int Health { get; }

        bool Collectible { get; }

        string? Description { get; }

        string? DescriptionRaw { get; }

        string? LevelupDescription { get; }

        string? LevelupDescriptionRaw { get; }

        string? FlavorText { get; }

        string? ArtistName { get; }

        Uri? GameArtPath { get; }

        Uri? FullArtPath { get; }

        IReadOnlyList<ICard> AssociatedCards { get; }
    }
}
