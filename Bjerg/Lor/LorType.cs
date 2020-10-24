namespace Bjerg.Lor
{
    public class LorType
    {
        public string Name { get; }

        public LorType(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
