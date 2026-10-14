public class NormalBoard : Board
{
    // Store all cells in a two-dimensional array
    private int[,] cells;

    // Constructor: create a square board
    public NormalBoard(int size) : base(size, size)
    {
        cells = new int[size, size];
    }

    // Check whether a position is inside the board
    private bool IsValidPosition(int row, int col)
    {
        return row >= 1 && row <= Rows &&
               col >= 1 && col <= Columns;
    }

    // Return the value stored in a cell
    public override int GetCell(int row, int col)
    {
        if (!IsValidPosition(row, col))
        {
            throw new ArgumentOutOfRangeException(
                "Position is outside the board.");
        }

        return cells[row - 1, col - 1];
    }

    // Change the value stored in a cell
    public override void SetCell(int row, int col, int value)
    {
        if (!IsValidPosition(row, col))
        {
            throw new ArgumentOutOfRangeException(
                "Position is outside the board.");
        }

        if (value < 0 || value > 4)
        {
            throw new ArgumentException("Invalid cell value.");
        }

        cells[row - 1, col - 1] = value;
    }

    // Check whether a cell is empty
    public override bool IsCellEmpty(int row, int col)
    {
        if (!IsValidPosition(row, col))
        {
            return false;
        }

        return GetCell(row, col) == 0;
    }

    // Remove all stones from the board
    public override void Clear()
    {
        Array.Clear(cells, 0, cells.Length);
    }

    // Display the complete board
    public override void Display()
    {
        Console.Write("  ");
        for (int col = 1; col <= Columns; col++)
        {
            Console.Write($"{col, 4}");
        }
        Console.WriteLine();

        Console.Write("   +");
        for (int col = 1; col <= Columns; col++)
        {
            Console.Write("---+");
        }
        Console.WriteLine();

        for (int row = 1; row <= Rows; row++)
        {
            Console.Write($"{row, 2} |");

            for (int col = 1; col <= Columns; col++)
            {
                int value = GetCell(row, col);

                char symbol = value switch
                {
                    0 => ' ',  // Empty
                    1 => 'X',  // Player 1 Ordinary
                    2 => '#',  // Player 1 Heavy
                    3 => 'O',  // Player 2 Ordinary
                    4 => '@',  // Player 2 Heavy
                    _ => '?'   // Unexpected value
                };

                Console.Write($" {symbol} |");
            }
            Console.WriteLine();

            Console.Write("   +");
            for (int col = 1; col <= Columns; col++)
            {
                Console.Write("---+");
            }
            Console.WriteLine();
        }
    }
}
