public class PlayerBoardView
{
    // Store the filtered view of each cell
    private readonly CellView[,] cells;

    public int Rows { get; }
    public int Columns { get; }

    // Constructor: create a player-specific board view
    public PlayerBoardView(int rows, int columns)
    {
        if (rows <= 0 || columns <= 0)
        {
            throw new ArgumentException("Invalid board dimensions.");
        }

        Rows = rows;
        Columns = columns;

        cells = new CellView[rows, columns];

        // Initialise every cell as hidden
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                cells[row, col] = new CellView();
            }
        }
    }

    // Return one cell using 1-based coordinates
    public CellView GetCell(int row, int col)
    {
        if (row < 1 || row > Rows ||
            col < 1 || col > Columns)
        {
            throw new ArgumentOutOfRangeException(
                nameof(row), "Position is outside the board.");
        }
        return cells[row - 1, col - 1];
    }

    // Update the filtered board using the true board state
    public void Update(Board board, Player viewer)
    {
        if (board == null || viewer == null)
        {
            throw new ArgumentNullException(
                board == null ? nameof(board) : nameof(viewer));
        }

        if (board.Rows != Rows || board.Columns != Columns)
        {
            throw new ArgumentException("Board dimensions do not match.");
        }

        // Hide all cells before recalculating visibility
        for (int row = 1; row <= Rows; row++)
        {
            for (int col = 1; col <= Columns; col++)
            {
                GetCell(row, col).SetHidden();
            }
        }

        // Fog uses ordinary stones only:
        // Player 1 = 1 (X), Player 2 = 3 (O)
        int ownStone = viewer.Id == 1 ? 1 :
                       viewer.Id == 2 ? 3 :
                       throw new ArgumentException("Unknown player.");

        // Find all the viewer's stones on the true board
        for (int row = 1; row <= Rows; row++)
        {
            for (int col = 1; col <= Columns; col++)
            {
                if (board.GetCell(row, col) != ownStone)
                {
                    continue;
                }

                // Reveal own stone and its eight neighbours
                for (int dr = -1; dr <= 1; dr++)
                {
                    for (int dc = -1; dc <= 1; dc++)
                    {
                        int nearbyRow = row + dr;
                        int nearbyCol = col + dc;

                        if (nearbyRow < 1 || nearbyRow > Rows ||
                            nearbyCol < 1 || nearbyCol > Columns)
                        {
                            continue;
                        }

                        int value = board.GetCell(nearbyRow, nearbyCol);

                        GetCell(nearbyRow, nearbyCol).SetVisible(value);
                    }
                }
            }
        }
    }

    // Display the filtered board
    public void Display()
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
                Console.Write($" {GetCell(row, col).GetSymbol()} |");
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
