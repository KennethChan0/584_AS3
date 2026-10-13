
public class GameSession
{

    private IGame currentGame; // IGame has board & player
    private HelpSystem helpSystem;
    private SaveLoad saveLoad;
    private History history;
    private CommandParser parser;
    private bool stopRunning;


    // For save and load to use
    public string Variant{get; private set;}
    public string Mode{get; private set;}

    public GameSession(IGame game, string variant, string mode)// Constrcutor
    {
        
        currentGame = game;
        helpSystem = new HelpSystem();
        saveLoad = new SaveLoad();
        history = new History();
        parser = new CommandParser();
        stopRunning = false;
        Variant = variant;
        Mode = mode;

    }
    
    public IGame GetGame()
    {
        return currentGame;
    }

    public List<Move> GetMovesHistory()
    {
        return history.GetMovesHistory();
    }
    public int GetTurn()
    {
        return history.GetMovesHistory().Count;
    }


    public Player GetViewer()
    {
        Player viewer = currentGame.CurrentPlayer;

        if(!(viewer is HumanPlayer))
        {
            foreach (Player player in currentGame.Players)
            {
                if(player is HumanPlayer)
                {
                    viewer = player;
                }
            }
        }
        return viewer;
    }
    private Move? GetNextMove()
    {
        Player player = currentGame.CurrentPlayer;
        Move? move = null;

        if(player is ComputerPlayer computer)
        {
            Console.WriteLine(computer.Name + "is thinking...");
            move = computer.GetMove(currentGame);

        }
        else
        {
            move = HumanMove(player);
        }
        return move;
    }
    private Move? HumanMove(Player player)
    {
        string? input;
        string word;
        Move? move = null;
        CommandParser.CommandType command;

        Console.WriteLine(player.Name);
        input = Console.ReadLine();

        if(input == null)
        {
            stopRunning = true;
        }
        else if(input.Trim().Length == 0)
        {
            Console.WriteLine("Enter a move or type help to see information.");
        }
        else
        {
            word = input.Trim().Split(' ')[0];
            command = parser.CheckCommand(word);
            if(command != CommandParser.CommandType.Other)
            {
                AddressCommand(command);
            }
            else
            {
                move = ParseMove(input, player);
            }
        }
        return move;
    }

    private void AddressCommand(CommandParser.CommandType command)
    {
        switch(command)
        {
            case CommandParser.CommandType.Help:
                helpSystem.DisplayHelp(currentGame);
                break;
            case CommandParser.CommandType.Save:
                SaveGame();
                break;
            case CommandParser.CommandType.Load:
                LoadGame();
                break;
            case CommandParser.CommandType.Redo:
                Redo();
                break;
            case CommandParser.CommandType.Undo:
                Undo();
                break;
            case CommandParser.CommandType.Quit:
                stopRunning = true;
                break;
        }
    }

    private void Undo()
    {
        List<Move> undo;

        if(!history.CanUndo())
        {
            Console.WriteLine("Noting can undo");
        }
        else
        {
            undo = history.Undo();
            foreach(Move move in undo)
            {
                currentGame.UndoMove(move);
            }
            currentGame.CheckWin();
            Console.WriteLine($"{currentGame.CurrentPlayer.Name} turn.");

        }
    }

    private void Redo()
    {
        List<Move> redo;

        if(!history.CanRedo())
        {
            Console.WriteLine("Noting can redo");
        }
        else
        {
            redo = history.Redo();
            foreach (Move move in redo)
            {
                currentGame.RedoMove(move);
            }
            currentGame.CheckWin();
            Console.WriteLine($"{currentGame.CurrentPlayer.Name} turn.");
        }
    }

    private void SaveGame()
    {
        if(saveLoad.SaveGame(this))
        {
            Console.WriteLine("Game saved successfully");
        }
    }

    private void LoadGame()
    {
        GameSession? load = saveLoad.LoadGame();

        if(load == null)
        {
            Console.WriteLine("Load failed");
        }
        else
        {
            currentGame = load.currentGame;
            history = load.history;
            Variant = load.Variant;
            Mode = load.Mode;
            Console.WriteLine("Loaded successfully");
        }
    }

    private Move? ParseMove(string input, Player player)
    {
        Move? move = null;

        try
        {
            move = parser.ParseCommand(input, player, currentGame.Board.Size);
        }
        catch(Exception e)
        {
            Console.WriteLine(e.Message);
        }
        return move;

    }

    public bool ApplyMove(Move move)// Save and load need to use this to get data
    {
        bool isRightMove;
        
        isRightMove = currentGame.Play(move);

        if(isRightMove)
        {
            history.AddMove(move);
        }
        return isRightMove;
    }


    public void GetResult()
    {
        Player? winner = currentGame.Winner;

        if(winner == null)
        {
            Console.WriteLine("Draw");
        }
        else
        {
            Console.WriteLine($"{winner.Name} win");
        }
    }


    public void RunSession()
    {
        Move? move;
        bool isRightMove;

        stopRunning = false;

        while(!stopRunning && !currentGame.IsOver)
        {
            currentGame.DisplayBoard(GetViewer());//Get board

            move = GetNextMove();
            if(move != null)
            {
                isRightMove = ApplyMove(move);
                if(!isRightMove)
                {
                    if(currentGame.CurrentPlayer is ComputerPlayer)
                    {
                        Console.WriteLine("Computer player placed an illegal move.");
                        stopRunning = true;
                
                    }
                    else
                    {
                        Console.WriteLine("Invalid move, try again.");
                    }
                }
            }
        if(currentGame.IsOver)
        {
            currentGame.DisplayBoard(GetViewer());
            GetResult();
        }
            
        }


    }
    //For auto test enter commands
    public void RunScriptForAuto(string script)
    {
        string[] commands = script.Split(',');
        int index = 0;
        bool failed = false;
        Move? move;

        while(!failed && !currentGame.IsOver && index < commands.Length)
        {
            if(commands[index].Trim().ToUpper() == "PASS")
            {
                if(currentGame.GetPossibleMove(currentGame.CurrentPlayer).Count > 0)
                {
                    Console.WriteLine($"Command{index + 1} 'PASS' is in invalid, still have legal move");
                    failed = true;
                }
                else
                {
                    Console.WriteLine("PASS");
                }
            }
            else
            {
                move = ParseMove(commands[index], currentGame.CurrentPlayer);
                if(move == null || !ApplyMove(move))
                {
                    Console.WriteLine($"Command{index + 1} {commands[index].Trim()} is in invalid.");
                    failed = true;
                }
            }
            index++;

        }

        currentGame.DisplayBoard(GetViewer());
        Console.WriteLine("Active player:" + currentGame.CurrentPlayer.Name );
        Console.WriteLine("Player 1: " + CountStones(1) + "Player2: " + CountStones(2));
        if(currentGame.IsOver)
            {
                GetResult();
            }

    }
    

    public int CountStones(int playerId)
    {
        int count = 0;
        int value;

        for(int row =1; row <= currentGame.Board.Rows; row++)
        {
            for(int col = 1; col <= currentGame.Board.Columns; col++)
            {
                value = currentGame.Board.GetCell(row, col);
                if(value == playerId * 2 -1 || value == playerId *2)
                {
                    count++;
                }
            }
        }
        return count;
    }
}

