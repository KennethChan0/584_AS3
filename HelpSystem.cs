
public class HelpSystem
{
    public void DisplayHelp(IGame game)
    {
        GameVariantPrint(game);
        Console.WriteLine(game.GetGameInfo());
        CommandSyntaxPrint();
    }

    private void GameVariantPrint(IGame game)
    {
        Console.WriteLine($"Game Variant: {game.Name}");
    }

}