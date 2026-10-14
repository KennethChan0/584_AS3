public class History
{
    private List<Move> movesHistory = new List<Move>();
    private List<Move> redoHistory = new List<Move>();

    public List<Move> GetMovesHistory() => new List<Move>(movesHistory);
    public List<Move> GetRedoHistory() => new List<Move>(redoHistory);

    public void AddMove(Move move)
    {
        movesHistory.Add(move);
        redoHistory.Clear();
    }
    // check move/redo history is possible
    public bool CanUndo() => movesHistory.Count >= 2;
    public bool CanRedo() => redoHistory.Count >= 2;

    public List<Move> Undo()
    {
        List<Move> undo = new List<Move>();
        int lastIndex;
        int lastSecondIndex;
        Move olderMoves;
        Move newerMoves;

        /* movesHistory: m1 m2 m3 m4 m5 m6
                 movesHistory    redoHistory     return
        undo:    m1 m2 m3 m4     m5 m6           m6 m5
        undo:    m1 m2           m5 m6 m3 m4     m4 m3
        redo:    m1 m2 m3 m4     m5 m6           m3 m4
        addMove: m1 m2 m3 m4 m7  empty
        */
        if(CanUndo())
        {
            lastIndex = movesHistory.Count - 1;
            lastSecondIndex = movesHistory.Count - 2;
            olderMoves = movesHistory[lastSecondIndex];
            newerMoves = movesHistory[lastIndex];

            movesHistory.RemoveRange(lastSecondIndex, 2); //m3 m4
            redoHistory.Add(olderMoves);//m3
            redoHistory.Add(newerMoves);//m4
            undo.Add(newerMoves);//m4
            undo.Add(olderMoves);//m3 gameSession should take the newer move then the older move
        }
        return undo;

    }

    //mirror undo
    public List<Move> Redo()
    {
        List<Move> redo = new List<Move>();
        int lastIndex;
        int lastSecondIndex;
        Move olderMoves;
        Move newerMoves;

        if(CanRedo())
        {
            lastIndex = redoHistory.Count -1;
            lastSecondIndex = redoHistory.Count -2;
            olderMoves = redoHistory[lastSecondIndex];
            newerMoves = redoHistory[lastIndex];
            redoHistory.RemoveRange(lastSecondIndex, 2);
            movesHistory.Add(olderMoves);
            movesHistory.Add(newerMoves);
            redo.Add(olderMoves);
            redo.Add(newerMoves);
        }
        return redo;
    }
}
