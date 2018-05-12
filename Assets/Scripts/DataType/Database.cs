using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class Database {

    public static List<PieceAttributes> pieces = new List<PieceAttributes>();
    public static List<TacticAttributes> tactics = new List<TacticAttributes>();
    public static Dictionary<string, List<PieceAttributes>> attributes = new Dictionary<string, List<PieceAttributes>>()
    {
        {"General", new List<PieceAttributes>() }, {"Advisor", new List<PieceAttributes>() }, {"Elephant", new List<PieceAttributes>() },
        {"Horse", new List<PieceAttributes>() }, {"Chariot", new List<PieceAttributes>() }, {"Cannon", new List<PieceAttributes>() }, {"Soldier", new List<PieceAttributes>() }
    };
    public static List<BoardAttributes> boards = new List<BoardAttributes>();
    public static List<Trap> traps = new List<Trap>();
    public static List<Mission> missions = new List<Mission>();

    public Database()
    {
        //foreach(string path in Directory.GetFiles("Assets/Resources/Piece/"))
        //    pieces.Add(InfoLoader.FindPieceAttributes(path));
        //foreach (string path in Directory.GetFiles("Assets/Resources/Tactic/"))
        //    tactics.Add(InfoLoader.FindTacticAttributes(path));
        //foreach (string path in Directory.GetFiles("Assets/Resources/Board/"))
        //    boards.Add(InfoLoader.FindBoardAttributes(path));
        FindAttributes("Trap");
    }

    private void FindAttributes(string type)
    {
        string path = "Assets/Resources/" + type + "/";
        foreach(string file in Directory.GetFiles(path))
        {
            string folder = file.Substring(path.Length);
            folder = folder.Substring(0, folder.Length - 5);
            if (type == "Piece") pieces.Add(InfoLoader.FindPieceAttributes(folder));
            else if (type == "Tactic") tactics.Add(InfoLoader.FindTacticAttributes(folder));
            else if (type == "Board") boards.Add(InfoLoader.FindBoardAttributes(folder));
            else if (type == "Trap") traps.Add(InfoLoader.FindTrap(folder));
        }
    }
}