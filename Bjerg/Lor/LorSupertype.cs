namespace Bjerg.Lor
{
    public class LorSupertype
    {
        public LorSupertype(string name)
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
