using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Lineup
{
    public Dictionary<Vector2, Collection> cardLocations;
    public List<string> tactics;
    public string boardName, lineupName;
    public bool complete;
    public static int tacticLimit = 10;

    public Lineup()
    {
        cardLocations = new Dictionary<Vector2, Collection>();
        tactics = new List<string>();
        boardName = "Standard Board";
        lineupName = "Custom Lineup";
    }

    public Lineup(Dictionary<Vector2, Collection> cardLoc, List<string> Tactics, string BoardName, string LineupName)
    {
        SetInfo(cardLoc, Tactics, BoardName, LineupName);
    }

    public void SetInfo(Dictionary<Vector2, Collection> cardLoc, List<string> Tactics, string BoardName, string LineupName)
    {
        cardLocations = cardLoc;
        tactics = Tactics;
        boardName = BoardName;
        lineupName = LineupName;
    }

    public bool IsEmpty()
    {
        return this == new Lineup();
    }


}
