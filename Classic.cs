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
        throw new System.NotImplementedException();
    }

    public override void ExecuteMove(Move move)
    {
        throw new System.NotImplementedException();
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
        throw new System.NotImplementedException();
    }

    public override void DisplayBoard(Player viewer)
    {
        throw new System.NotImplementedException();
    }

    public override string GetGameInfo()
    {
        return Name;
    }
}