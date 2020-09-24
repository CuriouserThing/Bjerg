namespace Bjerg.ClientApi
{
    public class PositionalRectangleState
    {
        public string? PlayerName { get; set; }

        public string? OpponentName { get; set; }

        public string? GameState { get; set; }

        public Screen? Screen { get; set; }

        public PositionalRectangle[]? Rectangles { get; set; }
    }
}
