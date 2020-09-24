namespace Bjerg.ClientApi
{
    public class ExpeditionsState
    {
        #region Properties

        public bool IsActive { get; set; }

        public string? State { get; set; }

        public string[]? Record { get; set; }

        public object[]? DraftPicks { get; set; }

        public string[]? Deck { get; set; }

        public int Games { get; set; }

        public int Wins { get; set; }

        public int Losses { get; set; }

        #endregion

        public override string ToString()
        {
            return IsActive ? $"Active expedition ({State}, {Wins}-{Losses})" : "Inactive expedition";
        }
    }
}
