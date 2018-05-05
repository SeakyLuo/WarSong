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

    public static void Reactivate(Piece piece)
    {
        boardSetup.pieces[piece.GetCastle()].SetActive(true);
        GameInfo.Add(piece, true);
    }

    public static void Deactivate(Piece piece)
    {
        Vector2Int loc = piece.GetCastle();
        boardSetup.pieces[loc].SetActive(false);
        GameInfo.Remove(piece, loc);
    }

    public static void Deactivate(Vector2Int loc)
    {
        Vector2Int castle = GameInfo.board[loc].GetCastle();
        boardSetup.pieces[castle].SetActive(false);
        GameInfo.Remove(GameInfo.board[loc], loc);
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
