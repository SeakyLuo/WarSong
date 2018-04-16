using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceInfo : MonoBehaviour {
    public PieceAttributes pieceAttributes;

    private Vector2 startLocation, location;

    public string GetCardType()
    {
        return pieceAttributes.type;
    }

    public Vector2 GetLocation()
    {
        return location;
    }

    public void SetLocation(Vector2 loc)
    {
        location = loc;
    }

    public void SetStartLocation(Vector2 loc)
    {
        startLocation = loc;
    }
}
