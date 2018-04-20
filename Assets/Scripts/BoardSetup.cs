using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardSetup : MonoBehaviour {

    public BoardAttributes boardAttributes;
    public Canvas boardCanvas;
    private Dictionary<Vector2Int, GameObject> pieces = new Dictionary<Vector2Int, GameObject>();

    public void Setup(Lineup lineup, bool isAlly)
    {
        foreach (KeyValuePair<Vector2Int,Collection> pair in lineup.cardLocations)
        {
            Vector2Int loc = pair.Key;
            if (!isAlly)
                loc = new Vector2Int(boardAttributes.boardWidth - pair.Key.x - 1, boardAttributes.boardHeight - pair.Key.y - 1);
            GameObject pieceObj = boardCanvas.transform.Find(Vec2ToString(loc) + "/Piece").gameObject;
            pieceObj.GetComponent<PieceInfo>().Setup(pair.Value, loc, isAlly);
            pieces.Add(loc, pieceObj);
            GameInfo.Add(pieceObj.GetComponent<PieceInfo>().GetPiece(), loc);
        }
    }

    public void Reactivate(Piece piece)
    {
        Vector2Int loc = piece.GetStartLocation();
        pieces[loc].SetActive(true);
        GameInfo.Add(piece, loc, true);
    }

    public void Deactivate(Piece piece)
    {
        Vector2Int loc = piece.GetStartLocation();
        pieces[loc].SetActive(false);
        GameInfo.Remove(piece, loc);
    }

    private string Vec2ToString(Vector2Int vec) { return vec.x.ToString() + vec.y.ToString(); }
}
