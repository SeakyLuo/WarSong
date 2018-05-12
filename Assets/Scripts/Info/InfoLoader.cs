using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using System.IO;

public class InfoLoader : MonoBehaviour {

    public static UserInfo user;
    public static int playerID;
    public static Database database;
    public static string switchSceneCaller = "Main";
    public static Dictionary<string, PieceAttributes> standardAttributes = new Dictionary<string, PieceAttributes>();
    public PieceAttributes standardGeneral, standardAdvisor, standardElephant, standardHorse, standardChariot, standardCannon, standardSoldier;

    private void Awake()
    {
        user = new CheatAccount(); // This line should be
        // user = new UserInfo();
        // download Json
        // user.JsonToClass();
        playerID = user.playerID;
        database = new Database();
        standardAttributes = new Dictionary<string, PieceAttributes>(){
            { "Standard General", standardGeneral },
            { "Standard Advisor", standardAdvisor },
            { "Standard Elephant", standardElephant },
            { "Standard Horse", standardHorse },
            { "Standard Chariot", standardChariot },
            { "Standard Cannon", standardCannon },
            { "Standard Soldier", standardSoldier }
        };
    }

    public static string Vec2ToString(Vector2Int vec) { return vec.x.ToString() + vec.y.ToString(); }
    public static Vector2Int StringToVec2(string loc) { return new Vector2Int((int)Char.GetNumericValue(loc[0]), (int)Char.GetNumericValue(loc[1])); }
    public static BoardAttributes FindBoardAttributes(string boardName) { return Resources.Load<BoardAttributes>("Board/" + boardName + "/Attributes"); }
    public static PieceAttributes FindPieceAttributes(string pieceName)
    {
        if (pieceName.StartsWith("Standard ")) return InfoLoader.standardAttributes[pieceName];
        return Resources.Load<PieceAttributes>("Piece/" + pieceName + "/Attributes");
    }
    public static TacticAttributes FindTacticAttributes(string tacticName) { return Resources.Load<TacticAttributes>("Tactic/" + tacticName + "/Attributes"); }
    public static Trap FindTrap(string trapName) { return Resources.Load<Trap>("Trap/" + trapName + "/Attributes"); }
    public static ContractAttributes FindContractAttributes(string contractName) { return Resources.Load<ContractAttributes>("Contract/" + contractName + "/Attributes"); }
}