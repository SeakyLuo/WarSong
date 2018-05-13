[System.Serializable]
public class Tactic {

    public string tacticName;
    public int oreCost;
    public int goldCost;

    public Tactic(TacticAttributes tacticAttributes)
    {
        tacticName = tacticAttributes.Name;
        oreCost = tacticAttributes.oreCost;
        goldCost = tacticAttributes.goldCost;
    }

    public Tactic(string name, int OreCost, int GoldCost)
    {
        tacticName = name;
        oreCost = OreCost;
        goldCost = GoldCost;
    }

    public static bool operator == (Tactic tactic1, Tactic tactic2)
    {
        return tactic1.oreCost == tactic2.oreCost && tactic1.goldCost == tactic2.goldCost && tactic1.tacticName == tactic2.tacticName;
    }

    public static bool operator !=(Tactic tactic1, Tactic tactic2) { return !(tactic1 == tactic2); }

    public static bool operator < (Tactic tactic1, Tactic tactic2)
    {
        return tactic1.oreCost < tactic2.oreCost ||
                (tactic1.oreCost == tactic2.oreCost && tactic1.goldCost < tactic2.goldCost) ||
                (tactic1.oreCost == tactic2.oreCost && tactic1.goldCost == tactic2.goldCost && tactic1.tacticName.CompareTo(tactic2.tacticName) < 0);
    }

    public static bool operator >(Tactic tactic1, Tactic tactic2) { return !(tactic1 < tactic2 && tactic1 != tactic2); } // Because tactics can't be the same.
}