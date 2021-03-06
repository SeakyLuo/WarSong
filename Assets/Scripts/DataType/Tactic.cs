﻿[System.Serializable]
public class Tactic {

    public string tacticName;
    public int oreCost;
    public int goldCost;
    public bool original = true;
    public int ownerID;

    public Tactic() { }

    public Tactic(TacticAttributes tacticAttributes, int owner = 0, bool original = true)
    {
        tacticName = tacticAttributes.Name;
        oreCost = tacticAttributes.oreCost;
        goldCost = tacticAttributes.goldCost;
        if (owner == 0) ownerID = Login.playerID;
        else ownerID = owner;
        this.original = original;
    }

    public Tactic(string name, int oreCost, int goldCost, int owner = 0, bool original = true)
    {
        tacticName = name;
        this.oreCost = oreCost;
        this.goldCost = goldCost;
        if (owner == 0) ownerID = Login.playerID;
        else ownerID = owner;
        this.original = original;
    }

    public Tactic(Collection collection, int owner = 0, bool original = true)
    {
        tacticName = collection.name;
        TacticAttributes attributes = Database.FindTacticAttributes(tacticName);
        oreCost = attributes.oreCost;
        goldCost = attributes.goldCost;
        ownerID = owner;
        this.original = original;
    }

    public bool IsAlly() { return ownerID == Login.playerID; }

    public static bool operator < (Tactic tactic1, Tactic tactic2)
    {
        return tactic1.oreCost < tactic2.oreCost ||
                (tactic1.oreCost == tactic2.oreCost && tactic1.goldCost < tactic2.goldCost) ||
                (tactic1.oreCost == tactic2.oreCost && tactic1.goldCost == tactic2.goldCost && tactic1.tacticName.CompareTo(tactic2.tacticName) < 0);
    }

    public static bool operator >(Tactic tactic1, Tactic tactic2) { return !(tactic1 < tactic2); } // Because tactics can't be the same.
}