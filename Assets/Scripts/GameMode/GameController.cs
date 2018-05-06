using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public static OnEnterGame onEnterGame;

    private static BoardSetup boardSetup;
    private static Dictionary<string, List<Vector2Int>> castles;

    // Use this for initialization
    void Start () {
        onEnterGame = GameObject.Find("UIPanel").GetComponent<OnEnterGame>();
        boardSetup = onEnterGame.boardSetup;
        castles = new Dictionary<string, List<Vector2Int>>()
        {
            {"Advisor", boardSetup.boardAttributes.AdvisorCastle() },
            {"Elephant", boardSetup.boardAttributes.ElephantCastle()  },
            {"Horse", boardSetup.boardAttributes.HorseCastle()  },
            {"Chariot", boardSetup.boardAttributes.ChariotCastle()  },
            {"Cannon", boardSetup.boardAttributes.CannonCastle()  },
            {"Soldier", boardSetup.boardAttributes.SoldierCastle()  },
        };
    }

    public static void AddPiece(Collection collection, Vector2Int castle, bool isAlly)
    {
        boardSetup.AddPiece(collection, castle, isAlly);
    }

    public static void Eliminate(Piece piece)
    {
        Destroy(boardSetup.pieces[piece.location]);
        boardSetup.pieces.Remove(piece.location);
        GameInfo.Remove(piece);
    }

    public static void Eliminate(Vector2Int loc)
    {
        Destroy(boardSetup.pieces[loc]);
        boardSetup.pieces.Remove(loc);
        GameInfo.Remove(GameInfo.board[loc]);
    }

    public static void ChangeOre(int deltaAmount)
    {
        GameInfo.ores[InfoLoader.user.playerID] += deltaAmount;
        onEnterGame.SetOreText();
    }

    public static void ChangeCoin(int deltaAmount)
    {
        InfoLoader.user.coins += deltaAmount;
        onEnterGame.SetCoinText();
    }

    public static List<Vector2Int> FindCastles(string type) { return castles[type]; }
}
