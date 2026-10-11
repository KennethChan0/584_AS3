
public class HelpSystem
{
    public void DisplayHelp(IGame game)
    {
        PrintGameHeader(game);
        Console.WriteLine(game.GetGameInfo());// variant rules, win condition, pass rule, pieces
        CommandSyntax();
    }

    private void PrintGameHeader(IGame game)
    {
        int size = game.Board.Size;
        Console.WriteLine();
        Console.WriteLine("HELP Menu");
        Console.WriteLine($"Game: {game.Name}");
        Console.WriteLine($"Board: "); //Wait for Board class
        Console.WriteLine();
    }

    private void CommandSyntax()
    {
        Console.WriteLine("Commands(Type one command on your turn):");
        Console.WriteLine("How to make a move: e.g. O3:4 (row 3, column 4)");
        Console.WriteLine("pass: only in Reversi, when you have no legal move");
        Console.WriteLine("undo: revert the previous full turn (both players' moves)");
        Console.WriteLine("redo: replay the turn you just undid");
        Console.WriteLine("Save: e.g. save game1.txt");
        Console.WriteLine("Load: e.g. load game1.txt");
        Console.WriteLine("help: show this screen");
        Console.WriteLine("quit: leave the game");
        Console.WriteLine();
    }
}