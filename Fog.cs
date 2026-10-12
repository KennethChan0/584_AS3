
using System;
using System.Collections.Generic;

public class Fog : Gomoku
{
    public override string Name => "GomokuFog";

    // Publisher stores its observers
    private readonly List<IObserver> observers = new();

    // Each player has a separate filtered view
    private readonly Dictionary<Player, FogView> playerViews = new();

    public Fog(Board board, List<Player> players)
        : base(board, players)
    {
        // Create and register one observer for each player
        foreach (Player player in Players)
        {
            FogView view = new FogView(player, Board);
            playerViews[player] = view;
            Attach(view);
        }
    }

    // Subscribe an observer
    public void Attach(IObserver observer)
    {
        if (observer == null)
            throw new ArgumentNullException(nameof(observer));

        if (!observers.Contains(observer))
            observers.Add(observer);
    }

    // Unsubscribe an observer
    public void Detach(IObserver observer)
    {
        observers.Remove(observer);
    }

    // Notify subscribers when the true board changes
    public void NotifyObservers()
    {
        foreach (IObserver observer in observers)
        {
            observer.Update(Board);
        }
    }

    // Validate using the TRUE board, not filtered view
    public override bool ValidateMove(Move move)
    {
        if (move == null || IsOver)
            return false;

        if (!ReferenceEquals(move.player, CurrentPlayer))
            return false;

        // Fog only allows ordinary stones
        if (move.Stone != 'O')
            return false;

        if (!IsWithinBounds(move.Row, move.Col))
            return false;

        return Board.IsCellEmpty(move.Row, move.Col);
    }

    // Update the true board, then notify observers
    public override void ExecuteMove(Move move)
    {
        int value = GetBoardValue(move.Stone, move.player);

        Board.PlaceStone(move.Row, move.Col, value);

        NotifyObservers();
    }

    // Restore the true board and refresh both views
    public override void UndoMove(Move move)
    {
        Board.RemoveStone(move.Row, move.Col);

        SwitchPlayer();
        IsOver = false;
        Winner = null;
        LastMove = null;

        NotifyObservers();
    }

    // Reapply an undone move
    public override void RedoMove(Move move)
    {
        if (!ReferenceEquals(move.player, CurrentPlayer))
            throw new InvalidOperationException("Wrong player for redo.");

        if (!ValidateMove(move))
            throw new InvalidOperationException("Cannot redo invalid move.");

        ExecuteMove(move);
        LastMove = move;

        if (!CheckWin())
            SwitchPlayer();
    }

    // Check win conditions on the true board
    public override bool CheckWin()
    {
        int[] directionsRow = { 0, 1, 1, 1 };
        int[] directionsCol = { 1, 0, 1, -1 };

        for (int row = 1; row <= Board.Rows; row++)
        {
            for (int col = 1; col <= Board.Columns; col++)
            {
                int value = Board.GetCell(row, col);

                if (value != 1 && value != 3)
                    continue;

                for (int d = 0; d < 4; d++)
                {
                    int count = 0;
                    int r = row;
                    int c = col;

                    while (IsWithinBounds(r, c) &&
                           Board.GetCell(r, c) == value)
                    {
                        count++;
                        r += directionsRow[d];
                        c += directionsCol[d];
                    }

                    if (count >= 5)
                    {
                        Winner = value == 1 ? Players[0] : Players[1];
                        IsOver = true;
                        return true;
                    }
                }
            }
        }

        // Draw if all cells are occupied
        bool hasEmptyCell = false;

        for (int row = 1; row <= Board.Rows; row++)
        {
            for (int col = 1; col <= Board.Columns; col++)
            {
                if (Board.IsCellEmpty(row, col))
                {
                    hasEmptyCell = true;
                    break;
                }
            }

            if (hasEmptyCell)
                break;
        }

        IsOver = !hasEmptyCell;
        Winner = null;

        return IsOver;
    }

    // Generate possible ordinary moves
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
                    moves.Add(new Move(row, col, 'O', player));
            }
        }

        return moves;
    }

    // Display only the requesting player's filtered view
    public override void DisplayBoard(Player viewer)
    {
        if (!playerViews.TryGetValue(viewer, out FogView? view))
            throw new ArgumentException("Unknown viewer.");

        view.Display();
    }

    public override string GetGameInfo()
    {
        return
            "Only ordinary stones are allowed.\n" +
            "Players see their own stones and adjacent cells.\n" +
            "Other cells are displayed as '?'.\n" +
            "Win: Five consecutive stones horizontally, " +
            "vertically or diagonally.\n" +
            "Move example: O5:5";
    }
}
