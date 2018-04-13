﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserInfo {

    public string username;
    public List<Collection> collections;
    public List<Lineup> lineups;
    public int coins;
    //public int[] winningCount;
    //public Quest[] quest;
    //public Progress[] progress;

    public UserInfo()
    {
        username = "WarSong Account";
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
        username = "WarSong CheatAccount";
        Collection[] cheat = { new Collection("Greeeeeat Elephant", "Elephant", 3, 5), new Collection("Zhuge Liang", "General"), new Collection("A Secret Plan", 3),
            new Collection("No Way", 100), new Collection("Qin Shihuang", "General"), new Collection("Xiao He", "General"),
             new Collection("Link Soldier","Soldier",11), new Collection("Buy 1 Get 1 Free",15), new Collection("Build A Cannon","Tactic"),
            new Collection("Build A Rook"),new Collection("Winner Trophy",5),new Collection("Horse Rider","Horse",4)
        };
        foreach (Collection c in cheat) collections.Add(c);
        //lineups = null;
        coins = 99999;
    }
}