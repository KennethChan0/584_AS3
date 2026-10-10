public class DumbReversiStrategy : IAiStrategy
{
    public Move ChooseMove(IGame game)
    {
        if (game is Reversi reversi)
        {
            Player computer = reversi.CurrentPlayer;
            List<Move> possibleMoves = reversi.GetPossibleMove(computer);

            Random rand = new Random();
            int index = rand.Next(0, possibleMoves.Count);
            return possibleMoves[index];
        }
        throw new Exception("This only supports Reversi");
    }
}

public class SmartReversiStrategy : IAiStrategy
{
    private int Flip { get; set; }
    private int Corner { get; set; }
    public SmartReversiStrategy(int flip, int corner)
    {
        Flip = flip;
        Corner = corner;
    }

    public Move ChooseMove(IGame game)
    {
        if (game is Reversi reversi)
        {
            Player computer = reversi.CurrentPlayer;
            List<Move> possibleMoves = reversi.GetPossibleMove(computer);
            int bestScore = -100000000;
            Move bestMove = possibleMoves[0];

            foreach (Move move in possibleMoves)
            {
                int flipNumber = reversi.GetFlips(move.Row, move.Col, computer).Count();

                int score = flipNumber * Flip;

                if ((move.Row == 1 || move.Row == reversi.Board.Size) && (move.Col == 1 || move.Col == reversi.Board.Size))
                {
                    score = score + Corner * reversi.Board.Size * reversi.Board.Size;
                }

                if (score > bestScore)
                {
                    bestScore = score;
                    bestMove = move;
                }
            }

            return bestMove;
        }
        throw new Exception("This only supports Reversi");


    }
}