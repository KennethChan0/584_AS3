
Board board = new NormalBoard(10);
Console.WriteLine("\n=== Gomoku 10x10 Board ===");
// Test 1: Board size
Console.WriteLine("Test 1: Board Size");
Console.WriteLine($"Rows: {board.Rows}, Columns: {board.Columns}");

// Test 2: Place ordinary stones
board.SetCell(5, 5, 1); // Player 1: X
board.SetCell(6, 6, 3); // Player 2: O

// Test 3: Place heavy stones
board.SetCell(7, 7, 2); // Player 1: #
board.SetCell(8, 8, 4); // Player 2: @

// Test 4: Check cell values
Console.WriteLine($"Cell (5,5): {board.GetCell(5, 5)}");
Console.WriteLine($"Cell (6,6): {board.GetCell(6, 6)}");

// Test 5: Check empty cells
Console.WriteLine($"Is (5,5) empty? {board.IsCellEmpty(5, 5)}");
Console.WriteLine($"Is (1,1) empty? {board.IsCellEmpty(1, 1)}");

// Test 6: Display board
Console.WriteLine("\nBoard:");
board.Display();

// Test 7: Reversi board
Console.WriteLine("\n=== Reversi 8x8 Board ===");

Board reversiBoard = new NormalBoard(8);

// Set the four initial Reversi disks
reversiBoard.SetCell(4, 4, 3); // O
reversiBoard.SetCell(4, 5, 1); // X
reversiBoard.SetCell(5, 4, 1); // X
reversiBoard.SetCell(5, 5, 3); // O

reversiBoard.Display();

