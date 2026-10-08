public class ReversiFactory : IGameFactory
{
    private const int BoardSize = 8;
    public Board CreateBoard()
    {
        return new NormalBoard(BoardSize);
    }

    public List<Player> CreatePlayer(string mode, string variant)
    {
        var players = new List<Player>
        {
            new HumanPlayer("Player 1", 'X')
        };

        switch (mode)
        {
            case "HvH":
                players.Add(new HumanPlayer("Player 2", 'O'));
                break;

            case "HvC Dumb":
                players.Add(new ComputerPlayer("Computer", 'O', new DumbStrategy()));
                break;

            case "HvC Smart":
                ReversiSmartStrategy strategy;
                switch (variant)
                {
                    case "standard":
                        // flip as many as possible
                        strategy = new ReversiSmartStrategy(flip: 1, corner: 0);
                        break;

                    case "anti":
                        // flip as few as possible, avoid corners, take a corner only if no other move
                        strategy = new ReversiSmartStrategy(flip: -1, corner: -1);
                        break;

                    case "corner":
                        // take a corner whenever possible, otherwise flip the most disks
                        strategy = new ReversiSmartStrategy(flip: 1, corner: 1);
                        break;
                    
                    default:
                        throw new ArgumentException($"Unknown Reversi variant '{variant}'. Use standard, anti or corner.", nameof(variant));
                }
                players.Add(new ComputerPlayer("Computer", 'O', strategy));
                break;

            default:
                throw new ArgumentException($"Unknown mode '{mode}'. Use HvH, HvC Dumb or HvC Smart.", nameof(mode));

        }
        return players;
    }

    public Game CreateGame(string variant, Board board, List<Player> players)
    {
        switch (variant)
        {
            case "standard":
                return new Standard(board, players);

            case "anti":
                return new Anti(board, players);

            case "corner":
                return new Corner(board, players);

            default:
                throw new ArgumentException($"Unknown Reversi variant '{variant}'. Use standard, anti or corner.", nameof(variant));
        }
    }
}
