public class SaveLoad
{
    public void SaveGame(IGame game, string fileName)
    {
        using (StreamWriter writer = new StreamWriter(fileName))
        {
            writer.WriteLine(game.Name);
            writer.WriteLine(game.GetGameInfo());
            writer.WriteLine(game.GetMovesHistory().Count);
            foreach (var move in game.GetMovesHistory())
            {
                writer.WriteLine(move.ToString());
            }
        }
    }

    public IGame LoadGame(string fileName)
    {
        using (StreamReader reader = new StreamReader(fileName))
        {
            string gameName = reader.ReadLine();
            string gameInfo = reader.ReadLine();
            int movesCount = int.Parse(reader.ReadLine());

            IGame game = GameFactory.CreateGame(gameName, gameInfo);

            for (int i = 0; i < movesCount; i++)
            {
                string moveData = reader.ReadLine();
                Move move = Move.FromString(moveData);
                game.AddMove(move);
            }

            return game;
        }
    }
}