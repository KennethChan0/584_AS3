public void SaveGame()
{
    const string FILENAME = "Save.txt";
    string saveData = board.GetMemory();
    FileStream saveFile = new FileStream(FILENAME, FileMode.Create, FileAccess.Write);
    StreamWriter loadFile = new StreamWriter(saveFile);
        
    loadFile.WriteLine(saveData); //loadData
    loadFile.WriteLine(currentPlayer.GetPlayerName()); // currentPlayerName
    loadFile.WriteLine(turnCount); // round
    loadFile.WriteLine($"{P1.GetHeavyCount()},{P1.GetEraserCount()}"); // pOneItems
    loadFile.WriteLine($"{P2!.GetHeavyCount()},{P2.GetEraserCount()}"); // pTwoItems
    loadFile.WriteLine(P2!.GetPlayerName()); // pTwoName


    loadFile.Close();
    saveFile.Close();
}

public void LoadGame()
{

    const string FILENAME = "Save.txt";

    if(!File.Exists(FILENAME))
        {
            Console.WriteLine("Can not find the file");
            return;
        }

    FileStream readFile = new FileStream(FILENAME, FileMode.Open, FileAccess.Read);
    StreamReader reader = new StreamReader(readFile);

    string? loadData = reader.ReadLine();
    string? currentPlayerName = reader.ReadLine();
    string? round = reader.ReadLine();
    string? pOneItems = reader.ReadLine();
    string? pTwoItems = reader.ReadLine();
    string? pTwoName = reader.ReadLine();

    reader.Close();
    readFile.Close();

    if(pTwoName != null)
        {
            if(pTwoName == "Computer")
            {
                P2 = new Player("Computer", " O ", " # ");
            }
            else
            {
                P2 = new Player("P2", " O ", " # ");
            }
        }

    if(loadData != null)
        {
            board.GetBoard(loadData);
        }

    if(currentPlayerName != null)
        {
            if(currentPlayerName == P1.GetPlayerName())
            {
                currentPlayer = P1;
            }
            else if(P2 != null && currentPlayerName == P2.GetPlayerName())
            {
                currentPlayer = P2;
            }
        }

    if(int.TryParse(round, out int parseRound))
        {
            turnCount = parseRound;
        }
    else
        {
            turnCount = 0;
        }

    if(pOneItems != null)
        {
            string[] p1Items = pOneItems.Split(',');
            if(p1Items.Length ==2)
            {
                int.TryParse(p1Items[0], out int p1Heavy);
                int.TryParse(p1Items[1], out int p1Eraser);

                P1.SetItems(p1Heavy, p1Eraser);
            }
        }

    if(pTwoItems != null)
        {
            string[] p2Items = pTwoItems.Split(',');
            if(p2Items.Length ==2)
            {
                int.TryParse(p2Items[0], out int p2Heavy);
                int.TryParse(p2Items[1], out int p2Eraser);

                P2!.SetItems(p2Heavy, p2Eraser);
            }
        }

    isGameOver = false;

    }