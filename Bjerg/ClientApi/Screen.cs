namespace Bjerg.ClientApi
{
    public class Screen
    {
        #region Properties

        public int ScreenWidth { get; set; }

        public int ScreenHeight { get; set; }

        #endregion

        public override string ToString()
        {
            return $"{ScreenWidth} × {ScreenHeight}";
        }
    }
}
