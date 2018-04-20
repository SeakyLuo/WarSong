using UnityEngine;

public class Piece
{
    private Collection collection;
    private Vector2Int startLocation, location;
    private bool isAlly, active = true;
    private static Vector2Int noLoction = new Vector2Int(-1, -1);

    public Piece(Collection setupCollection, Vector2Int loc, bool IsAlly)
    {
        collection = setupCollection;
        startLocation = loc;
        location = loc;
        isAlly = IsAlly;
    }

    public string GetPieceType() { return collection.type; }
    public Vector2Int GetLocation() { return location; }
    public Vector2Int GetStartLocation() { return startLocation; }
    public void SetLocation(Vector2Int loc) { location = loc; }
    public void SetStartLocation(Vector2Int loc) { startLocation = loc; }
    public bool IsAlly() { return isAlly; }
    public bool IsActive() { return active; }
    public void SetActive(bool status)
    {
        active = status;
        if(active)
        {
            location = startLocation;
        }
        else
        {
            location = noLoction;
        }
    }
}
