public class Standard : Reversi
{
    public Standard(Board board, List<Player> players) : base(board, players)
    {
    }

    public override string Name
    {
        get { return "Reversi Standard"; }
    }

    public override bool CheckWin()
    {
        return NoMovesLeft() && DecideWinnerByCount(fewestWins: false);
    }

    public override string GetGameInfo()
    {
        return "Standard Reversi\n" + Rules +
            "  Win: the player with MORE disks wins. Equal counts = draw.\n";
    }
}
