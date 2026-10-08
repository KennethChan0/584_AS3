using System.Security;

public class CommandParser
{
    public Move ParseCommand(string playerInput, Player player, int boardSize)
    {
        checkValid(playerInput, boardSize);

        char stone = playerInput[0];
        string[] parts = playerInput.Substring(1).Split(":");

        int row = int.Parse(parts[0]);
        int col = int.Parse(parts[1]);

        Move move = new Move(row, col, stone, player);
        return move;
    }

    public void checkValid(string playerInput, int boardSize)
    {

            if (playerInput.Length == 0)
            {
                throw new Exception("Please do not enter nothing");
            }

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
    
}
    
