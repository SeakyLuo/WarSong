using System.Collections.Generic;
using UnityEngine;

public class BoardSetup : MonoBehaviour {

    public BoardAttributes boardAttributes;
    public Transform boardCanvas;
    public Dictionary<Vector2Int, GameObject> pieces = new Dictionary<Vector2Int, GameObject>();

    public void Setup(Lineup lineup, bool isAlly)
    {
        foreach (KeyValuePair<Vector2Int,Collection> pair in lineup.cardLocations)
        {
            Vector2Int loc = pair.Key;
            if (!isAlly)
                loc = new Vector2Int(boardAttributes.boardWidth - pair.Key.x - 1, boardAttributes.boardHeight - pair.Key.y - 1);
            GameObject pieceObj = boardCanvas.Find(InfoLoader.Vec2ToString(loc) + "/Piece").gameObject;
            pieceObj.GetComponent<PieceInfo>().Setup(pair.Value, loc, isAlly);
            if(pieceObj.GetComponent<PieceInfo>().trigger != null) pieceObj.GetComponent<PieceInfo>().trigger.StartOfGame();
            pieces.Add(loc, pieceObj);
            GameInfo.Add(pieceObj.GetComponent<PieceInfo>().piece);
        }
        GameInfo.castles = new Dictionary<Vector2Int, Piece>(GameInfo.board);
    }

    public void Reactivate(Piece piece)
    {
        Vector2Int loc = piece.GetCastle();
        pieces[loc].SetActive(true);
        GameInfo.Add(piece, true);
    }

    public void Deactivate(Piece piece)
    {
        Vector2Int loc = piece.GetCastle();
        pieces[loc].SetActive(false);
        GameInfo.Remove(piece, loc);
    }
}
