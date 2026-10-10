public abstract class Reversi : IGame
{
    private int currentIndex = 0;
    private const int Empty = 0;
    private Dictionary<Move, List<(int Row, int Col)>> flipped = new Dictionary<Move, List<(int Row, int Col)>>();


    protected Reversi(Board board, List<Player> players)
    {
        Board = board;
        Players = players;

        int mid = board.Size / 2;
        board.PlaceStone(mid, mid, players[1].Id);
        board.PlaceStone(mid, mid + 1, players[0].Id);
        board.PlaceStone(mid + 1, mid, players[0].Id);
        board.PlaceStone(mid + 1, mid + 1, players[1].Id);
    }

    public Board Board { get; }
    public List<Player> Players { get; }
    public abstract string Name { get; }
    public bool IsOver { get; protected set; }
    public Player? Winner { get; protected set; }

    public Player CurrentPlayer
    {
        get { return Players[currentIndex]; }
    }

    public bool Play(Move move)
    {
        if (!ValidateMove(move))
        {
            return false;
        }
        ExecuteMove(move);
        CheckWin();
        return true;
    }

    public bool ValidateMove(Move move)
    {
        if (move.Stone == Move.Pass)
        {
            if (GetPossibleMove(move.player)[0].Stone != Move.Pass)
            {
                return Fail("You still have a legal move, so you cannot PASS.");
            }
            return true;
        }
        if (move.Stone != 'P')
        {
            return Fail("Use P[row]:[col] or PASS in Reversi.");
        }
        if (!IsInsideBoard(move.Row, move.Col))
        {
            return Fail($"({move.Row},{move.Col}) is off the board.");
        }
        if (Board.GetCell(move.Row, move.Col) != Empty)
        {
            return Fail($"({move.Row},{move.Col}) is already taken.");
        }
        if (GetFlips(move.Row, move.Col, move.player).Count == 0)
        {
            return Fail($"A disk at ({move.Row},{move.Col}) would not flip any opponent disk.");
        }
        return true;
    }

    public void ExecuteMove(Move move)
    {
        if (move.Stone != Move.Pass)
        {
            List<(int Row, int Col)> disks = GetFlips(move.Row, move.Col, move.player);
            flipped[move] = disks;
            Board.PlaceStone(move.Row, move.Col, move.player.Id);
            foreach ((int Row, int Col) disk in disks)
            {
                Board.PlaceStone(disk.Row, disk.Col, move.player.Id);
            }
        }
        SwitchPlayer();
    }

    public void UndoMove(Move move)
    {
        if (move.Stone != Move.Pass)
        {
            int opponent = Opponent(move.player).Id;
            foreach ((int Row, int Col) disk in flipped[move])
            {
                Board.PlaceStone(disk.Row, disk.Col, opponent);   // flip back
            }
            flipped.Remove(move);
            Board.RemoveStone(move.Row, move.Col);
        }
        SwitchPlayer();
        IsOver = false;
        Winner = null;
    }

    public void RedoMove(Move move)
    {
        ExecuteMove(move);
    }

    public abstract bool CheckWin();
    
    public List<Move> GetPossibleMove(Player player)
    {
        List<Move> moves = new List<Move>();
        for (int r = 1; r <= Board.Size; r++)
        {
            for (int c = 1; c <= Board.Size; c++)
            {
                if (Board.GetCell(r, c) == Empty && GetFlips(r, c, player).Count > 0)
                {
                    moves.Add(new Move(r, c, 'P', player));
                }
            }
        }
        if (moves.Count == 0)
        {
            moves.Add(new Move(0, 0, Move.Pass, player));
        }
        return moves;
    }

    public List<(int Row, int Col)> GetFlips(int row, int col, Player player)
    {
        List<(int Row, int Col)> flips = new List<(int Row, int Col)>();
        for (int dr = -1; dr <= 1; dr++)
        {
            for (int dc = -1; dc <= 1; dc++)
            {
                if (dr == 0 && dc == 0)
                {
                    continue;
                }
                List<(int Row, int Col)> line = new List<(int Row, int Col)>();
                int r = row + dr;
                int c = col + dc;
                while (IsInsideBoard(r, c) && Board.GetCell(r, c) != Empty && Board.GetCell(r, c) != player.Id)
                {
                    line.Add((r, c));
                    r += dr;
                    c += dc;
                }
                if (line.Count > 0 && IsInsideBoard(r, c) && Board.GetCell(r, c) == player.Id)
                {
                    flips.AddRange(line);
                }
            }
        }
        return flips;
    }

    public void DisplayBoard(Player viewer)
    {
        Board.DisplayBoard();
    }

    public abstract string GetGameInfo();

    protected const string Rules =
        "  Board: 8 x 8. X = Dark (Player 1, moves first), O = Light (Player 2).\n" +
        "  Start: (4,4)=O, (4,5)=X, (5,4)=X, (5,5)=O.\n" +
        "  Place a disk on an empty cell so that it flanks one or more opponent disks\n" +
        "  in a straight line (horizontal, vertical or diagonal) ending in one of your disks.\n" +
        "  All flanked opponent disks flip to your colour.\n" +
        "  Every move must flip at least one disk. If you have no legal move, type PASS.\n" +
        "  The game ends when neither player can move (or the board is full).\n" +
        "  Moves: P3:4 (place a disk at row 3, column 4), PASS\n";

    protected bool NoMovesLeft()
    {
        return GetPossibleMove(Players[0])[0].Stone == Move.Pass && GetPossibleMove(Players[1])[0].Stone == Move.Pass;
    }

    protected int CountDisks(Player player)
    {
        int count = 0;
        for (int r = 1; r <= Board.Size; r++)
        {
            for (int c = 1; c <= Board.Size; c++)
            {
                if (Board.GetCell(r, c) == player.Id)
                {
                    count++;
                }
            }
        }
        return count;
    }

    protected bool DecideWinnerByCount(bool fewestWins)
    {
        int p1 = CountDisks(Players[0]);
        int p2 = CountDisks(Players[1]);
        IsOver = true;
        if (p1 == p2)
        {
            Winner = null;
        }
        else if (p1 > p2)
        {
            Winner = fewestWins ? Players[1] : Players[0];
        }
        else
        {
            Winner = fewestWins ? Players[0] : Players[1];
        }
        return true;
    }

    protected Player Opponent(Player player)
    {
        if (Players[0] == player)
        {
            return Players[1];
        }
        return Players[0];
    }
    
    private bool IsInsideBoard(int row, int col)
    {
        return row >= 1 && row <= Board.Size && col >= 1 && col <= Board.Size;
    }

    private void SwitchPlayer()
    {
        currentIndex = 1 - currentIndex;
    }

    protected static bool Fail(string message)
    {
        Console.WriteLine($"  ! {message}");
        return false;
    }
}
