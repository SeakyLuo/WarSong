﻿using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardInfo : MonoBehaviour
{
    public BoardAttributes attributes;
    public Dictionary<Location, Collection> cardLocations = new Dictionary<Location, Collection>();
    public Dictionary<string, PieceAttributes> attributesDict = new Dictionary<string, PieceAttributes>();
    public Dictionary<string, List<Location>> typeLocations = new Dictionary<string, List<Location>>();
    public Dictionary<string, string> locationType = new Dictionary<string, string>();
    public Dictionary<Location, Collection> standardLocations;

    private void Awake()
    {
        if (standardLocations == null) SetupStandardLocation();
        DataSetup();
        GameObject.Find("Collection").GetComponent<CollectionGestureHandler>().SetBoardInfo(this);
        transform.parent.parent.parent.GetComponent<LineupBuilder>().SetBoardInfo(this);
        transform.parent.GetComponent<LineupBoardGestureHandler>().SetBoardInfo(this);
    }

    public void Reset()
    {
        SetAttributes(attributes);
    }

    private void SetupStandardLocation()
    {
        standardLocations = new Dictionary<Location, Collection>
        {
            {attributes.agloc, Collection.General },
            {attributes.aaloc1, Collection.Advisor },{attributes.aaloc2, Collection.Advisor },
            {attributes.aeloc1, Collection.Elephant },{attributes.aeloc2, Collection.Elephant },
            {attributes.ahloc1, Collection.Horse },{attributes.ahloc2, Collection.Horse },
            {attributes.arloc1, Collection.Chariot },{attributes.arloc2, Collection.Chariot },
            {attributes.acloc1, Collection.Cannon },{attributes.acloc2, Collection.Cannon },
            {attributes.asloc1, Collection.Soldier },{attributes.asloc2, Collection.Soldier },
            {attributes.asloc3, Collection.Soldier },{attributes.asloc4, Collection.Soldier },
            {attributes.asloc5, Collection.Soldier }
        };
    }

    public void SetAttributes(string boardName, Dictionary<Location, Collection> newLocations)
    {
        SetAttributes(Database.FindBoardAttributes(boardName), newLocations);
    }

    public void SetAttributes(BoardAttributes board, Dictionary<Location, Collection> newLocations = null)
    {
        attributes = board;
        if (standardLocations == null) SetupStandardLocation();
        DataSetup(newLocations);
        Color tmpColor;
        foreach (KeyValuePair<string, PieceAttributes> pair in attributesDict)
        {
            Image image = transform.Find(pair.Key).Find("CardImage").GetComponent<Image>();
            image.sprite = pair.Value.image;
            tmpColor = image.color;
            tmpColor.a = 255;
            image.color = tmpColor;
        }
    }

    public void SetStandardCard(string type, Location location)
    {
        SetCard(Collection.StandardCollection(type), location);
    }

    public void SetCard(Collection collection, Location location)
    {
        collection.count = 1;
        cardLocations[location] = collection;
        string locName = location.ToString();
        attributesDict[locName] = Database.FindPieceAttributes(collection.name);
    }

    public void SetCard(PieceAttributes attributes, Location location)
    {
        cardLocations[location] = new Collection(attributes.Name, attributes.type, 1, attributes.health);
        attributesDict[location.ToString()] = attributes;
    }

    private void DataSetup(Dictionary<Location, Collection> newLocations = null)
    {
        if (newLocations == null)
        {
            newLocations = standardLocations;
            cardLocations = newLocations;
        }
        else
        {
            cardLocations = new Dictionary<Location, Collection>
            {
                { attributes.agloc, newLocations[attributes.agloc] },
                { attributes.aaloc1,newLocations[attributes.aaloc1] },{ attributes.aaloc2,newLocations[attributes.aaloc2] },
                { attributes.aeloc1,newLocations[attributes.aeloc1] },{ attributes.aeloc2,newLocations[attributes.aeloc2] },
                { attributes.ahloc1,newLocations[attributes.ahloc1] },{ attributes.ahloc2,newLocations[attributes.ahloc2] },
                { attributes.arloc1,newLocations[attributes.arloc1] },{ attributes.arloc2,newLocations[attributes.arloc2] },
                { attributes.acloc1,newLocations[attributes.acloc1] },{ attributes.acloc2,newLocations[attributes.acloc2] },
                { attributes.asloc1,newLocations[attributes.asloc1] },{ attributes.asloc2,newLocations[attributes.asloc2] },
                { attributes.asloc3,newLocations[attributes.asloc3] },{ attributes.asloc4,newLocations[attributes.asloc4] },{ attributes.asloc5,newLocations[attributes.asloc5] }
            };
        }
        attributesDict = new Dictionary<string, PieceAttributes>
        {
            { attributes.agloc.ToString(), Database.FindPieceAttributes(newLocations[attributes.agloc].name) },
            { attributes.aaloc1.ToString(), Database.FindPieceAttributes(newLocations[attributes.aaloc1].name) },
            { attributes.aaloc2.ToString(), Database.FindPieceAttributes(newLocations[attributes.aaloc2].name) },
            { attributes.aeloc1.ToString(), Database.FindPieceAttributes(newLocations[attributes.aeloc1].name) },
            { attributes.aeloc2.ToString(), Database.FindPieceAttributes(newLocations[attributes.aeloc2].name) },
            { attributes.ahloc1.ToString(), Database.FindPieceAttributes(newLocations[attributes.ahloc1].name) },
            { attributes.ahloc2.ToString(), Database.FindPieceAttributes(newLocations[attributes.ahloc2].name) },
            { attributes.arloc1.ToString(), Database.FindPieceAttributes(newLocations[attributes.arloc1].name) },
            { attributes.arloc2.ToString(), Database.FindPieceAttributes(newLocations[attributes.arloc2].name) },
            { attributes.acloc1.ToString(), Database.FindPieceAttributes(newLocations[attributes.acloc1].name) },
            { attributes.acloc2.ToString(), Database.FindPieceAttributes(newLocations[attributes.acloc2].name) },
            { attributes.asloc1.ToString(), Database.FindPieceAttributes(newLocations[attributes.asloc1].name) },
            { attributes.asloc2.ToString(), Database.FindPieceAttributes(newLocations[attributes.asloc2].name) },
            { attributes.asloc3.ToString(), Database.FindPieceAttributes(newLocations[attributes.asloc3].name) },
            { attributes.asloc4.ToString(), Database.FindPieceAttributes(newLocations[attributes.asloc4].name) },
            { attributes.asloc5.ToString(), Database.FindPieceAttributes(newLocations[attributes.asloc5].name) }
        };
        typeLocations = new Dictionary<string, List<Location>>
        {
            { "General", new List <Location>{ attributes.agloc } },
            { "Advisor", new List<Location>{ attributes.aaloc1, attributes.aaloc2} },
            { "Elephant", new List<Location> { attributes.aeloc1, attributes.aeloc2 } },
            { "Horse", new List<Location> { attributes.ahloc1, attributes.ahloc2 } },
            { "Chariot", new List<Location> { attributes.arloc1, attributes.arloc2 } },
            { "Cannon", new List<Location> { attributes.acloc1, attributes.acloc2 } },
            { "Soldier", new List<Location> { attributes.asloc1, attributes.asloc2, attributes.asloc3, attributes.asloc4, attributes.asloc5 } }
        };
        locationType = new Dictionary<string, string>
        {
            { attributes.agloc.ToString(), "General" },
            { attributes.aaloc1.ToString(), "Advisor" },{ attributes.aaloc2.ToString(), "Advisor" },
            { attributes.aeloc1.ToString(), "Elephant" },{ attributes.aeloc2.ToString(), "Elephant" },
            { attributes.ahloc1.ToString(), "Horse" },{ attributes.ahloc2.ToString(), "Horse" },
            { attributes.arloc1.ToString(), "Chariot" },{ attributes.arloc2.ToString(), "Chariot" },
            { attributes.acloc1.ToString(), "Cannon" },{ attributes.acloc2.ToString(), "Cannon" },
            { attributes.asloc1.ToString(), "Soldier" },{ attributes.asloc2.ToString(), "Soldier" },
            { attributes.asloc3.ToString(), "Soldier" },{ attributes.asloc4.ToString(), "Soldier" },{ attributes.asloc5.ToString(), "Soldier" }
        };
    }
}