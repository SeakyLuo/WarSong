using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSetup : MonoBehaviour {

    public BoardAttributes boardAttributes;

    public void Setup(Lineup lineup, bool isAlly)
    {
        foreach (KeyValuePair<Vector2Int,Collection> pair in lineup.cardLocations)
        {
            Vector2Int vec = pair.Key;
            if (!isAlly)
                vec = new Vector2Int(boardAttributes.boardWidth - pair.Key.x - 1, boardAttributes.boardHeight - pair.Key.y - 1);
            string loc = Vec2ToString(vec);
            GameObject pieceObj = transform.Find(loc+"/Piece").gameObject;
            pieceObj.GetComponent<PieceInfo>().Setup(pair.Value, vec, isAlly);
            Piece piece = pieceObj.GetComponent<PieceInfo>().GetPiece();
            if (isAlly) GameInfo.activeAlly.Add(piece);
            else GameInfo.activeEnemy.Add(piece);
            GameInfo.board.Add(vec, piece);
        }
    }

    public void ReactivateAlly(GameObject obj, Vector2Int loc)
    {
        obj.SetActive(true);
        GameInfo.activeAlly.Add(GameInfo.board[loc]);
        GameInfo.inactiveAlly.Remove(GameInfo.board[loc]);
        GameInfo.board.Add(loc, GameInfo.board[loc]);
    }

    public void DeactivateAlly(GameObject obj, Vector2Int loc)
    {
        obj.SetActive(false);
        GameInfo.activeAlly.Remove(GameInfo.board[loc]);
        GameInfo.inactiveAlly.Add(GameInfo.board[loc]);
        GameInfo.board.Remove(loc);
    }

    public void ReactivateEnemy(GameObject obj, Vector2Int loc)
    {
        obj.SetActive(true);
        GameInfo.activeEnemy.Add(GameInfo.board[loc]);
        GameInfo.inactiveEnemy.Remove(GameInfo.board[loc]);
        GameInfo.board.Add(loc, GameInfo.board[loc]);
    }

    public void DeactivateEnemy(GameObject obj, Vector2Int loc)
    {
        obj.SetActive(false);
        GameInfo.activeEnemy.Remove(GameInfo.board[loc]);
        GameInfo.inactiveEnemy.Add(GameInfo.board[loc]);
        GameInfo.board.Remove(loc);
    }

    private string Vec2ToString(Vector2Int vec) { return vec.x.ToString() + vec.y.ToString(); }
}
