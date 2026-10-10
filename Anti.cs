public class Anti : Reversi
{
    public Anti(Board board, List<Player> players) : base(board, players)
    {
    }

    public override string Name
    {
        get { return "Reversi Anti"; }
    }

    public override bool CheckWin()
    {
        return NoMovesLeft() && DecideWinnerByCount(fewestWins: true);
    }

    public override string GetGameInfo()
    {
        return "Anti-Reversi (Misere)\n" + Rules +
            "  Win: the player with FEWER disks wins. Equal counts = draw.\n";
    }
}
