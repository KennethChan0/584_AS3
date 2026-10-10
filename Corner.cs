public class Corner : Reversi
{
    public Corner(Board board, List<Player> players) : base(board, players)
    {
    }

    public override string Name
    {
        get { return "Reversi Corner"; }
    }

    public override bool CheckWin()
    {
        int size = Board.Size;
        foreach (Player player in Players)
        {
            int corners = 0;
            if (Board.GetCell(1, 1) == player.Id) corners++;
            if (Board.GetCell(1, size) == player.Id) corners++;
            if (Board.GetCell(size, 1) == player.Id) corners++;
            if (Board.GetCell(size, size) == player.Id) corners++;

            if (corners >= 3)
            {
                IsOver = true;
                Winner = player;
                return true;
            }
        }
        return NoMovesLeft() && DecideWinnerByCount(fewestWins: false);
    }

    public override string GetGameInfo()
    {
        return "Corner Reversi (Corner Dominance)\n" + Rules +
            "  Win (1): capture 3 of the 4 corners at any time = instant win,\n" +
            "           no matter how many disks each player has.\n" +
            "  Win (2): if the game ends and nobody has 3 corners,\n" +
            "           the player with MORE disks wins. Equal counts = draw.\n";
    }
}
