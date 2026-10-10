using System.Security;



public class GameSession
{
    public currentGame IGame;
    public board Board;
    public players List<Player>;
    public parser CommandParser;
    public history History;
    public helpSystem HelpSystem;

    public GameSession()
    {
        IGame = new currentGame();
        Board = new board();
        List<Player> = new players();
        CommandParser = new parser();
        History = new history();
        HelpSystem = new helpSystem();
    }

    public void runSession()
    {
        
    }

    public void playTurn(Move move)
    {
        
    }

    public string getCommand(Move move)
    {
        return $"{move.Stone}{move.Row}:{move.Col}";
    }

    public void switchPlayer()
    {
        // Logic to switch the current player
    }

    public bool Undo()
    {
        // Logic to undo the last move
        return true; // Return true if undo was successful, false otherwise
    }

    public bool Redo()
    {
        // Logic to redo the last undone move
        return true; // Return true if redo was successful, false otherwise
    }

    public void DisplayHelp()
    {
        // Logic to display help information
    }

    public void saveGame(string fileName)
    {
        // Logic to save the current game state to a file
    }

    public void loadGame(string fileName)
    {
        // Logic to load a game state from a file
    }
}