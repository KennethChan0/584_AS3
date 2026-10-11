public class HeavyStone : Stone
{
    public HeavyStone(int amount)
        : base(amount)
    {
    }

    public override bool Apply(
        Board board,
        int row,
        int col,
        Player player)
    {
        if (board == null || player == null)
        {
            return false;
        }

        if (Amount <= 0)
        {
            return false;
        }

        if (!board.IsCellEmpty(row, col))
        {
            return false;
        }

        char heavySymbol = char.ToLower(player.Symbol);

        board.SetCell(row, col, heavySymbol);

        UseStone();

        return true;
    }
}