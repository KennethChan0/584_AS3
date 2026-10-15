

public class CommandParserTest
{
    [Theory]
    [InlineData("o3:4", 'O', 3, 4)]
    [InlineData("h6:4", 'H', 6, 4)]
    [InlineData("p5:5", 'P', 5, 5)]

    public void ParseCommand_AutoTesting(string input, char stone, int row, int col)
    {
        CommandParser parser = new CommandParser();
        Player player = new HumanPlayer("Player 1", 1);
        Move move = parser.ParseCommand(input, player, 10);

        Assert.Equal(stone, move.Stone);
        Assert.Equal(row, move.Row);
        Assert.Equal(col, move.Col);
    }
}