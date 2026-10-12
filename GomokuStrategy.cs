public class SmartGomokuStrategy : IAiStrategy
{
    public Move ChooseMove(IGame game)
    {
        if (game is Gomoku gomoku)
        {
            Board board = gomoku.Board;
            Player computer = gomoku.CurrentPlayer;
            int opponentId;

            int computerId = computer.Id;

            if (computerId == 1)
            {
                opponentId = 2;
            }
            else
            {
                opponentId = 1;
            }

            // Find out my possible 5-in-row, 4-in-row, 3-in-roow... and line up
            for (int target = 5; target >= 2; target--)
            {
                for (int row = 1; row <= board.Size; row++)
                {
                    for (int col = 1; col <= board.Size; col++)
                    {
                        if (board.GetCell(row, col) == 0)
                        {
                            if (FindLongestLine(board, row, col, computerId, target))
                            {
                                return new Move(row, col, 'O', computer);
                            }
                        }
                    }
                }
                // Find out opponent possible 5-in-row, 4-in-row, 3-in-roow... and block it
                for (int row = 1; row <= board.Size; row++)
                {
                    for (int col = 1; col <= board.Size; col++)
                    {
                        if (board.GetCell(row, col) == 0)
                        {
                            if (FindLongestLine(board, row, col, opponentId, target))
                            {
                                return new Move(row, col, 'O', computer);
                            }
                        }
                    }
                }
            }

            //find random possible place if cannot find any 2 in row 
            List<Move> possibleMoves = new List<Move>();

            for (int row = 1; row <= board.Size; row++)
            {
                for (int col = 1; col <= board.Size; col++)
                {
                    if (board.GetCell(row, col) == 0)
                    {
                        possibleMoves.Add(new Move(row, col, 'O', computer));
                    }
                }
            }

            if (possibleMoves.Count == 0)
                throw new Exception("No empty cells");

            Random rand = new Random();
            int index = rand.Next(0, possibleMoves.Count);
            return possibleMoves[index];
        }
        throw new Exception("This only supports Gomoku");
    }



    //Matching cell values with player and return cell contents
    private bool PlayerStone(int playerCellValue, int playerId)
    {
        // Board values: 1 = Player 1 ordinary, 2 = Player 1 heavy,
        //               3 = Player 2 ordinary, 4 = Player 2 heavy
        if (playerId == 1)
        {
            return playerCellValue == 1 || playerCellValue == 2;
        }
        else if (playerId == 2)
        {
            return playerCellValue == 3 || playerCellValue == 4;
        }
        return false;
    }

    //combining all winning  methods
    private bool FindLongestLine(Board board, int row, int col, int playerId, int target)
    {
        if (CheckHorizontal(board, row, col, playerId, target) == true)
        {
            return true;
        }

        if (CheckVertical(board, row, col, playerId, target))
        {
            return true;
        }

        if (CheckDiagonal1(board, row, col, playerId, target) == true)
        {
            return true;
        }

        if (CheckDiagonal2(board, row, col, playerId, target))
        {
            return true;
        }

        return false;
    }

    //check horizontal win
    private bool CheckHorizontal(Board board, int row, int col, int playerId, int target)
    {
        int count = 1;

        for (int i = col + 1; i <= board.Size; i++)
        {
            if (PlayerStone(board.GetCell(row, i), playerId))
            {
                count += 1;
            }
            else
            {
                break;
            }
        }

        for (int i = col - 1; i >= 1; i--)
        {
            if (PlayerStone(board.GetCell(row, i), playerId))
            {
                count += 1;
            }
            else
            {
                break;
            }
        }

        return count >= target;
    }

    //check vertical win
    private bool CheckVertical(Board board, int row, int col, int playerId, int target)
    {
        int count = 1;

        for (int i = row + 1; i <= board.Size; i++)
        {
            if (PlayerStone(board.GetCell(i, col), playerId))
            {
                count += 1;
            }
            else
            {
                break;
            }
        }

        for (int i = row - 1; i >= 1; i--)
        {
            if (PlayerStone(board.GetCell(i, col), playerId))
            {
                count += 1;
            }
            else
            {
                break;
            }
        }

        return count >= target;
    }

    //check backslash win \
    private bool CheckDiagonal1(Board board, int row, int col, int playerId, int target)
    {
        int count = 1;

        for (int i = row + 1, j = col + 1; i <= board.Size && j <= board.Size; i++, j++)
        {

            if (PlayerStone(board.GetCell(i, j), playerId))
            {
                count += 1;
            }
            else
            {
                break;
            }
        }

        for (int i = row - 1, j = col - 1; i >= 1 && j >= 1; i--, j--)
        {

            if (PlayerStone(board.GetCell(i, j), playerId))
            {
                count += 1;
            }
            else
            {
                break;
            }
        }

        return count >= target;
    }

    //check slash win /
    private bool CheckDiagonal2(Board board, int row, int col, int playerId, int target)
    {

        int count = 1;

        for (int i = row + 1, j = col - 1; i <= board.Size && j >= 1; i++, j--)
        {

            if (PlayerStone(board.GetCell(i, j), playerId))
            {
                count += 1;
            }
            else
            {
                break;
            }
        }

        for (int i = row - 1, j = col + 1; i >= 1 && j <= board.Size; i--, j++)
        {

            if (PlayerStone(board.GetCell(i, j), playerId))
            {
                count += 1;
            }
            else
            {
                break;
            }
        }

        return count >= target;
    }
}

public class DumbGomokuStrategy : IAiStrategy
{
    public Move ChooseMove(IGame game)
    {
        if (game is Gomoku gomoku)
        {
            Board board = gomoku.Board;
            Player computer = game.CurrentPlayer;

            //find random valid cell to place a stone
            List<Move> possibleMoves = new List<Move>();

            for (int row = 1; row <= board.Size; row++)
            {
                for (int col = 1; col <= board.Size; col++)
                {
                    if (board.GetCell(row, col) == 0)
                    {
                        possibleMoves.Add(new Move(row, col, 'O', computer));
                    }
                }
            }

            if (possibleMoves.Count == 0)
                throw new Exception("No empty cells");

            Random rand = new Random();
            int index = rand.Next(0, possibleMoves.Count);
            return possibleMoves[index];
        }
        throw new Exception("This only supports Gomoku");
    }
}
