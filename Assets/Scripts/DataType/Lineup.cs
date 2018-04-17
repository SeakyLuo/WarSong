using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Lineup
{
    public Dictionary<Vector2Int, Collection> cardLocations;
    public List<string> tactics;
    public string boardName, lineupName;
    public bool complete;
    public static int tacticLimit = 10;

    public Lineup()
    {
        cardLocations = new Dictionary<Vector2Int, Collection>();
        tactics = new List<string>();
        boardName = "Standard Board";
        lineupName = "Custom Lineup";
        complete = false;
    }

    public Lineup(Dictionary<Vector2Int, Collection> cardLoc, List<string> Tactics, string BoardName, string LineupName)
    {
        SetInfo(cardLoc, Tactics, BoardName, LineupName);
    }

    public void SetInfo(Dictionary<Vector2Int, Collection> cardLoc, List<string> Tactics, string BoardName, string LineupName)
    {
        cardLocations = cardLoc;
        tactics = Tactics;
        boardName = BoardName;
        lineupName = LineupName;
        if (tactics.Count == tacticLimit) complete = true;
    }

    public bool IsEmpty()
    {
        return this == new Lineup();
    }
}

public class EnemyLineup: Lineup
{
    public EnemyLineup()
    {
        cardLocations = new Dictionary<Vector2Int, Collection>()
                    {
                        {new Vector2Int(4,9), Collection.General},
                        {new Vector2Int(3,9), Collection.Advisor },{new Vector2Int(5,9), Collection.Advisor },
                        {new Vector2Int(2,9), Collection.Elephant },{new Vector2Int(6,9), Collection.Elephant },
                        {new Vector2Int(1,9), Collection.Horse },{new Vector2Int(7,9), Collection.Horse },
                        {new Vector2Int(0,9), Collection.Chariot },{new Vector2Int(8,9), Collection.Chariot },
                        {new Vector2Int(1,7), Collection.Cannon },{new Vector2Int(7,7), Collection.Cannon },
                        {new Vector2Int(0,6), Collection.Soldier },{new Vector2Int(2,6), Collection.Soldier },
                        {new Vector2Int(4,6), Collection.Soldier },{new Vector2Int(6,6), Collection.Soldier },{new Vector2Int(8,6), Collection.Soldier }
                    };
    }
}