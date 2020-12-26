namespace Bjerg.ClientApi
{
    public class Screen
    {
        public override string ToString()
        {
            return $"{ScreenWidth} × {ScreenHeight}";
        }

        #region Properties

        public int ScreenWidth { get; set; }

        public int ScreenHeight { get; set; }

        #endregion
    }
}
