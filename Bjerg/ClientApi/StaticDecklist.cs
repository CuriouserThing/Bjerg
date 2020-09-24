using System.Collections.Generic;

namespace Bjerg.ClientApi
{
    public class StaticDecklist
    {
        public string? DeckCode { get; set; }

        public Dictionary<string, int>? CardsInDeck { get; set; }
    }
}
