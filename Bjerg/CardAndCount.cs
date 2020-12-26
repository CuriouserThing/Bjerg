namespace Bjerg
{
    public class CardAndCount
    {
        public CardAndCount(ICard card, int count)
        {
            Card = card;
            Count = count;
        }

        public ICard Card { get; }

        public int Count { get; }

        public void Deconstruct(out ICard card, out int count)
        {
            card = Card;
            count = Count;
        }
    }
}
