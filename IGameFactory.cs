public interface IGameFactory
{
    Board CreateBoard();

    List<Player> CreatePlayer(string mode, string variant);

    Game CreateGame(string variant, Board board, List<Player> players);
}
