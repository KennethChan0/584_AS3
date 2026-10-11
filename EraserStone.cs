public class EraserStone : Stone
{
    public EraserStone(int amount)
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

        if (board.IsCellEmpty(row, col))
        {
            return false;
        }

        char target = board.GetCell(row, col);

        char opponentOrdinarySymbol =
            player.Symbol == 'X' ? 'O' : 'X';

        if (target != opponentOrdinarySymbol)
        {
            return false;
        }

        board.SetCell(row, col, ' ');

        UseStone();

        return true;
    }
}