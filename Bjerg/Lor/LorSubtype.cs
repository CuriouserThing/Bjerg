namespace Bjerg.Lor
{
    public class LorSubtype
    {
        public string Name { get; }

        public LorSubtype(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
