namespace Bjerg.Lor
{
    public class LorSubtype
    {
        public LorSubtype(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
