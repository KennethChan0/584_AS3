
public class HelpSystem
{
    public void DisplayHelp(IGame game)
    {
        GameVariantPrint(game, game.Board);
        Console.WriteLine(game.GetGameInfo());
        CommandSyntaxPrint();
    }

    private void GameVariantPrint(IGame game, Board board)// Header for the game variant
    {
        Console.WriteLine($"{IGame.Name}");
        Console.WriteLine($"Game Variant: {game}");
        Console.WriteLine($"Board Size: {board.Size}");

    }

    private void CommandSyntaxPrint()
    {
        Console.WriteLine("Command Syntax:");
        Console.WriteLine("  - To make a move, enter the row and column numbers separated by a space (e.g., '3 4').");
        Console.WriteLine("  - To undo the last move, enter 'undo'.");
        Console.WriteLine("  - To redo the last undone move, enter 'redo'.");
        Console.WriteLine("  - To save the game, enter 'save'.");
        Console.WriteLine("  - To load a saved game, enter 'load'.");
        Console.WriteLine("  - To exit the game, enter 'exit'.");
    }

}