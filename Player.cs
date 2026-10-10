using System.Reflection.Metadata;
using System.Runtime.CompilerServices;

public abstract class Player
{
    public int Id { get; }
    public string Name {get;}
    public Player(string name, int id)
    {
        Name = name;
        Id = id;
    }

}

public class ComputerPlayer : Player
{
    private readonly IAiStrategy strategy;
    public ComputerPlayer(string name, int id, IAiStrategy strategy) : base(name,id)
    {
        this.strategy = strategy;
    }
    public Move GetMove(IGame game)
    {
        return strategy.chooseMove(game);
    }
}

public class HumanPlayer:Player
{
    public HumanPlayer(string name,int id) : base(name,id)
    {
        
    }
}

