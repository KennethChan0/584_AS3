public abstract class Board
{
    public int Rows { get; protected set; }
    public int Columns { get; protected set; }

    public int Size => Rows;  //return Rows; //assumes square board

    protected Board(int rows, int columns)
    {
        Rows = rows;
        Columns = columns;
    }

    // Cell values:
    // 0 = Empty
    // 1 = Player 1 ordinary stone (X)
    // 2 = Player 1 heavy stone
    // 3 = Player 2 ordinary stone (O)
    // 4 = Player 2 heavy stone
    // Reversi uses 0, 1 and 3 only.

    public abstract int GetCell(int row, int col);

    public abstract void SetCell(int row, int col, int value);

    public abstract bool IsCellEmpty(int row, int col);

    public abstract void Clear();

    public abstract void Display();
}