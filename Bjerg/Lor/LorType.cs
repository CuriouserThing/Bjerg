namespace Bjerg.Lor
{
    public class LorType
    {
        public LorType(string name)
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
