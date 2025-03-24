namespace WebApplication1
{
    public class Rooms
    {
        public List<string> Nicks { get; set; } = new(4);
        internal void AddNewClient(string nickname)
        {
            if (Nicks.Count < 3)
                Nicks.Add(nickname);
            else
            {
                StartNewGame(Nicks);
                Nicks = new();
            }
        }

        Action<List<string>, string> proc;
        internal void SetStart(Action<List<string>, string> proc)
        {
            this.proc = proc;
        }

        Dictionary<string, GameWord> games = new();

        private void StartNewGame(List<string> nicks)
        {
            var game = new GameWord { ID = Guid.NewGuid().ToString(),P1 = nicks[0], P2 = nicks[1], P3 = nicks[2], P4 = nicks[3], Turn = 0, Question = "Почему почему?", Word = "Почему".Select(s=> new WordChar { Char = s.ToString()}).ToList()  };
            games.Add(game.ID, game);
            proc(nicks, game.ID);
        }

        public string GetNextPlayer(Turn turn)
        {
            var game = games[turn.GameId];
            string result = string.Empty;

            int currentPlayerIndex = game.Turn;

            if (currentPlayerIndex == 0)
            {
                result = game.P1;
            }
            else if (currentPlayerIndex == 1)
            {
                result = game.P2;
            }
            else if (currentPlayerIndex == 2)
            {
                result = game.P3;
            }
            else
            {
                result = game.P4;
            }

            game.Turn = (currentPlayerIndex + 1) % 4;
            return result;

        }

        internal bool CheckChar(Turn turn)
        {
            bool result = false;
            turn.Char.ToString();
            foreach (var w in games[turn.GameId].Word)
            {
                if (w.Char == turn.Char)
                {
                    w.IsOpened = true;
                    result = true;
                }
            }
            return result;
        }

        internal string MakeTurn(Turn turn)
        {
            string result = string.Empty;
            return result;
        }
    }
}
