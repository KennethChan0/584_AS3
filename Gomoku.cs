using System.Collections.Generic;

public abstract class Gomoku : IGame
{
    public Board Board { get; protected set; }
    public List<Player> Players { get; protected set; }

    public abstract string Name { get; }

    public Player CurrentPlayer { get; protected set; }
    public bool IsOver { get; protected set; }
    public Player? Winner { get; protected set; }

    protected Gomoku(Board board, List<Player> players)
    {
        if (board == null)
            throw new System.ArgumentNullException(nameof(board));

        if (players == null)
            throw new System.ArgumentNullException(nameof(players));

        if (players.Count != 2)
            throw new System.ArgumentException(
                "This game requires exactly two players.",
                nameof(players));

        Board = board;
        Players = players;
        CurrentPlayer = players[0];

        IsOver = false;
        Winner = null;
    }

    // Template Method
    public bool Play(Move move)
    {
        if (IsOver)
            return false;

        if (!ValidateMove(move))
            return false;

        ExecuteMove(move);

        if (CheckWin())
        {
            IsOver = true;
            Winner = CurrentPlayer;
            return true;
        }

        SwitchPlayer();

        return true;
    }

    public abstract bool ValidateMove(Move move);

    public abstract void ExecuteMove(Move move);

    public abstract void UndoMove(Move move);

    public abstract void RedoMove(Move move);

    public abstract bool CheckWin();

    public abstract List<Move> GetPossibleMove(Player player);

    public abstract void DisplayBoard(Player viewer);

    public abstract string GetGameInfo();

    protected virtual void SwitchPlayer()
    {
        int currentIndex = Players.IndexOf(CurrentPlayer);
        int nextIndex = (currentIndex + 1) % Players.Count;

        CurrentPlayer = Players[nextIndex];
    }
}