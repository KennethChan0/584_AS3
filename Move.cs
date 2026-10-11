public class Move
{
    public int Col{get;}
    public int Row{get;}
    public char Stone{get;}
    public Player player{get;} 

    public Move(int row, int col, char stone, Player player)
    {
        Col = col;
        Row = row;
        Stone = stone;
        this.player = player;
    }
}