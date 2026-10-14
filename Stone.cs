public abstract class Stone
{
    // The player who owns this stone //property record the stone who belomgs to which player
    public int PlayerId { get; }

    // Constructor
    protected Stone(int playerId)
    {
        if (playerId != 1 && playerId != 2)
        {
            throw new ArgumentException("Invalid player ID.");
        }

        PlayerId = playerId;
    }

    // Check whether the stone can be used at a position //polymorphism method to check if the stone can be placed at a position
    public abstract bool CanPlace(Board board, int row, int col);
}
