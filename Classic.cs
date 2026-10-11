using System.Collections.Generic;

using System.Collections.Generic;

public class Classic : Gomoku
{
    public override string Name => "Classic Gomoku";

    public Classic(Board board, List<Player> players)
        : base(board, players)
    {
    }

    public override bool ValidateMove(Move move)
    {
        return Board.IsCellEmpty(move.Row, move.Col);
    }

    public override void ExecuteMove(Move move)
    {
        Board.SetCell(move.Row, move.Col, move.Stone);
    }

    public override void UndoMove(Move move)
    {
        throw new System.NotImplementedException();
    }

    public override void RedoMove(Move move)
    {
        throw new System.NotImplementedException();
    }

    public override bool CheckWin()
    {
        throw new System.NotImplementedException();
    }

    public override List<Move> GetPossibleMove(Player player)
    {
        List<Move> moves = new List<Move>();

        for (int row = 1; row <= Board.Rows; row++)
        {
            for (int col = 1; col <= Board.Columns; col++)
            {
                if (Board.IsCellEmpty(row, col))
                {
                    Move move = new Move(
                        row,
                        col,
                        player.Symbol,
                        player
                    );

                    moves.Add(move);
                }
            }
        }

        return moves;
    }

    public override void DisplayBoard(Player viewer)
    {
        Board.Display();
    }

    public override string GetGameInfo()
    {
        return Name;
    }
}