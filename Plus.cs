
using System;
using System.Collections.Generic;

public class Plus : Gomoku
{
    public override string Name => "GomokuPlus";

    // Remaining special stones for each player //dictionaries to track the remaining heavy and eraser stones for each player //heavyInventory and eraserInventory are dictionaries that map each player to their remaining count of heavy and eraser stones, respectively
    //-- = -1, ++ = +1
    private readonly Dictionary<Player, int> heavyInventory = new();
    private readonly Dictionary<Player, int> eraserInventory = new();

    // Remember the original cell value for Undo
    private readonly Dictionary<Move, int> previousCells = new();

    // Default quantities are provisional.
    // Confirm these values against Assignment 2 rules.
    public Plus(Board board, List<Player> players)
        : this(board, players, 1, 1)
    {
    }

    public Plus(Board board, List<Player> players,
                int heavyPerPlayer, int erasersPerPlayer)
        : base(board, players)
    {
        if (heavyPerPlayer < 0 || erasersPerPlayer < 0)
            throw new ArgumentException("Inventory cannot be negative.");

        foreach (Player player in Players)
        {
            heavyInventory[player] = heavyPerPlayer;
            eraserInventory[player] = erasersPerPlayer;
        }
    }

    // Return the remaining Heavy Stones
    public int GetHeavyCount(Player player)
    {
        return heavyInventory[player];
    }

    // Return the remaining Eraser Stones
    public int GetEraserCount(Player player)
    {
        return eraserInventory[player];
    }

    // Validate an ordinary, heavy or eraser move
    public override bool ValidateMove(Move move)
    {
        if (move == null || IsOver)
            return false;

        if (!ReferenceEquals(move.player, CurrentPlayer))
            return false;

        if (!IsWithinBounds(move.Row, move.Col))
            return false;

        switch (move.Stone)
        {
            case 'O':
                return Board.IsCellEmpty(move.Row, move.Col);

            case 'H':
                return GetHeavyCount(move.player) > 0 &&
                    new HeavyStone(move.player.Id)
                        .CanPlace(Board, move.Row, move.Col);

            case 'E':
                return GetEraserCount(move.player) > 0 &&
                    new EraserStone(move.player.Id)
                        .CanPlace(Board, move.Row, move.Col);

            default:
                return false;
        }
    }

    // Execute a valid move
    public override void ExecuteMove(Move move)
    {
        // Save previous cell state for Undo
        previousCells[move] = Board.GetCell(move.Row, move.Col);

        switch (move.Stone)
        {
            case 'O':
                Board.PlaceStone(
                    move.Row, move.Col,
                    GetBoardValue('O', move.player));
                break;

            case 'H':
                Board.PlaceStone(
                    move.Row, move.Col,
                    new HeavyStone(move.player.Id).GetBoardValue());

                heavyInventory[move.player]--;
                break;

            case 'E':
                // The eraser is consumed, not displayed
                Board.RemoveStone(move.Row, move.Col);
                eraserInventory[move.player]--;
                break;
        }
    }

    // Undo a move and restore special inventory
    public override void UndoMove(Move move)
    {
        if (!previousCells.TryGetValue(move, out int oldValue))
            throw new InvalidOperationException(
                "No previous state found for this move.");

        // Restore the cell to its previous value // Restore the cell to its previous value using the stored oldValue
        Board.SetCell(move.Row, move.Col, oldValue);

        if (move.Stone == 'H')
            heavyInventory[move.player]++;

        if (move.Stone == 'E')
            eraserInventory[move.player]++;

        previousCells.Remove(move);

        SwitchPlayer();
        IsOver = false;
        Winner = null;
        LastMove = null;
    }

    // Reapply an undone move
    public override void RedoMove(Move move)
    {
        if (!ReferenceEquals(move.player, CurrentPlayer))
            throw new InvalidOperationException(
                "Wrong player for redo.");

        if (!ValidateMove(move))
            throw new InvalidOperationException(
                "Cannot redo an invalid move.");

        ExecuteMove(move);
        LastMove = move;
        CheckWin();

        if (!IsOver)
            SwitchPlayer();
    }

    // Identify who owns a cell
    private int GetCellOwner(int value)
    {
        if (value == 1 || value == 2)
            return 1;

        if (value == 3 || value == 4)
            return 2;

        return 0;
    }

    // Check five stones belonging to the same player
    public override bool CheckWin()
    {
        for (int row = 1; row <= Board.Rows; row++)
        {
            for (int col = 1; col <= Board.Columns; col++)
            {
                int owner = GetCellOwner(Board.GetCell(row, col));

                if (owner == 0)
                    continue;

                if (CountStones(row, col, 0, 1, owner) >= 5 ||
                    CountStones(row, col, 1, 0, owner) >= 5 ||
                    CountStones(row, col, 1, 1, owner) >= 5 ||
                    CountStones(row, col, 1, -1, owner) >= 5)
                {
                    Winner = owner == 1 ? Players[0] : Players[1];
                    IsOver = true;
                    return true;
                }
            }
        }

        // A full board can still have legal Eraser moves
        if (GetPossibleMove(Players[0]).Count == 0 &&
            GetPossibleMove(Players[1]).Count == 0)
        {
            Winner = null;
            IsOver = true;
            return true;
        }

        Winner = null;
        IsOver = false;
        return false;
    }

    // Count adjacent stones owned by the same player
    private int CountStones(
        int row, int col,
        int rowDirection, int colDirection,
        int owner)
    {
        int count = 0;

        while (IsWithinBounds(row, col) &&
               GetCellOwner(Board.GetCell(row, col)) == owner)
        {
            count++;
            row += rowDirection;
            col += colDirection;
        }

        return count;
    }

    // Generate possible moves for an individual player
    public override List<Move> GetPossibleMove(Player player)
    {
        List<Move> moves = new();

        if (IsOver || !Players.Contains(player))
            return moves;

        for (int row = 1; row <= Board.Rows; row++)
        {
            for (int col = 1; col <= Board.Columns; col++)
            {
                if (Board.IsCellEmpty(row, col))
                {
                    moves.Add(new Move(row, col, 'O', player));

                    if (GetHeavyCount(player) > 0)
                        moves.Add(new Move(row, col, 'H', player));
                }
                else if (GetEraserCount(player) > 0 &&
                         new EraserStone(player.Id)
                             .CanPlace(Board, row, col))
                {
                    moves.Add(new Move(row, col, 'E', player));
                }
            }
        }

        return moves;
    }

    // Display the complete board and remaining inventory
    public override void DisplayBoard(Player viewer)
    {
        Board.DisplayBoard();

        foreach (Player player in Players)
        {
            Console.WriteLine(
                $"{player.Name}: Heavy={GetHeavyCount(player)}, " +
                $"Eraser={GetEraserCount(player)}");
        }
    }

    // Information for the help system
    public override string GetGameInfo()
    {
        return
            "GomokuPlus\n" +
            "Board: 10 x 10\n" +
            "O[row]:[col] - Place an ordinary stone\n" +
            "H[row]:[col] - Place a heavy stone\n" +
            "E[row]:[col] - Erase an opponent's ordinary stone\n" +
            "Heavy stones cannot be erased.\n" +
            "Five consecutive stones owned by one player win.\n" +
            $"Player 1: Heavy={GetHeavyCount(Players[0])}, " +
            $"Eraser={GetEraserCount(Players[0])}\n" +
            $"Player 2: Heavy={GetHeavyCount(Players[1])}, " +
            $"Eraser={GetEraserCount(Players[1])}";
    }
}
