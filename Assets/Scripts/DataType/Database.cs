using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class Database {
    public static Dictionary<string, PieceAttributes> standardAttributes = new Dictionary<string, PieceAttributes>();
    public static List<PieceAttributes> pieces = new List<PieceAttributes>();
    public static List<TacticAttributes> tactics = new List<TacticAttributes>();
    public static Dictionary<string, List<PieceAttributes>> pieceDict = new Dictionary<string, List<PieceAttributes>>()
    {
        {"General", new List<PieceAttributes>() }, {"Advisor", new List<PieceAttributes>() }, {"Elephant", new List<PieceAttributes>() },
        {"Horse", new List<PieceAttributes>() }, {"Chariot", new List<PieceAttributes>() }, {"Cannon", new List<PieceAttributes>() }, {"Soldier", new List<PieceAttributes>() }
    };
    public static List<BoardAttributes> boards = new List<BoardAttributes>();
    public static List<Trap> traps = new List<Trap>();
    public static List<Mission> missions = new List<Mission>();
    public static List<ContractAttributes> contracts = new List<ContractAttributes>();

    public Database()
    {
        FindAttributes("Standard Piece");
        FindAttributes("Piece");
        FindAttributes("Tactic");
        FindAttributes("Trap");
        FindAttributes("Contract");;
    }

    private void FindAttributes(string type)
    {
        string path = "Assets/Resources/" + type + "/";
        foreach(string file in Directory.GetFiles(path))
        {
            string folder = file.Substring(path.Length);
            folder = folder.Substring(0, folder.Length - 5);
            if(type == "Standard Piece")
            {
                PieceAttributes pieceAttributes = FindPieceAttributes(folder);
                standardAttributes.Add(pieceAttributes.Name, pieceAttributes);
            }
            else if (type == "Piece")
            {
                PieceAttributes pieceAttributes = FindPieceAttributes(folder);
                pieceDict[pieceAttributes.type].Add(pieceAttributes);
                pieces.Add(pieceAttributes);
            }
            else if (type == "Tactic") tactics.Add(FindTacticAttributes(folder));
            else if (type == "Board") boards.Add(FindBoardAttributes(folder));
            else if (type == "Trap") traps.Add(FindTrapAttributes(folder));
            else if (type == "Contract") contracts.Add(FindContractAttributes(folder));
        }
    }

    public static BoardAttributes FindBoardAttributes(string boardName) { return Resources.Load<BoardAttributes>("Board/" + boardName + "/Attributes"); }
    public static PieceAttributes FindPieceAttributes(string pieceName)
    {
        if (pieceName.StartsWith("Standard ")) return Resources.Load<PieceAttributes>("Standard Piece/" + pieceName + "/Attributes");
        return Resources.Load<PieceAttributes>("Piece/" + pieceName + "/Attributes");
    }
    public static TacticAttributes FindTacticAttributes(string tacticName) { return Resources.Load<TacticAttributes>("Tactic/" + tacticName + "/Attributes"); }
    public static Trap FindTrapAttributes(string trapName) { return Resources.Load<Trap>("Trap/" + trapName + "/Attributes"); }
    public static ContractAttributes FindContractAttributes(string contractName) { return Resources.Load<ContractAttributes>("Contract/" + contractName + "/Attributes"); }
}