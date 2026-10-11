

public class GameSession
{
    private IGame currentGame;
    private History history;
    private CommandParser parser;
    private HelpSystem helpSystem;
    private SaveLoad saveLoad;
    private bool running;

    // For SaveLoad to read
    public string Variant{ get; private set; }
    public string Mode { get; private set; }

    public GameSession(IGame game, string variant, string mode)
    {
        currentGame = game;
        Variant = variant;
        Mode = mode;
        history = new History();
        parser = new CommandParser();
        helpSystem = new HelpSystem();
        saveLoad = new SaveLoad();
        running = true;
    }

    //  read-only access (used by SaveLoad) 
    public IGame GetGame()
    {
        return currentGame;
    }

    public List<Move> GetMoveHistory()
    {
        return history.GetMovesHistory();
    }

    public int GetTurnCount()
    {
        return history.GetMovesHistory().Count;
    }

    // main session
    public void RunSession()
    {
        running = true;

        while (running && !currentGame.IsOver)
        {
            currentGame.DisplayBoard(GetViewer());// Fog variant should see different board

            Move? move = GetNextMove();
            if (move == null)
            {
                continue;   // a system command or a typo: no move this round
            }

            if (!TryLegalMove(move))
            {
                if (currentGame.CurrentPlayer is ComputerPlayer)
                {
                    Console.WriteLine("  ! The computer produced an illegal move. Ending the game.");
                    running = false;
                }
                else
                {
                    Console.WriteLine("  (move not accepted, try again)");
                }
            }
        }

        if (currentGame.IsOver)
        {
            currentGame.DisplayBoard(GetViewer());
            AnnounceResult();
        }
    }


    // Getting the next move 
    private Move? GetNextMove()
    {
        Player player = currentGame.CurrentPlayer;

        if (player is ComputerPlayer computer)
        {
            Console.WriteLine($"{computer.Name} is thinking...");
            return computer.GetMove(currentGame);
        }

        Console.Write($"{player.Name} > ");
        string? input = Console.ReadLine();
        if (input == null)
        {
            running = false;      // input stream closed
            return null;
        }

        string[] parts = input.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length == 0)
        {
            Console.WriteLine("  ! Enter a move or a command (type help).");
            return null;
        }

        string word = parts[0].ToLower();
        string? argument = parts.Length > 1 ? parts[1] : null;

        if (word == "save")
        {
            SaveCommand(argument);
            return null;
        }
        if (word == "load")
        {
            LoadCommand(argument);
            return null;
        }

        CommandParser.CommandType type = parser.CheckCommand(word);
        if (type != CommandParser.CommandType.Other)
        {
            HandleSystemCommand(type);
            return null;
        }

        try
        {
            return parser.ParseCommand(input, player, currentGame.Board.Size);
        }
        catch (Exception e)
        {
            Console.WriteLine($"{e.Message}!");
            return null;
        }
    }

    private Player GetViewer()
    {
        Player current = currentGame.CurrentPlayer;
        if (current is HumanPlayer)
        {
            return current;
        }
        foreach (Player p in currentGame.Players)
        {
            if (p is HumanPlayer)
            {
                return p;      // HvC: always show the human's view
            }
        }
        return current;
    }

    // Used by playing AND by SaveLoad when replaying a save file.
    public bool TryLegalMove(Move move)
    {
        if (!currentGame.Play(move))
        {
            return false;
        }
        history.AddMove(move);   // AI moves never reach History
        return true;
    }

    // system commands
    private void HandleSystemCommand(CommandParser.CommandType type)
    {
        switch (type)
        {
            case CommandParser.CommandType.Help:
                helpSystem.DisplayHelp(currentGame);
                break;
            case CommandParser.CommandType.Undo:
                Undo();
                break;
            case CommandParser.CommandType.Redo:
                Redo();
                break;
            case CommandParser.CommandType.Quit:
                running = false;
                break;
        }
    }

    private void Undo()
    {
        if (currentGame.IsOver)
        {
            Console.WriteLine("  ! The game is over, undo is not available.");
            return;
        }
        if (!history.CanUndo())
        {
            Console.WriteLine("  ! Nothing to undo.");
            return;
        }

        List<Move> undone = history.UndoMove();      // newest first
        foreach (Move move in undone)
        {
            currentGame.UndoMove(move);
        }
        Console.WriteLine("  Undid the previous turn.");
    }

    private void Redo()
    {
        if (!history.CanRedo())
        {
            Console.WriteLine("  ! Nothing to redo.");
            return;
        }

        List<Move> redone = history.RedoMove();      // oldest first
        foreach (Move move in redone)
        {
            currentGame.RedoMove(move);
        }
        currentGame.CheckWin();                      // redo may re-create the winning position
        Console.WriteLine("  Redid the next turn.");
    }

    // ===== save / load =====
    private void SaveCommand(string? filename)
    {
        if (filename == null)
        {
            Console.Write("Save file name: ");
            filename = Console.ReadLine();
        }
        if (string.IsNullOrWhiteSpace(filename))
        {
            Console.WriteLine("No file name given.");
            return;
        }
        SaveGame(filename);
    }

    private void LoadCommand(string? filename)
    {
        if (filename == null)
        {
            Console.Write("Load file name: ");
            filename = Console.ReadLine();
        }
        if (string.IsNullOrWhiteSpace(filename))
        {
            Console.WriteLine("  ! No file name given.");
            return;
        }
        LoadGame(filename);
    }

    public void SaveGame(string filename)
    {
        if (saveLoad.SaveGame(this, filename))
        {
            Console.WriteLine($"Game saved to {filename}.");
        }
    }

    public void LoadGame(string filename)
    {
        GameSession? loaded = saveLoad.LoadGame(filename);
        if (loaded == null)
        {
            Console.WriteLine("Load failed. Your current game is unchanged.");
            return;
        }
        AdoptState(loaded);
        Console.WriteLine($"Game loaded from {filename}.");
    }

    // We are inside the running loop, so we cannot become another object:
    // take over the loaded session's contents instead.
    private void AdoptState(GameSession other)
    {
        currentGame = other.currentGame;
        history = other.history;
        Variant = other.Variant;
        Mode = other.Mode;
    }

    private void AnnounceResult()
    {
        Player? winner = currentGame.Winner;
        if (winner == null)
        {
            Console.WriteLine("The game is a draw.");
        }
        else
        {
            Console.WriteLine($"{winner.Name} wins!");
        }
    }
}