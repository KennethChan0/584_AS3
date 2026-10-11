public class SaveLoad
{
    public bool SaveGame(GameSession session, string filename)
    {
        IGame game = session.GetGame();
        List<string> lines = new List<string>();

        lines.Add($"VARIANT={session.Variant}");
        lines.Add($"MODE={session.Mode}");
        lines.Add($"TURN={session.GetTurnCount()}");
        lines.Add($"ACTIVE={game.CurrentPlayer.Id}");
        lines.Add("MOVES");
        foreach (Move move in session.GetMoveHistory())
        {
            lines.Add(MoveToLine(move));
        }

        try
        {
            File.WriteAllLines(filename, lines);
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine($"  ! Save failed: {e.Message}");
            return false;
        }
    }

    public GameSession? LoadGame(string filename)
    {
        if (!File.Exists(filename))
        {
            Console.WriteLine($"  ! File not found: {filename}");
            return null;
        }

        string[] lines;
        try
        {
            lines = File.ReadAllLines(filename);
        }
        catch (Exception e)
        {
            Console.WriteLine($"  ! Could not read the file: {e.Message}");
            return null;
        }

        // ----- read the header and collect the move lines -----
        string? variant = null;
        string? mode = null;
        string? turnText = null;
        string? activeText = null;
        List<string> moveLines = new List<string>();
        bool inMoves = false;

        foreach (string rawLine in lines)
        {
            string line = rawLine.Trim();
            if (line.Length == 0)
            {
                continue;
            }

            if (inMoves)
            {
                moveLines.Add(line);
            }
            else if (line == "MOVES")
            {
                inMoves = true;
            }
            else if (line.StartsWith("VARIANT="))
            {
                variant = line.Substring("VARIANT=".Length);
            }
            else if (line.StartsWith("MODE="))
            {
                mode = line.Substring("MODE=".Length);
            }
            else if (line.StartsWith("TURN="))
            {
                turnText = line.Substring("TURN=".Length);
            }
            else if (line.StartsWith("ACTIVE="))
            {
                activeText = line.Substring("ACTIVE=".Length);
            }
        }

        if (variant == null || mode == null || !inMoves)
        {
            Console.WriteLine("  ! The save file is missing required lines.");
            return null;
        }
        if (!int.TryParse(turnText, out int expectedTurn) ||
            !int.TryParse(activeText, out int expectedActive))
        {
            Console.WriteLine("  ! TURN or ACTIVE in the save file is not a number.");
            return null;
        }

        // ----- rebuild a brand-new game through the factory -----
        IGameFactory? factory = GameMenu.FindFactory(variant);
        if (factory == null)
        {
            Console.WriteLine($"  ! Unknown variant '{variant}' in the save file.");
            return null;
        }

        GameSession session;
        try
        {
            Board board = factory.CreateBoard();
            List<Player> players = factory.CreatePlayer(mode, variant);
            IGame game = factory.CreateGame(variant, board, players);
            session = new GameSession(game, variant, mode);
        }
        catch (Exception e)
        {
            Console.WriteLine($"  ! Could not rebuild the game: {e.Message}");
            return null;
        }

        // ----- replay every move through the normal path -----
        int number = 0;
        foreach (string moveLine in moveLines)
        {
            number++;
            Move? move = LineToMove(moveLine, session.GetGame().Players);
            if (move == null)
            {
                Console.WriteLine($"  ! Bad move line {number}: {moveLine}");
                return null;
            }
            if (!session.TryLegalMove(move))
            {
                Console.WriteLine($"  ! Move {number} ({moveLine}) is not legal in this game.");
                return null;
            }
        }

        // ----- check the replay against what the file says -----
        if (session.GetTurnCount() != expectedTurn ||
            session.GetGame().CurrentPlayer.Id != expectedActive)
        {
            Console.WriteLine("  ! The save file does not match the replayed game (corrupt file?).");
            return null;
        }

        return session;
    }

    private string MoveToLine(Move move)
    {
        return $"{move.Stone},{move.Row},{move.Col},{move.player.Id}";
    }

    private Move? LineToMove(string line, List<Player> players)
    {
        string[] parts = line.Split(',');
        if (parts.Length != 4 || parts[0].Length != 1)
        {
            return null;
        }

        char stone = parts[0][0];
        if (!int.TryParse(parts[1], out int row)) return null;
        if (!int.TryParse(parts[2], out int col)) return null;
        if (!int.TryParse(parts[3], out int playerId)) return null;

        foreach (Player p in players)
        {
            if (p.Id == playerId)
            {
                return new Move(row, col, stone, p);   // the SAME player object the game uses
            }
        }
        return null;
    }
}