
public class EraserStone : Stone
{
    // Constructor: identify the owner of the eraser
    public EraserStone(int playerId) : base(playerId)
    {
    }

    // Check whether the eraser can remove the target stone
    public override bool CanPlace(Board board, int row, int col)
    {
        // Check whether the board exists
        if (board == null)
        {
            return false;
        }

        // Check whether the coordinates are valid
        if (row < 1 || row > board.Rows ||
            col < 1 || col > board.Columns)
        {
            return false;
        }

        // Get the target cell value
        int target = board.GetCell(row, col);

        // Only opponent ordinary stones can be erased
        if (PlayerId == 1)
        {
            return target == 3;
        }
        else
        {
            return target == 1;
        }
    }
}
