namespace Bjerg
{
    public class CardAndCount
    {
        public ICard Card { get; }

        public int Count { get; }

        public CardAndCount(ICard card, int count)
        {
            Card = card;
            Count = count;
        }

        public void Deconstruct(out ICard card, out int count)
        {
            card = Card;
            count = Count;
        }
    }
}
