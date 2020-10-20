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
    }
}
