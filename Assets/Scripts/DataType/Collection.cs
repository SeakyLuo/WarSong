﻿using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Collection
{
    public string name = "";
    public string type = "";
    public int count = 1;
    public int health = 0;

    private int oreCost = 0; //tmp

    public static List<string> types = new List<string> { "General", "Advisor", "Elephant", "Horse", "Chariot", "Cannon", "Soldier", "Tactic" };
    public static Collection General = StandardCollection("General");
    public static Collection Advisor = StandardCollection("Advisor");
    public static Collection Elephant = StandardCollection("Elephant");
    public static Collection Horse = StandardCollection("Horse");
    public static Collection Chariot = StandardCollection("Chariot");
    public static Collection Cannon = StandardCollection("Cannon");
    public static Collection Soldier = StandardCollection("Soldier");
    public static Dictionary<string, Collection> standardCollectionDict = new Dictionary<string, Collection>
    {
        {"General", General },
        {"Advisor", Advisor },
        {"Elephant", Elephant },
        {"Horse", Horse },
        {"Chariot", Chariot },
        {"Cannon", Cannon },
        {"Soldier", Soldier }
    };

    public Collection() { }

    public Collection(PieceAttributes attributes, int Count = 1, int Health = 0)
    {
        name = attributes.Name;
        type = attributes.type;
        count = Count;
        oreCost = attributes.oreCost;
        health = Health;
        if (Health == 0) health = attributes.health;
    }

    public Collection(string tacticName, int Count = 1)
    {
        name = tacticName;
        type = "Tactic";
        count = Count;
        TacticAttributes attributes = InfoLoader.FindTacticAttributes(tacticName);
        oreCost = attributes.oreCost;
        health = attributes.goldCost;
    }

    public Collection(Tactic tactic, int Count = 1)
    {
        name = tactic.tacticName;
        count = Count;
        oreCost = tactic.oreCost;
        health = tactic.goldCost;
    }

    public Collection(TacticAttributes attributes, int Count = 1)
    {
        name = attributes.Name;
        type = "Tactic";
        count = Count;
        oreCost = attributes.oreCost;
        health = attributes.goldCost;
    }

    public Collection(string Name, string Type, int Count = 1, int Health = 0)
    {
        name = Name;
        type = Type;
        count = Count;
        health = Health;
        if (Type == "Tactic")
        {
            TacticAttributes attributes = InfoLoader.FindTacticAttributes(Name);
            health = attributes.goldCost;
            oreCost = attributes.oreCost;
        }
        else if (!Name.StartsWith("Standard "))
        {
            PieceAttributes attributes = InfoLoader.FindPieceAttributes(Name);
            if (Health == 0) health = attributes.health;
            oreCost = attributes.oreCost;
        }
    }

    public static Collection StandardCollection(string type)
    {
        return new Collection("Standard " + type, type);
    }

    public bool IsEmpty()
    {
        return name == "" && type == "" && count == 1 && health == 0;
    }

    public bool LessThan(Collection collection)
    {
        if(type == "Tactic" && collection.type == type)
        {
            return (oreCost < collection.oreCost) ||
                   (oreCost == collection.oreCost && health < collection.health) ||
                   (oreCost == collection.oreCost && health == collection.health && name.CompareTo(collection.name) < 0);
        }
        else
        {
            int typeIndex = types.IndexOf(type), collectionTypeIndex = types.IndexOf(collection.type);
            return (typeIndex < collectionTypeIndex) ||
                   (typeIndex == collectionTypeIndex && oreCost < collection.oreCost) ||
                   (typeIndex == collectionTypeIndex && oreCost == collection.oreCost && name.CompareTo(collection.name) < 0) ||
                   (typeIndex == collectionTypeIndex && name == collection.name && health < collection.health);
        }
    }

    public bool GreaterThan(Collection collection)
    {
        return !LessThan(collection) && !Equals(collection);
    }

    public bool Equals(Collection collection)
    {
        return type == collection.type && oreCost == collection.oreCost && name == collection.name && health == collection.health;
    }
}