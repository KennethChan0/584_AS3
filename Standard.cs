public class Standard : Reversi
{
    public Standard(Board board, List<Player> players) : base(board, players)
    {
    }

    public override string Name => "Standard Reversi";

    public override bool CheckWin()
    {
        return NoMovesLeft() && DecideWinnerByCount(fewestWins: false);
    }

    public override string GetGameInfo()
    {
        return Rules +
            "Win: The player with MORE disks wins. Equal counts = draw.\n";
    }
}
