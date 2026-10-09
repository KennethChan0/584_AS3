using System.Security;

public class Move
{
    public int Col{get;}
    public int Row{get;}
    public char Stone{get;}
    public Player player{get;} 
    public Move(int row, int col, char stone, Player player)
    {
        Col = col;
        Row = row;
        Stone = stone;
        this.player = player;
    }
}

public class History
{
    public List <Move> movesHistory = new List<Move>();
    public List <Move> redoHistory = new List<Move> ();
    public List<Move> GetMoveHistory()
    {
        return movesHistory;
    }
    public List<Move> GetRedoHistory()
    {
        return redoHistory;
    }

    public void AddMove(Move move)
    {
        movesHistory.Add(move);
        redoHistory.Clear();
    }

        public void UndoMove()
    {
        if(movesHistory.Count < 2)
        {
            return ;
        }
        int LastIndex = movesHistory.Count -2;
        redoHistory.AddRange(movesHistory.GetRange(LastIndex, 2));
        movesHistory.RemoveRange(LastIndex,2);
    }

    public void RedoMove()
    {   
        if (redoHistory.Count < 2)
        {
            return;
        }
        int LastIndex = redoHistory.Count -2;

        movesHistory.AddRange(redoHistory.GetRange(LastIndex, 2));
        redoHistory.RemoveRange(LastIndex,2);
    }
    
}