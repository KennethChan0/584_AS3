public abstract class Stone
{
    public int Amount { get; protected set; }

    protected Stone(int amount)
    {
        Amount = amount;
    }

    public void UseStone()
    {
        if (Amount > 0)
        {
            Amount--;
        }
    }

    public void AddStone()
    {
        Amount++;
    }

    public abstract bool Apply(
        Board board,
        int row,
        int col,
        Player player);
}