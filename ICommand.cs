public interface ICommand
{
    Move Move { get; }
    bool Execute();
    void Undo();
}

