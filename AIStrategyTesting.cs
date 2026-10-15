public class AIStrategyTest
{
    [Fact]
    public void ChosseMove_AutoTesting()
    {
        IAiStrategy strategy = new DumbGomokuStrategy();
        Board board = new NormalBoard(10);
        Classic game = new Classic(new NormalBoard(10), new List<Player> 
        {
            new ComputerPlayer("Computer 1", 1, strategy), 
            new ComputerPlayer("Computer 2", 2, strategy)
        });
        
        for(int i =0; i < 20; i++)
        {
            Move move = strategy.ChooseMove(game);
            Assert.True(game.ValidateMove(move));
            Assert.True(game.Play(move));
            game.DisplayBoard(game.CurrentPlayer);
        }

    }
}