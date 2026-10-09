public interface IAiStrategy
{
    public Move chooseMove(Game game);
}

public class SmartGomokuStrategy : IAiStrategy
{
    public Move chooseMove(Game game)
    {
        Board board = game.Board;
        Player computer = game.CurrentPlayer;
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

        int[,] cells = board.GetCells();

        // Find out my possible 5-in-row, 4-in-row, 3-in-roow... and line up
        for (int target = 5; target >= 2; target--)
        {
            for (int row = 0; row < board.Size; row++)
            {
                for (int col = 0; col < board.Size; col++)
                {
                    int cellValue = cells[row, col];
                    if (cellValue == 0)
                    {
                        if (FindLongestLine(cells, row, col, computerId, target))
                        {
                            return new Move(row + 1, col + 1, 'O', computer);
                        }
                    }
                }
            }
        // Find out opponent possible 5-in-row, 4-in-row, 3-in-roow... and block it
            for (int row = 0; row < board.Size; row++)
            {
                for (int col = 0; col < board.Size; col++)
                {
                    int cellValue = cells[row, col];
                    if (cellValue == 0)
                    {
                        if (FindLongestLine(cells, row, col, opponentId, target))
                        {
                            return new Move(row + 1, col + 1, 'O', computer);
                        }
                    }
                }
            }
        }

        //find random possible place if cannot find any 2 in row 
        List<Move> possibleMoves = new List<Move>();

        for (int row = 0; row < board.Size; row++)
        {
            for (int col = 0; col < board.Size; col++)
            {
                if (cells[row, col] == 0)
                {
                       possibleMoves.Add(new Move(row+1,col+1,'O',computer));
                }
            }
        }

        if(possibleMoves.Count ==0)
            throw new Exception ("No empty cells");

        Random rand = new Random();
        int index = rand.Next(0,possibleMoves.Count);
        return possibleMoves[index];

    }



    //Matching cell values with player and return cell contents
    private bool PlayerStone(int playerCellValue, int playerId)
    {
        if (playerId == 1)
        {
            return playerCellValue == 1 || playerCellValue == 3;
        }
        else if (playerId == 2)
        {
            return playerCellValue == 2 || playerCellValue == 4;
        }
        return false;
    }

    //combining all winning  methods
    private bool FindLongestLine(int[,] cells, int row, int col, int playerId, int target)
    {
        if (CheckHorizontal(cells, row, col, playerId, target) == true)
        {
            return true;
        }

        if (CheckVertical(cells, row, col, playerId, target))
        {
            return true;
        }

        if (CheckDiagonal1(cells, row, col, playerId, target) == true)
        {
            return true;
        }

        if (CheckDiagonal2(cells, row, col, playerId, target))
        {
            return true;
        }

        return false;
    }

    //check horizontal win
    private bool CheckHorizontal(int[,] cells, int row, int col, int playerId, int target)
    {
        int count = 1;


        for (int i = col + 1; i < cells.GetLength(1); i++)
        {
            if (PlayerStone(cells[row, i], playerId))
            {
                count += 1;
            }
            else
            {
                break;
            }
        }

        for (int i = col - 1; i >= 0; i--)
        {
            if (PlayerStone(cells[row, i], playerId))
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
    private bool CheckVertical(int[,] cells, int row, int col, int playerId, int target)
    {
        int count = 1;


        for (int i = row + 1; i < cells.GetLength(0); i++)
        {
            if (PlayerStone(cells[i, col], playerId))
            {
                count += 1;
            }
            else
            {
                break;
            }
        }

        for (int i = row - 1; i >= 0; i--)
        {
            if (PlayerStone(cells[i, col], playerId))
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
    private bool CheckDiagonal1(int[,] cells, int row, int col, int playerId, int target)
    {
        int count = 1;

        for (int i = row + 1, j = col + 1; i < cells.GetLength(0) && j < cells.GetLength(1); i++, j++)
        {

            if (PlayerStone(cells[i, j], playerId))
            {
                count += 1;
            }
            else
            {
                break;
            }
        }

        for (int i = row - 1, j = col - 1; i >= 0 && j >= 0; i--, j--)
        {

            if (PlayerStone(cells[i, j], playerId))
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
    private bool CheckDiagonal2(int[,] cells, int row, int col, int playerId, int target)
    {

        int count = 1;

        for (int i = row + 1, j = col - 1; i < cells.GetLength(0) && j >= 0; i++, j--)
        {

            if (PlayerStone(cells[i, j], playerId))
            {
                count += 1;
            }
            else
            {
                break;
            }
        }

        for (int i = row - 1, j = col + 1; i >= 0 && j < cells.GetLength(1); i--, j++)
        {

            if (PlayerStone(cells[i, j], playerId))
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

