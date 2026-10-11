public class GomokuBoard : Board
{
    private char[,] cells;

    public GomokuBoard()
        : base(10, 10)
    {
        cells = new char[Rows, Columns];

        for (int row = 0; row < Rows; row++)
        {
            for (int col = 0; col < Columns; col++)
            {
                cells[row, col] = ' ';
            }
        }
    }

    public override char GetCell(int row, int col)
    {
        return cells[row - 1, col - 1];
    }

    public override void SetCell(int row, int col, char symbol)
    {
        cells[row - 1, col - 1] = symbol;
    }

    public override bool IsCellEmpty(int row, int col)
    {
        if (row < 1 || row > Rows ||
            col < 1 || col > Columns)
        {
            return false;
        }

        return cells[row - 1, col - 1] == ' ';
    }

    public override void Clear()
    {
        for (int row = 0; row < Rows; row++)
        {
            for (int col = 0; col < Columns; col++)
            {
                cells[row, col] = ' ';
            }
        }
    }

    public override void Display()
    {
        Console.Write("  ");

        for (int col = 1; col <= Columns; col++)
        {
            Console.Write($"{col,4}");
        }

        Console.WriteLine();
        PrintHorizontalLine();

        for (int row = 0; row < Rows; row++)
        {
            Console.Write($"{row + 1,2} ");

            for (int col = 0; col < Columns; col++)
            {
                Console.Write($"| {cells[row, col]} ");
            }

            Console.WriteLine("|");
            PrintHorizontalLine();
        }
    }

    private void PrintHorizontalLine()
    {
        Console.Write("   ");

        for (int col = 0; col < Columns; col++)
        {
            Console.Write("+---");
        }

        Console.WriteLine("+");
    }
}