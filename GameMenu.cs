public class GameMenu
{
    public void Run()
    {
        while (true)
        {
            DisplayMenu();
            string choice = Ask("Choose: ");

            if (choice == "0")
            {
                Console.WriteLine("Goodbye!");
                return;
            }
            if (choice == "7")
            {
                LoadFromFile();
                continue;
            }

            string? variant = VariantFromChoice(choice);
            if (variant == null)
            {
                Console.WriteLine("  ! Please enter a number from the menu.");
                continue;
            }

            string mode = SelectMode();

            GameSession session;
            try
            {
                session = BuildSession(variant, mode);
            }
            catch (Exception e)
            {
                Console.WriteLine($"  ! Could not create the game: {e.Message}");
                continue;
            }

            session.RunSession();   // returns when the game ends or the player quits
        }
    }

    private void DisplayMenu()
    {
        Console.WriteLine();
        Console.WriteLine("Gomoku & Reversi");
        Console.WriteLine("  Gomoku:   1) Classic   2) Plus   3) Fog");
        Console.WriteLine("  Reversi:  4) Standard  5) Anti   6) Corner");
        Console.WriteLine("  7) Load a saved game");
        Console.WriteLine("  0) Exit");
        Console.WriteLine();
    }

    private string? VariantFromChoice(string choice)
    {
        switch (choice)
        {
            case "1": return "classic";
            case "2": return "plus";
            case "3": return "fog";
            case "4": return "standard";
            case "5": return "anti";
            case "6": return "corner";
            default: return null;
        }
    }

    private string SelectMode()
    {
        while (true)
        {
            Console.WriteLine("Mode:  1) Human vs Human   2) Human vs Computer (Dumb)   3) Human vs Computer (Smart)");
            string choice = Ask("Choose: ");

            switch (choice)
            {
                case "1": return "HvH";
                case "2": return "HvC Dumb";
                case "3": return "HvC Smart";
                default:
                    Console.WriteLine("  ! Please enter 1, 2 or 3.");
                    break;
            }
        }
    }

    private GameSession BuildSession(string variant, string mode)
    {
        IGameFactory? factory = FindFactory(variant);
        if (factory == null)
        {
            throw new ArgumentException($"Unknown variant '{variant}'.");
        }

        Board board = factory.CreateBoard();
        List<Player> players = factory.CreatePlayer(mode, variant);
        IGame game = factory.CreateGame(variant, board, players);
        return new GameSession(game, variant, mode);
    }

    // Also used by SaveLoad, so it is public static.
    public static IGameFactory? FindFactory(string variant)
    {
        switch (variant)
        {
            case "classic":
            case "plus":
            case "fog":
                return new GomokuFactory();
            case "standard":
            case "anti":
            case "corner":
                return new ReversiFactory();
            default:
                return null;
        }
    }

    private void LoadFromFile()
    {
        string filename = Ask("Save file name: ");
        GameSession? session = new SaveLoad().LoadGame(filename);
        if (session == null)
        {
            Console.WriteLine("  Load failed.");
            return;
        }
        Console.WriteLine("  Game loaded.");
        session.RunSession();
    }

    private string Ask(string prompt)
    {
        Console.Write(prompt);
        string? line = Console.ReadLine();
        if (line == null)
        {
            Environment.Exit(0);   // input stream closed: avoid looping forever
        }
        return line.Trim();
    }
}