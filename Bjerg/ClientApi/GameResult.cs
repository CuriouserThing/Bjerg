namespace Bjerg.ClientApi
{
    public class GameResult
    {
        #region Properties

        public int GameId { get; set; }

        public bool LocalPlayerWon { get; set; }

        #endregion

        public override string ToString()
        {
            string result = LocalPlayerWon ? "Win" : "Loss";
            return $"Game result: {result}";
        }
    }
}
