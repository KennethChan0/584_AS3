using System.Reflection.Metadata;
using System.Runtime.CompilerServices;

public abstract class Player
{
    public int Id { get; }
    public Player(int id)
    {
        Id = id;
    }

}

public class Computer : Player
{
    private readonly IAiStrategy strategy;
    public Computer(int id, IAiStrategy strategy) : base(id)
    {
        this.strategy = strategy;
    }
    public Move GetMove(Game game)
    {
        return strategy.chooseMove(game);
    }
}

