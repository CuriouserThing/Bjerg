namespace Bjerg.ClientApi
{
    public class PositionalRectangle
    {
        public int Left => TopLeftX;

        public int Top => TopLeftY;

        public int Right => TopLeftX + Width;

        public int Bottom => TopLeftY - Height;

        public int CenterX => TopLeftX + Width / 2;

        public int CenterY => TopLeftY - Height / 2;

        public override string ToString()
        {
            return $"{CardCode}";
        }

        #region Properties

        public int CardId { get; set; }

        public string? CardCode { get; set; }

        public int TopLeftX { get; set; }

        public int TopLeftY { get; set; }

        public int Width { get; set; }

        public int Height { get; set; }

        public bool LocalPlayer { get; set; }

        #endregion
    }
}
