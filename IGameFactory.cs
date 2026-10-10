public interface IGameFactory
{
    Board CreateBoard();

    List<Player> CreatePlayer(string mode, string variant);

    IGame CreateGame(string variant, Board board, List<Player> players);
}
