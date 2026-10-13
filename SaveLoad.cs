public class SaveFileException : Exception
{
    public SaveFileException(string message): base(message)
    {
        
    }
}


public class SaveLoad
{
    private const string FILENAME = "savegame.txt";
    private const char DELIM =',';
    private const char HEADER_DELIM = '=';
    private const string MOVES_MARK = "MOVES";

    public bool SaveGame(GameSession session)
    {
        bool saved = false;
        IGame game = session.GetGame();
        FileStream inFile;
        StreamWriter writer;

        try
        {
            inFile = new FileStream(FILENAME, FileMode.Create, FileAccess.Write);
            writer = new StreamWriter(inFile);

            writer.WriteLine("VARIANT" + HEADER_DELIM + session.Variant);
            writer.WriteLine("MODE" + HEADER_DELIM + session.Mode);
            writer.WriteLine("TURN" + HEADER_DELIM + session.GetTurn());
            writer.WriteLine("ACTIVE" + HEADER_DELIM + game.CurrentPlayer.Id);
            writer.WriteLine(MOVES_MARK);
            foreach (Move move in session.GetMovesHistory())
            {
                writer.WriteLine(move.Stone.ToString() + DELIM + move.Row + DELIM + move.Col + DELIM + move.player.Id);
            }
            writer.Close();
            inFile.Close();
            saved = true;
        }
        catch(Exception e)
        {
            Console.WriteLine("Save failed:" + e.Message);
        }
        return saved;
    }


    public GameSession? LoadGame()
    {
        GameSession? session = null;
        List<string> lines;

        if (!File.Exists(FILENAME))
        {
            Console.WriteLine("  ! File not found: " + FILENAME);
        }
        else
        {
            try
            {
                lines = ReadFileLines();
                session = Rebuild(lines);
            }
            catch (SaveFileException e)
            {
                Console.WriteLine("  ! " + e.Message);
                session = null;
            }
            catch (Exception e)
            {
                Console.WriteLine("  ! Could not load the file: " + e.Message);
                session = null;
            }
        }
        return session;
    }

    private List<string> ReadFileLines()
    {
        List<string> lines = new List<string>();
        string? recordIn;
        FileStream inFile = new FileStream(FILENAME, FileMode.Open, FileAccess.Read);
        StreamReader reader = new StreamReader(inFile);

        recordIn = reader.ReadLine();
        while (recordIn != null)
        {
            lines.Add(recordIn.Trim());
            recordIn = reader.ReadLine();
        }

        reader.Close();
        inFile.Close();
        return lines;
    }

    private GameSession Rebuild(List<string> lines)
    {
        string variant = "";
        string mode = "";
        int expectedTurn = -1;
        int expectedActive = -1;
        bool inMoves = false;
        List<string> moveLines = new List<string>();
        string[] fields;

        foreach (string line in lines)
        {
            if (line.Length > 0)
            {
                if (inMoves)
                {
                    moveLines.Add(line);
                }
                else if (line == MOVES_MARK)
                {
                    inMoves = true;
                }
                else
                {
                    fields = line.Split(HEADER_DELIM);
                    if (fields.Length != 2)
                    {
                        throw new SaveFileException("Bad header line: " + line);
                    }

                    if (fields[0] == "VARIANT")
                    {
                        variant = fields[1];
                    }
                    else if (fields[0] == "MODE")
                    {
                        mode = fields[1];
                    }
                    else if (fields[0] == "TURN")
                    {
                        expectedTurn = ToNumber(fields[1], line);
                    }
                    else if (fields[0] == "ACTIVE")
                    {
                        expectedActive = ToNumber(fields[1], line);
                    }
                    else
                    {
                        throw new SaveFileException("Unknown header line: " + line);
                    }
                }
            }
        }

        if (variant == "" || mode == "" || expectedTurn < 0 || expectedActive < 0 || !inMoves)
        {
            throw new SaveFileException("The save file is missing required lines.");
        }
        IGameFactory? factory = GameMenu.FindFactory(variant);
        if (factory == null)
        {
            throw new SaveFileException("Unknown variant '" + variant + "' in the save file.");
        }

        Board board = factory.CreateBoard();
        List<Player> players = factory.CreatePlayer(mode, variant);
        IGame game = factory.CreateGame(variant, board, players);
        GameSession session = new GameSession(game, variant, mode);

        int number = 0;
        Move move;
        foreach (string moveLine in moveLines)
        {
            number++;
            move = LineToMove(moveLine, game.Players);
            if (!session.ApplyMove(move))
            {
                throw new SaveFileException("Move " + number + " (" + moveLine + ") is not legal in this game.");
            }
        }

        if (session.GetTurn() != expectedTurn || game.CurrentPlayer.Id != expectedActive)
        {
            throw new SaveFileException("The save file does not match the replayed game (corrupt file?).");
        }
        return session;
    }

    private Move LineToMove(string line, List<Player> players)
    {
        string[] fields = line.Split(DELIM);
        Player? owner = null;
        char stone;
        int row;
        int col;
        int playerId;

        if (fields.Length != 4 || fields[0].Length != 1)
        {
            throw new SaveFileException("Bad move line: " + line);
        }

        stone = fields[0][0];
        row = ToNumber(fields[1], line);
        col = ToNumber(fields[2], line);
        playerId = ToNumber(fields[3], line);

        foreach (Player p in players)
        {
            if (p.Id == playerId)
            {
                owner = p;
            }
        }
        if (owner == null)
        {
            throw new SaveFileException("Unknown player in line: " + line);
        }
        return new Move(row, col, stone, owner);
    }

    private int ToNumber(string text, string context)
    {
        int number;

        if (!int.TryParse(text, out number))
        {
            throw new SaveFileException("Not a number in: " + context);
        }
        return number;
    }
}
    


