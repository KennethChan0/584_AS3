public abstract class Board
{
    public int Rows { get; protected set; }
    public int Columns { get; protected set; }

    protected Board(int rows, int columns)
    {
        Rows = rows;
        Columns = columns;
    }

    public abstract char GetCell(int row, int col); -> reutrn int

    public abstract void SetCell(int row, int col, char symbol); -> symbol change to 0: empty, 1: X, 2: player1 heavy, 3: O, 4: player2 heavy (reversi: 1, 3)

    public abstract bool IsCellEmpty(int row, int col);

    public abstract void Clear();

    public abstract void Display();
}
