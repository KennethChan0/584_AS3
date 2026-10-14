public class MoveCommand : ICommand
{
    public Move Move { get; }
    private IGame game;
    public MoveCommand(IGame game, Move move)
    {
        this.game = game;
        Move = move;
    }
    public bool Execute()
    {
        return game.Play(Move);
    }
    public void Undo()
    {
        game.UndoMove(Move);
    }
}