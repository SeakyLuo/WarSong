using UnityEngine;

public class Collection
{
    public string name;
    public string type;
    public int count;
    public int health;

    public Collection() { }

    public Collection(PieceAttributes attributes, int Count = 1, int Health = 0)
    {
        name = attributes.Name;
        type = attributes.type;
        count = Count;
        health = Health;
        if (health == 0) health = attributes.health;
    }

    public Collection(string tacticName, int Count = 1)
    {
        name = tacticName;
        type = "Tactic";
        count = Count;
        health = Resources.Load<TacticAttributes>("Tactics/Info/" + tacticName + "/Attributes").goldCost;
    }

    public Collection(TacticAttributes attributes, int Count = 1)
    {
        name = attributes.Name;
        type = "Tactic";
        count = Count;
        health = attributes.goldCost;
    }

    public Collection(string Name, string Type, int Count = 1, int Health = 0)
    {
        name = Name;
        type = Type;
        count = Count;
        health = Health;
        if (Type == "Tactic") health = Resources.Load<TacticAttributes>("Tactics/Info/" + Name + "/Attributes").goldCost;
        else if (Health == 0) health = Resources.Load<PieceAttributes>("Pieces/Info/" + Name + "/Attributes").health;
    }
}
