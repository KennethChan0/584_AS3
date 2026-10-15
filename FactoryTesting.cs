public class FactoryTests
{
    [Fact]
    public void CreateGame_AutoTesting()
    {
        GomokuFactory gomoku = new GomokuFactory();
        ReversiFactory reversi = new ReversiFactory();

        IGame classic = gomoku.CreateGame("classic", gomoku.CreateBoard(), gomoku.CreatePlayer("HvH", "classic"));
        IGame standard = reversi.CreateGame("standard", reversi.CreateBoard(), reversi.CreatePlayer("HvH", "standard"));

        Assert.IsType<Classic>(classic);
        Assert.IsType<Standard>(standard);
    }
}