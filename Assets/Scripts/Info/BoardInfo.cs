using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoardInfo : MonoBehaviour {

    public BoardAttributes attributes;
    public GameObject boardObject;
    public Dictionary<Vector2, string> locations;
    public Dictionary<Vector2, Collection> cardLocations;
    public Dictionary<string, PieceAttributes> attributesDict;    
    public Dictionary<string, List<Vector2>> typeLocations;
    public Dictionary<string, string> typeDict;
    public Dictionary<Vector2, Collection> standardLocations;
    public static Dictionary<string, PieceAttributes> standardAttributes;

    public PieceAttributes standardGeneral, standardAdvisor, standardElephant, standardHorse, standardChariot, standardCannon, standardSoldier;

    private void Awake()
    {
        standardLocations = new Dictionary<Vector2, Collection>
        {
            {attributes.agloc,new Collection("Standard General", "General") },
            {attributes.aaloc1,new Collection("Standard Advisor", "Advisor") },{attributes.aaloc2,new Collection("Standard Advisor", "Advisor") },
            {attributes.aeloc1,new Collection("Standard Elephant", "Elephant") },{attributes.aeloc2,new Collection("Standard Elephant", "Elephant") },
            {attributes.ahloc1,new Collection("Standard Horse", "Horse") },{attributes.ahloc2,new Collection("Standard Horse", "Horse") },
            {attributes.arloc1,new Collection("Standard Chariot", "Chariot") },{attributes.arloc2,new Collection("Standard Chariot", "Chariot") },
            {attributes.acloc1,new Collection("Standard Cannon", "Cannon") },{attributes.acloc2,new Collection("Standard Cannon", "Cannon") },
            {attributes.asloc1,new Collection("Standard Soldier", "Soldier") },{attributes.asloc2,new Collection("Standard Soldier", "Soldier") },
            {attributes.asloc3,new Collection("Standard Soldier", "Soldier") },{attributes.asloc4,new Collection("Standard Soldier", "Soldier") },
            {attributes.asloc5,new Collection("Standard Soldier", "Soldier") }
        };
        standardAttributes = new Dictionary<string, PieceAttributes>
        {
            {"Standard General", standardGeneral },
            {"Standard Advisor", standardAdvisor },
            {"Standard Elephant", standardElephant },
            {"Standard Horse", standardHorse },
            {"Standard Chariot", standardChariot },
            {"Standard Cannon", standardCannon },
            {"Standard Soldier", standardSoldier }
        };
        boardObject = GameObject.Find("BoardObject(Clone)");
        DataSetup();
    }

    public void Reset()
    {
        SetAttributes(attributes);
    }

    public void SetAttributes(string boardName, Dictionary<Vector2, Collection> newLocations)
    {
        SetAttributes(Resources.Load<BoardAttributes>("Board/Info/" + boardName + "/Attributes"), newLocations);
    }

    public void SetAttributes(BoardAttributes board, Dictionary<Vector2, Collection> newLocations = null)
    {
        attributes = board;
        DataSetup(newLocations);
        if(boardObject == null) boardObject = GameObject.Find("BoardObject(Clone)");
        Color tmpColor;
        foreach(KeyValuePair<string, PieceAttributes> pair in attributesDict)
        {
            Image image = boardObject.transform.Find(pair.Key).Find("CardImage").GetComponent<Image>();
            image.sprite = pair.Value.image;
            tmpColor = image.color;
            tmpColor.a = 255;
            image.color = tmpColor;
        }
    }

    public void SetStandardCard(string type, Vector2 location)
    {
        SetCard(new Collection("Standard " + type, type), location);
    }

    public void SetCard(Collection collection, Vector2 location)
    {
        locations[location] = collection.name;
        cardLocations[location] = collection;
        attributesDict[Vector2ToString(location)] = LoadPieceAttributes(collection.name);
        attributesDict[Vector2ToString(location)].health = collection.health;
    }

    public void SetCard(PieceAttributes attributes ,Vector2 location)
    {
        locations[location] = attributes.Name;
        cardLocations[location] = new Collection(attributes.Name, attributes.type);
        attributesDict[Vector2ToString(location)] = attributes;
    }

    private void DataSetup(Dictionary<Vector2, Collection> newLocations = null)
    {
        if (newLocations == null) newLocations = standardLocations;
        locations = new Dictionary<Vector2, string>
        {
            { attributes.agloc, newLocations[attributes.agloc].name },
            { attributes.aaloc1,newLocations[attributes.aaloc1].name },{ attributes.aaloc2,newLocations[attributes.aaloc2].name },
            { attributes.aeloc1,newLocations[attributes.aeloc1].name },{ attributes.aeloc2,newLocations[attributes.aeloc2].name },
            { attributes.ahloc1,newLocations[attributes.ahloc1].name },{ attributes.ahloc2,newLocations[attributes.ahloc2].name },
            { attributes.arloc1,newLocations[attributes.arloc1].name },{ attributes.arloc2,newLocations[attributes.arloc2].name },
            { attributes.acloc1,newLocations[attributes.acloc1].name },{ attributes.acloc2,newLocations[attributes.acloc2].name },
            { attributes.asloc1,newLocations[attributes.asloc1].name },{ attributes.asloc2,newLocations[attributes.asloc2].name },
            { attributes.asloc3,newLocations[attributes.asloc3].name },{ attributes.asloc4,newLocations[attributes.asloc4].name },{ attributes.asloc5,newLocations[attributes.asloc5].name },
        };
        cardLocations = new Dictionary<Vector2, Collection>
        {
            { attributes.agloc, newLocations[attributes.agloc] },
            { attributes.aaloc1,newLocations[attributes.aaloc1] },{ attributes.aaloc2,newLocations[attributes.aaloc2] },
            { attributes.aeloc1,newLocations[attributes.aeloc1] },{ attributes.aeloc2,newLocations[attributes.aeloc2] },
            { attributes.ahloc1,newLocations[attributes.ahloc1] },{ attributes.ahloc2,newLocations[attributes.ahloc2] },
            { attributes.arloc1,newLocations[attributes.arloc1] },{ attributes.arloc2,newLocations[attributes.arloc2] },
            { attributes.acloc1,newLocations[attributes.acloc1] },{ attributes.acloc2,newLocations[attributes.acloc2] },
            { attributes.asloc1,newLocations[attributes.asloc1] },{ attributes.asloc2,newLocations[attributes.asloc2] },
            { attributes.asloc3,newLocations[attributes.asloc3] },{ attributes.asloc4,newLocations[attributes.asloc4] },{ attributes.asloc5,newLocations[attributes.asloc5] },            
        };
        attributesDict = new Dictionary<string, PieceAttributes>
        {
            { Vector2ToString(attributes.agloc), LoadPieceAttributes(newLocations[attributes.agloc].name) },
            { Vector2ToString(attributes.aaloc1), LoadPieceAttributes(newLocations[attributes.aaloc1].name) },
            { Vector2ToString(attributes.aaloc2), LoadPieceAttributes(newLocations[attributes.aaloc2].name) },
            { Vector2ToString(attributes.aeloc1), LoadPieceAttributes(newLocations[attributes.aeloc1].name) },
            { Vector2ToString(attributes.aeloc2), LoadPieceAttributes(newLocations[attributes.aeloc2].name) },
            { Vector2ToString(attributes.ahloc1), LoadPieceAttributes(newLocations[attributes.ahloc1].name) },
            { Vector2ToString(attributes.ahloc2), LoadPieceAttributes(newLocations[attributes.ahloc2].name) },
            { Vector2ToString(attributes.arloc1), LoadPieceAttributes(newLocations[attributes.arloc1].name) },
            { Vector2ToString(attributes.arloc2), LoadPieceAttributes(newLocations[attributes.arloc2].name) },
            { Vector2ToString(attributes.acloc1), LoadPieceAttributes(newLocations[attributes.acloc1].name) },
            { Vector2ToString(attributes.acloc2), LoadPieceAttributes(newLocations[attributes.acloc2].name) },
            { Vector2ToString(attributes.asloc1), LoadPieceAttributes(newLocations[attributes.asloc1].name) },
            { Vector2ToString(attributes.asloc2), LoadPieceAttributes(newLocations[attributes.asloc2].name) },
            { Vector2ToString(attributes.asloc3), LoadPieceAttributes(newLocations[attributes.asloc3].name) },
            { Vector2ToString(attributes.asloc4), LoadPieceAttributes(newLocations[attributes.asloc4].name) },
            { Vector2ToString(attributes.asloc5), LoadPieceAttributes(newLocations[attributes.asloc5].name) }
        };
        typeLocations = new Dictionary<string, List<Vector2>>
        {
            { "General", new List <Vector2>{ attributes.agloc } },
            { "Advisor", new List<Vector2>{ attributes.aaloc1, attributes.aaloc2} },
            { "Elephant", new List<Vector2> { attributes.aeloc1, attributes.aeloc2 } },
            { "Horse", new List<Vector2> { attributes.ahloc1, attributes.ahloc2 } },
            { "Chariot", new List<Vector2> { attributes.arloc1, attributes.arloc2 } },
            { "Cannon", new List<Vector2> { attributes.acloc1, attributes.acloc2 } },
            { "Soldier", new List<Vector2> { attributes.asloc1, attributes.asloc2, attributes.asloc3, attributes.asloc4, attributes.asloc5 } }
        };
        typeDict = new Dictionary<string, string>
        {
            { Vector2ToString(attributes.agloc), "General" },
            { Vector2ToString(attributes.aaloc1), "Advisor" },{ Vector2ToString(attributes.aaloc2), "Advisor" },
            { Vector2ToString(attributes.aeloc1), "Elephant" },{ Vector2ToString(attributes.aeloc2), "Elephant" },
            { Vector2ToString(attributes.ahloc1), "Horse" },{ Vector2ToString(attributes.ahloc2), "Horse" },
            { Vector2ToString(attributes.arloc1), "Chariot" },{ Vector2ToString(attributes.arloc2), "Chariot" },
            { Vector2ToString(attributes.acloc1), "Cannon" },{ Vector2ToString(attributes.acloc2), "Cannon" },
            { Vector2ToString(attributes.asloc1), "Soldier" },{ Vector2ToString(attributes.asloc2), "Soldier" },
            { Vector2ToString(attributes.asloc3), "Soldier" },{ Vector2ToString(attributes.asloc4), "Soldier" },{ Vector2ToString(attributes.asloc5), "Soldier" }
        };
    }

    private string Vector2ToString(Vector2 v) { return v.x.ToString() + v.y.ToString(); }
    private PieceAttributes LoadPieceAttributes(string pieceName)
    {
        if (pieceName.StartsWith("Standard ")) return standardAttributes[pieceName];
        return Resources.Load<PieceAttributes>("Pieces/Info/" + pieceName+"/Attributes");
    }

}
