
using System.Runtime.CompilerServices;

public class RunGameEngline
{
    public static void Main(string[] args)
    {
        if(args.Length != 5)
        {
            new GameMenu().Run();
        }
        else
        {
            AutoTestMode(args);
        }
    }


    private static void AutoTestMode(string[] args)
    {
        //--game[0] gomoku[1] --variant[2] standard[3]"O5:3,O6:3,O5:4,O6:4,O5:5,O6:5,O5:6,O6:6,O5:7"[4]
        string game = args[1].ToLower();//gomoku
        string variant = args[3].ToLower();//variant
        string script = args[4];
        IGameFactory? factory;
        Board board;
        List<Player> players;
        IGame newgame;
        GameSession session;

        if(game == "gomoku" && variant == "standard")
        {
            variant = "classic";
        }

        factory = GameMenu.FindFactory(variant);
        if(factory ==null)
        {
            Console.WriteLine("Unknown variant:"+ variant);
        }
        else
        {
            board = factory.CreateBoard();
            players = factory.CreatePlayer("HvH", variant);
            newgame = factory.CreateGame(variant, board, players);
            session = new GameSession(newgame, variant, "HvH");
            session.RunScriptForAuto(script);
        }
    }
}