using Bjerg.Lor;

namespace Bjerg.CatalogSearching.Services
{
    public class CatalogItemUnion
    {
        public enum Type
        {
            Card,
            Keyword,
            Deck,
        }

        private CatalogItemUnion(Type type)
        {
            T = type;
        }

        public Type T { get; }

        public ICard? Card { get; private init; }

        public LorKeyword? Keyword { get; private init; }

        public Deck? Deck { get; private init; }

        public static CatalogItemUnion AsCard(ICard card)
        {
            return new(Type.Card) { Card = card };
        }

        public static CatalogItemUnion AsKeyword(LorKeyword keyword)
        {
            return new(Type.Keyword) { Keyword = keyword };
        }

        public static CatalogItemUnion AsDeck(Deck deck)
        {
            return new(Type.Deck) { Deck = deck };
        }

        public static implicit operator CatalogItemUnion(LorKeyword keyword)
        {
            return AsKeyword(keyword);
        }

        public static implicit operator CatalogItemUnion(Deck deck)
        {
            return AsDeck(deck);
        }

        public static implicit operator LorKeyword(CatalogItemUnion union)
        {
            return union.Keyword!;
        }

        public static implicit operator Deck(CatalogItemUnion union)
        {
            return union.Deck!;
        }
    }
}
