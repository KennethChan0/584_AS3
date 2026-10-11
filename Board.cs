public abstract class Board
{
    public int Rows { get; protected set; }
    public int Columns { get; protected set; }

    // Return the size of a square board
    public int Size => Rows;

    protected Board(int rows, int columns)
    {
        Rows = rows;
        Columns = columns;
    }

    // Cell values:
    // 0 = Empty
    // 1 = Player 1 ordinary stone (X)
    // 2 = Player 1 heavy stone (#)
    // 3 = Player 2 ordinary stone (O)
    // 4 = Player 2 heavy stone (@)
    // Reversi uses 0, 1 and 3 only.

    // Get the value of a cell
    public abstract int GetCell(int row, int col);

    // Set the value of a cell
    public abstract void SetCell(int row, int col, int value);

    // Check whether a cell is empty
    public abstract bool IsCellEmpty(int row, int col);

    // Clear all cells
    public abstract void Clear();

    // Display the board
    public abstract void Display();

    // Place a stone or replace an existing value
    // Reversi uses this method to flip disks
    public virtual void PlaceStone(int row, int col, int value)
    {
        SetCell(row, col, value);
    }

    // Remove a stone from the board
    // Used for Eraser Stone and Undo
    public virtual void RemoveStone(int row, int col)
    {
        SetCell(row, col, 0);
    }

    // Display the board
    // Used by Reversi.cs
    public virtual void DisplayBoard()
    {
        Display();
    }
}
