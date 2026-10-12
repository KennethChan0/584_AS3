public class Anti : Reversi
{
    public Anti(Board board, List<Player> players) : base(board, players)
    {
    }

    public override string Name => "Anti-Reversi (Misere)";

    public override bool CheckWin()
    {
        return NoMovesLeft() && DecideWinnerByCount(fewestWins: true);
    }

    public override string GetGameInfo()
    {
        return Name + "\n" + Rules +
            "Win: The player with FEWER disks wins. Equal counts = draw.\n";
    }
}
