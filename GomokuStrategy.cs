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
                                return new Move(row , col , 'O', computer);
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
                                return new Move(row , col , 'O', computer);
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
                        possibleMoves.Add(new Move(row , col , 'O', computer));
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
        if (CheckLine(board, row, col, 0,1,playerId, target) == true)
        {
            return true;
        }

        if (CheckLine(board, row, col, 1,0,playerId, target) == true)
        {
            return true;
        }

        if (CheckLine(board, row, col, 1,1,playerId, target) == true)
        {
            return true;
        }

        if (CheckLine(board, row, col, 1,-1,playerId, target) == true)
        {
            return true;
        }


        return false;
    }

    private int CountStones(Board board, int row, int col,int rowDirection, int colDirection, int playerId)
    {
        int count = 0;

        while(row>=1 && col>=1 && row<=board.Size && col <= board.Size&&PlayerStone(board.GetCell(row,col),playerId)==true)
        {
            count+=1;
            row+= rowDirection;
            col+=colDirection;
        }

        return count;
    }

    private bool CheckLine(Board board, int row, int col,int rowDirection, int colDirection, int playerId, int target)
    {
        int count =1;

        count += CountStones(board, row+rowDirection, col+colDirection, rowDirection,colDirection,playerId);
        count += CountStones(board, row-rowDirection, col-colDirection, -rowDirection,-colDirection,playerId);


        return count>= target;
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
                        possibleMoves.Add(new Move(row , col , 'O', computer));
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
