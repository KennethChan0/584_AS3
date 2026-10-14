public class History
{
    private List<ICommand> movesHistory = new List<ICommand>();
    private List<ICommand> redoHistory = new List<ICommand>();

    public List<Move> GetMovesHistory()
    {
        List<Move> moveList = new();
        foreach(ICommand command in movesHistory)
            moveList.Add(command.Move);
        return moveList;
    }

    public List<Move> GetRedoHistory()
    {
        List<Move> redoMoveList = new();
        foreach(ICommand command in redoHistory)
            redoMoveList.Add(command.Move);
        return redoMoveList;
    }
    public void AddCommand(ICommand command)
    {
        movesHistory.Add(command);
        redoHistory.Clear();
    }
    // check move/redo history is possible
    public bool CanUndo() => movesHistory.Count >= 2;
    public bool CanRedo() => redoHistory.Count >= 2;

    public List<ICommand> Undo()
    {
        List<ICommand> undo = new List<ICommand>();
        int lastIndex;
        int lastSecondIndex;
        ICommand olderMoves;
        ICommand newerMoves;

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
    public List<ICommand> Redo()
    {
        List<ICommand> redo = new List<ICommand>();
        int lastIndex;
        int lastSecondIndex;
        ICommand olderMoves;
        ICommand newerMoves;

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
