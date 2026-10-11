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
    // Create a list to store moves and redo moves
    private List<Move> movesHistory = new List<Move>();
    private List <Move> redoHistory = new List<Move>();
    
    // Get the history of moves and redo moves
    public List<Move> GetMovesHistory() => new List<Move>(movesHistory);  

    public List<Move> GetRedoHistory() => new List<Move>(redoHistory);

    // Add a move to the history and clear the redo history(undo two times and then make a new move, the redo history will be cleared)
    public void AddMove(Move move)
    {
        movesHistory.Add(move);
        redoHistory.Clear();
    }

    // Check if undo and redo are possible
    public bool CanUndo() => movesHistory.Count >= 2;
    public bool CanRedo() => redoHistory.Count >= 2;

    public List<Move> UndoMove()
    {
        if (movesHistory.Count >= 2)
        {
            int lastIndex = movesHistory.Count - 1; //m4
            int lastSecondIndex = movesHistory.Count - 2; //m3
            Move olderMoves = movesHistory[lastSecondIndex]; //m3
            Move newerMoves = movesHistory[lastIndex]; //m4
            movesHistory.RemoveRange(lastSecondIndex, 2); // Remove the last two moves from the history
            redoHistory.Add(olderMoves); // Add m3
            redoHistory.Add(newerMoves); // Add m4
            return new List<Move> { newerMoves, olderMoves }; // GameSession take m4 first and then m3
        }
        else
        {
            return new List<Move>();
        }
    }

    public List<Move> RedoMove()
    {   
        if(redoHistory.Count >= 2)
        {
            int lastIndex = redoHistory.Count - 1; //m2
            int lastSecondIndex = redoHistory.Count - 2; //m1
            Move olderMoves = redoHistory[lastSecondIndex]; //m1
            Move newerMoves = redoHistory[lastIndex]; //m2
            redoHistory.RemoveRange(lastSecondIndex, 2);
            movesHistory.Add(olderMoves);
            movesHistory.Add(newerMoves);
            return new List<Move> { olderMoves, newerMoves };
        }
        else
        {
            return new List<Move>();
        }
    }
    
}