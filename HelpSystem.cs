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
<<<<<<< HEAD
        Console.WriteLine("Help Menu");
        Console.WriteLine($"{game.Name}");
        Console.WriteLine($"Board: "); //Wait for Board class
=======
        Console.WriteLine("HELP Menu");
        Console.WriteLine($"Game: {game.Name}");
        Console.WriteLine($"Board: {size} x {size}");
>>>>>>> c567fd3749132c1eb525c620ea490ac18f1e99cb
    }

    private void CommandSyntax()
    {
        Console.WriteLine("Commands(Type one command on your turn):");
<<<<<<< HEAD
        Console.WriteLine("How to make a move: e.g. O3:4 (row 3, column 4)");
=======
        Console.WriteLine("How to make a move: Gomoku O3:4 (H3:4 / E3:4 in Plus), Reversi P3:4");
>>>>>>> c567fd3749132c1eb525c620ea490ac18f1e99cb
        Console.WriteLine("undo: revert the previous full turn (both players' moves)");
        Console.WriteLine("redo: replay the turn you just undid");
        Console.WriteLine("save: enter save to save game");
        Console.WriteLine("load: enter load to load game");
        Console.WriteLine("help: show this screen");
        Console.WriteLine("quit: leave the game");
    }
}
