using UnityEngine;

public class Piece
{
    public static Vector2Int noLocation = new Vector2Int(-1, -1);
    private Collection collection;
    private Vector2Int castle, location;
    private bool isAlly, active = true;    

    public Piece(Collection setupCollection, Vector2Int loc, bool IsAlly)
    {
        collection = setupCollection;
        castle = loc;
        location = loc;
        isAlly = IsAlly;
    }

    public int GetHealth() { return collection.health; }
    public bool IsStandard() { return collection.name.StartsWith("Standard "); }
    public string GetPieceType() { return collection.type; }
    public Vector2Int GetLocation() { return location; }
    public Vector2Int GetCastle() { return castle; }
    public void SetLocation(Vector2Int loc) { location = loc; }
    public void SetCastle(Vector2Int loc) { castle = loc; }
    public bool IsAlly() { return isAlly; }
    public bool IsActive() { return active; }
    public void SetActive(bool status)
    {
        active = status;
        if(active)
        {
            location = castle;
        }
        else
        {
            location = noLocation;
        }
    }
}
