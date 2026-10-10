public interface IGame
{
    Board Board { get; }
    List<Player> Players { get; }
    string Name { get; }
    Player CurrentPlayer { get; }
    bool IsOver { get; }
    Player? Winner { get; }

    bool Play(Move move);
    bool ValidateMove(Move move);
    void ExecuteMove(Move move);
    void UndoMove(Move move);
    void RedoMove(Move move);
    bool CheckWin();
    List<Move> GetPossibleMove(Player player);
    void DisplayBoard(Player viewer);
    string GetGameInfo();
}
