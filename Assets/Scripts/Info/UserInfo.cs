using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfo {

    public List<Collection> collections;
    public List<Lineup> lineups;
    public int coins;
    //public Quest[] quest;
    //public Progress[] progress;

    public UserInfo()
    {
        collections = new List<Collection> { new Collection("Space Witch", "General"), new Collection("Fat Soldier", "Soldier",4),new Collection("Cripple","Cannon",3),
            new Collection("Soldier Recruitment",5), new Collection("Advisor Recruitment"), new Collection("Greeeeeat Elephant","Elephant",3),
            new Collection("Tame An Elephant"),new Collection("Purchase An Horse"), new Collection("King Guardian","Advisor", 3),
            new Collection("Monster Hunter","Chariot",4),new Collection("Treasure Horse","Horse",100)};
        lineups = new List<Lineup>();
        coins = 0;
    }

}

public class CheatAccount:UserInfo
{
    public CheatAccount():base()
    {
        Collection[] cheat = { new Collection("Greeeeeat Elephant", "Elephant", 3, 5), new Collection("Zhuge Liang", "General"), new Collection("A Secret Plan", 3),
            new Collection("No Way", 100), new Collection("Qin Shihuang", "General"), new Collection("Xiao He", "General"),
             new Collection("Link Soldier","Soldier",11), new Collection("Buy 1 Get 1 Free",15), new Collection("Build A Cannon","Tactic"),
            new Collection("Build A Rook"),new Collection("Winner Trophy",5)
        };
        foreach (Collection c in cheat) collections.Add(c);
        //lineups = null;
        coins = 99999;
    }
}

[System.Serializable]
public class Lineup
{
    public Dictionary<Vector2, Collection> cardLocations;
    public List<string> tactics;
    public string boardName;
    public string lineupName;

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


}