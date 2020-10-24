namespace Bjerg.Lor
{
    public class LorSupertype
    {
        public string Name { get; }

        public LorSupertype(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
