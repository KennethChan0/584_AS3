using System;
using System.Collections.Generic;

public abstract class Gomoku : IGame
{
    public Board Board { get; protected set; }
    public List<Player> Players { get; protected set; }

    public abstract string Name { get; }

    public Player CurrentPlayer { get; protected set; }
    public bool IsOver { get; protected set; }
    public Player? Winner { get; protected set; }

    protected Move? LastMove { get; set; }

    protected Gomoku(Board board, List<Player> players)
    {
        if (board == null)
        {
            throw new ArgumentNullException(nameof(board));
        }

        if (players == null)
        {
            throw new ArgumentNullException(nameof(players));
        }

        if (players.Count != 2)
        {
            throw new ArgumentException(
                "This game requires exactly two players.",
                nameof(players));
        }

        if (players[0] == null || players[1] == null)
        {
            throw new ArgumentException(
                "Players cannot contain null.",
                nameof(players));
        }

        if (ReferenceEquals(players[0], players[1]))
        {
            throw new ArgumentException(
                "The two players must be different.",
                nameof(players));
        }

        Board = board;
        Players = new List<Player>(players);
        CurrentPlayer = Players[0];

        IsOver = false;
        Winner = null;
        LastMove = null;
    }


    // Template Method: shared turn sequence //The parent class defines a fixed sequence of steps, while subclasses implement their own rules
    public bool Play(Move move)
    {
        // Step 1: Check whether the game is still active
        if (move == null || IsOver)
        {
            return false;
        }

        // Step 2: Validate the move
        if (!ValidateMove(move))
        {
            return false;
        }

        // Step 3: Execute the move
        ExecuteMove(move);

        // Step 4: Record the latest move
        LastMove = move;

        // Step 5: Check whether the game has ended
        if (CheckWin())
        {
            // CheckWin is responsible for setting
            // IsOver and Winner, including draws.
            return true;
        }

        // Step 6: Switch to the next player
        SwitchPlayer();

        return true;
    }


    // 'O' means ordinary stone here, not Player 2's display symbol.
    // 'H' means heavy stone.
    protected int GetBoardValue(char stone, Player player)
    {
        if (player == null)
        {
            throw new ArgumentNullException(nameof(player));
        }

        bool isPlayer1 = ReferenceEquals(player, Players[0]);
        bool isPlayer2 = ReferenceEquals(player, Players[1]);

        if (!isPlayer1 && !isPlayer2)
        {
            throw new ArgumentException(
                "The player does not belong to this game.",
                nameof(player));
        }

        switch (stone)
        {
            case 'O':
                return isPlayer1 ? 1 : 3;

            case 'H':
                return isPlayer1 ? 2 : 4;

            case 'E':
                throw new ArgumentException(
                    "An eraser removes a stone; it is not placed on the board.",
                    nameof(stone));

            default:
                throw new ArgumentException(
                    "Unknown stone type.",
                    nameof(stone));
        }
    }

    protected bool IsWithinBounds(int row, int col)
    {
    return row >= 1 &&
           row <= Board.Rows &&
           col >= 1 &&
           col <= Board.Columns;
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
        CurrentPlayer = ReferenceEquals(CurrentPlayer, Players[0])
            ? Players[1]
            : Players[0];
    }
}