public class HeavyStone : Stone //Inherits from Stone class, represents a heavy stone in the game
{
    // Constructor: identify the owner of the heavy stone //base constructor to set the player ID
    public HeavyStone(int playerId) : base(playerId)
    {
    }

    // Check whether a heavy stone can be placed //Polymorphism method to check if the heavy stone can be placed at a position
    public override bool CanPlace(Board board, int row, int col)
    {
        // Check whether the board exists
        if (board == null)
        {
            return false;
        }

        // Check whether the position is inside the board
        if (row < 1 || row > board.Rows ||
            col < 1 || col > board.Columns)
        {
            return false;
        }

        // Heavy stones can only be placed on empty cells
        return board.IsCellEmpty(row, col);
    }

    // Convert the heavy stone into a board cell value
    public int GetBoardValue()
    {
    if (PlayerId == 1)
    {
        return 2;
    }
    else
    {
        return 4;
    }
    }
}