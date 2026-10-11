public abstract class Board
{
    public int Rows { get; protected set; }
    public int Columns { get; protected set; }

    protected Board(int rows, int columns)
    {
        Rows = rows;
        Columns = columns;
    }

    public abstract char GetCell(int row, int col);

    public abstract void SetCell(int row, int col, char symbol);

    public abstract bool IsCellEmpty(int row, int col);

    public abstract void Clear();

    public abstract void Display();
}