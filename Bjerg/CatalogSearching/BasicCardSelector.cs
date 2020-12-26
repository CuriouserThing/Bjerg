using System.Collections.Generic;
using System.Linq;

namespace Bjerg.CatalogSearching
{
    /// <summary>
    ///     A no-fuss, batteries-included card selector with criteria that may change from version to version.
    /// </summary>
    public class BasicCardSelector : IItemSelector<ICard>
    {
        public BasicCardSelector(Catalog localCatalog, Catalog homeCatalog)
        {
            LocalCatalog = localCatalog;
            HomeCatalog = homeCatalog;
        }

        public Catalog LocalCatalog { get; }

        public Catalog HomeCatalog { get; }

        public IReadOnlyList<ICard> Reduce(IEnumerable<ICard> cards)
        {
            return cards
                .OrderByDescending(c => c.Collectible) // favor collectibles
                .ThenBy(c => c.Code.TNumber)           // favor "root" cards without a T number (0)
                .ThenBy(c => c.Code)                   // simple non-arbitrary tiebreaker
                .Take(1)
                .ToArray();
        }

        public IReadOnlyList<ICard> Expand(ICard card)
        {
            ICard homeCard = HomeCatalog.Cards[card.Code];

            // Expand the selected card if it's a lv. 1 champ
            if (card.Collectible && card.Code.TNumber == 0 && homeCard.Supertype?.Name == "Champion")
            {
                return ExpandChamp(card, LocalCatalog, HomeCatalog);
            }
            else
            {
                return new[] { card };
            }
        }

        private IReadOnlyList<ICard> ExpandChamp(ICard card, Catalog localCatalog, Catalog homeCatalog)
        {
            // The potential lv. 2 champs for this card are cards that share its code (sans T number)
            CardCode bc = card.Code;
            ICard[] champCards = localCatalog.Cards.Values
                .Where(c =>
                           c.Code.Number == bc.Number &&
                           c.Code.TNumber != 0 &&
                           c.Code.Faction == bc.Faction &&
                           c.Code.Set == bc.Set)
                .ToArray();

            // Of these, we [currently] only recognize the lv. 2 champ as the *single* card that either:

            // - Shares its name with the lv. 1 champ (e.g. Anivia but not Eggnivia)
            ICard[] sameNames = champCards
                .Where(c => c.Name == card.Name)
                .ToArray();
            if (sameNames.Length == 1) { return new[] { card, sameNames[0] }; }

            // - Is a Champion Unit (e.g. Spider Queen Elise)
            ICard[] champUnits = champCards
                .Select(c => homeCatalog.Cards[c.Code])
                .Where(c => c.Supertype?.Name == "Champion" && c.Type?.Name == "Unit")
                .ToArray();
            if (champUnits.Length == 1) { return new[] { card, champUnits[0] }; }

            // ...otherwise, just return the lv. 1 champ
            return new[] { card };
        }
    }
}
