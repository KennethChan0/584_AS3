public class Classic : Gomoku
{
    public override string Name => "Classic Gomoku";

    public Classic(Board board, List<Player> players)
        : base(board, players)
    {
    }

    // Validate an ordinary Gomoku move
    public override bool ValidateMove(Move move)
    {
        if (move == null || IsOver)
            return false;

        // Only the current player may make a move
        if (!ReferenceEquals(move.player, CurrentPlayer))
            return false;

        // Classic Gomoku only allows ordinary stones
        if (move.Stone != 'O')
            return false;

        // Check row and column range (1-10)
        if (!IsWithinBounds(move.Row, move.Col))
            return false;

        // The target cell must be empty
        return Board.IsCellEmpty(move.Row, move.Col);
    }

    // Execute a valid move
    public override void ExecuteMove(Move move)
    {
        int value = GetBoardValue(move.Stone, move.player);

        Board.PlaceStone(move.Row, move.Col, value);
    }

    // Undo the most recent move
    public override void UndoMove(Move move)
    {
        Board.RemoveStone(move.Row, move.Col); // Remove the stone from the board

        // Restore the previous player
        SwitchPlayer();

        IsOver = false;
        Winner = null;
    }

    // Redo a previously undone move
    public override void RedoMove(Move move)
    {
        ExecuteMove(move);

        // Advance to the next player
        SwitchPlayer();
    }

    // Check for five consecutive stones
    public override bool CheckWin()
    {
        for (int row = 1; row <= Board.Rows; row++)
        {
            for (int col = 1; col <= Board.Columns; col++)
            {
                int value = Board.GetCell(row, col);

                if (value == 0)
                    continue;

                // Horizontal, vertical and both diagonals
                if (CountStones(row, col, 0, 1, value) >= 5 ||  // Horizontal
                    CountStones(row, col, 1, 0, value) >= 5 ||  // Vertical
                    CountStones(row, col, 1, 1, value) >= 5 ||  // Diagonal down-right
                    CountStones(row, col, 1, -1, value) >= 5)   // Diagonal down-left
                {
                    Winner = (value == 1 || value == 2)
                        ? Players[0]
                        : Players[1];

                    IsOver = true;
                    return true;
                }
            }
        }

        // Draw if the board is full
        if (GetPossibleMoveCount() == 0)
        {
            IsOver = true;
            Winner = null;
            return true;
        }

        IsOver = false;
        Winner = null;
        return false;
    }

    // Count consecutive stones in one direction
    private int CountStones(
        int row, int col,
        int rowDirection, int colDirection,
        int value)
    {
        int count = 0;

        while (IsWithinBounds(row, col) &&
               Board.GetCell(row, col) == value)
        {
            count++;
            row += rowDirection;
            col += colDirection;
        }

        return count;
    }

    // Count remaining empty cells
    private int GetPossibleMoveCount()
    {
        int count = 0;

        for (int row = 1; row <= Board.Rows; row++)
        {
            for (int col = 1; col <= Board.Columns; col++)
            {
                if (Board.IsCellEmpty(row, col))
                    count++;
            }
        }

        return count;
    }

    // Return all legal ordinary moves
    public override List<Move> GetPossibleMove(Player player)
    {
        List<Move> moves = new List<Move>();

        if (IsOver || player == null)
            return moves;

        if (!Players.Contains(player))
            return moves;

        for (int row = 1; row <= Board.Rows; row++)
        {
            for (int col = 1; col <= Board.Columns; col++)
            {
                if (Board.IsCellEmpty(row, col))
                {
                    moves.Add(new Move(row, col, 'O', player));
                }
            }
        }

        return moves;
    }

    // Display the full board
    public override void DisplayBoard(Player viewer)
    {
        Board.DisplayBoard();
    }

    // Return game information for the help system
    public override string GetGameInfo()
    {
        return
            "Players: X and O\n" +
            "Rules: Take turns placing ordinary stones.\n" +
            "Win: Five consecutive stones horizontally, " +
            "vertically or diagonally.\n" +
            "Move example: O5:5";
    }
}
