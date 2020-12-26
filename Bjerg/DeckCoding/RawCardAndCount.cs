namespace Bjerg.DeckCoding
{
    public class RawCardAndCount
    {
        public RawCardAndCount(int set, int faction, int number, int count)
        {
            Set = set;
            Faction = faction;
            Number = number;
            Count = count;
        }

        public int Set { get; }
        public int Faction { get; }
        public int Number { get; }
        public int Count { get; }
    }
}
