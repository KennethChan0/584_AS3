public class HelpSystem
{
    public void DisplayHelp(IGame game)
    {
        PrintGameHeader(game);
        Console.WriteLine("");
        Console.WriteLine(game.GetGameInfo());// variant rules, win condition, pieces
        CommandSyntax();
    }

    private void PrintGameHeader(IGame game)
    {
        int size = game.Board.Size;
        Console.WriteLine();
        Console.WriteLine("HELP Menu");
        Console.WriteLine($"Game: {game.Name}");
        Console.WriteLine($"Board: {size} x {size}");
    }

    private void CommandSyntax()
    {
        Console.WriteLine("\nCommands(Type one command on your turn):");
        Console.WriteLine("How to make a move: Gomoku O3:4 (H3:4 / E3:4 in Plus), Reversi P3:4");
        Console.WriteLine("undo: Revert the previous full turn (both players' moves)");
        Console.WriteLine("redo: Replay the turn you just undid");
        Console.WriteLine("save: Enter save to save game");
        Console.WriteLine("load: Enter load to load game");
        Console.WriteLine("help: Show this screen");
        Console.WriteLine("quit: Leave the game");
    }
}
