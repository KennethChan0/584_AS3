public class GomokuFactory : IGameFactory
{
    private const int BoardSize = 10;
    public Board CreateBoard()
    {
        return new NormalBoard(BoardSize);
    }

    public List<Player> CreatePlayer(string mode, string variant)
    {
        var players = new List<Player>
        {
            new HumanPlayer("Player 1", 1)
        };

        switch (mode)
        {
            case "HvH":
                players.Add(new HumanPlayer("Player 2", 2));
                break;

            case "HvC Dumb":
                players.Add(new ComputerPlayer("Computer", 2, new DumbGomokuStrategy()));
                break;

            case "HvC Smart":
                players.Add(new ComputerPlayer("Computer", 2, new SmartGomokuStrategy()));
                break;

            default:
                throw new ArgumentException($"Unknown mode '{mode}'. Use HvH, HvC Dumb or HvC Smart.", nameof(mode));

        }
        return players;
    }

    public IGame CreateGame(string variant, Board board, List<Player> players)
    {
        switch (variant)
        {
            case "classic":
                return new Classic(board, players);

            case "plus":
                return new Plus(board, players);

            case "fog":
                return new Fog(board, players);

            default:
                throw new ArgumentException($"Unknown Gomoku variant '{variant}'. Use classic, plus or fog.", nameof(variant));
        }
    }
}
