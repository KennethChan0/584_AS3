using System.Security;

public class CommandParser
{
    // Parse and Call CheckValid to Check Correct syntax of Move Command 
    public Move ParseCommand(string playerInput, Player player, int boardSize)
    {
        if (playerInput == null || playerInput.Trim().Length==0)
        {
            throw new Exception("Please do not enter nothing");
        }

        playerInput = playerInput.Trim().ToUpper();
        CheckValid(playerInput, boardSize);

        char stone = playerInput[0];
        string[] parts = playerInput.Substring(1).Split(":");

        int row = int.Parse(parts[0]);
        int col = int.Parse(parts[1]);

        Move move = new Move(row, col, stone, player);
        return move;
    }

    // Check Correct syntax of move command
    private void CheckValid(string playerInput, int boardSize)
    {
        char stone = playerInput[0];

        if (stone != 'O' && stone != 'H' && stone != 'E')
        {
            throw new Exception("Stone must be 'O' or 'H' or 'E'!");
        }

        string[] parts = playerInput.Substring(1).Split(':');

        if (parts.Length != 2)
        {
            throw new Exception("Please use format O[row]:[column]");
        }

        bool rowIsNumber = int.TryParse(parts[0], out int row);
        bool colIsNumber = int.TryParse(parts[1], out int col);

        if (rowIsNumber == false)
        {
            throw new Exception("Row must be a number.");
        }

        if (colIsNumber == false)
        {
            throw new Exception("Column must be a number.");
        }

        if (col < 1 || col > boardSize || row < 1 || row > boardSize)
        {
            throw new Exception($"The valid number of row and column must be between 1 and {boardSize} ");
        }

    }

    // seperate dufferent command type by using enum
    public enum CommandType
    {
        Help,
        Save,
        Redo,
        Undo,
        Quit,
        Other
    }

    // identify different command type, please call in gameSession 
    public CommandType CheckCommand(string playerInput)
    {
        string command = playerInput.Trim().ToUpper();

        switch (command)
        {
            case "HELP":
                return CommandType.Help;

            case "SAVE":
                return CommandType.Save;

            case "REDO":
                return CommandType.Redo;

            case "UNDO":
                return CommandType.Undo;

            case "QUIT":
                return CommandType.Quit;

            default:
                return CommandType.Other;

        }
    }

// automatically testing parse and return a move list 
    public List<Move> ParseTestingCommand(string playerInput, Player player1,Player player2, int boardSize)
    {   
        List<Move> allMoves = new List<Move>();
        string[] commands = playerInput.Split(",");
        Player currentPlayer = player1;

        foreach (string command in commands)
        {   
            allMoves.Add(ParseCommand(command, currentPlayer, boardSize));
            if (currentPlayer== player1)
            {
                currentPlayer = player2;
            }
            else
            {
                currentPlayer= player1;
            }
            
        }
        return allMoves;
    }

}