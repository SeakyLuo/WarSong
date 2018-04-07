using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineupBuilder : MonoBehaviour {
    // Recommend Tactics || Random Tactics

    public GameObject collections, selectBoardPanel, createLineupPanel, myLineup, copyReminder;
    public Text tacticsCountText, totalOreCostText, totalGoldCostText;
    public InputField inputField;

    private Lineup lineup = new Lineup();
    private static Lineup copy = new Lineup();
    private CollectionManager collectionManager;
    private LineupsManager lineupsManager;
    private Transform board, boardObject;
    private BoardInfo boardInfo;
    private GameObject[] tacticObjs;
    private List<TacticAttributes> tacticAttributes = new List<TacticAttributes>();
    private static int tacticsLimit = 10;
    private int current_tactics = 0, totalOreCost = 0, totalGoldCost = 0;
    private Vector3 mousePosition;

    // Use this for initialization
    void Start () {
        collectionManager = collections.GetComponent<CollectionManager>();
        lineupsManager = myLineup.GetComponent<LineupsManager>();
        board = gameObject.transform.Find("BoardPanel/Board");
        boardObject = board.Find("BoardObject(Clone)/");
        boardInfo = board.GetComponent<BoardInfo>();
        tacticObjs = GameObject.FindGameObjectsWithTag("Tactic");
        foreach (GameObject obj in tacticObjs) obj.SetActive(false);
        lineup.lineupName = "Custom Lineup";        
    }

    public void AddPiece(CardInfo cardInfo, Vector2 loc)
    {
        string cardType,
               locName = loc.x.ToString() + loc.y.ToString();
        if (boardInfo.typeDict.TryGetValue(locName, out cardType) && cardType == cardInfo.GetCardType())
        {
            PieceAdder(cardInfo,(int) loc.x, (int)loc.y);
        }
            
    }

    public void AddPiece(CardInfo cardInfo, Vector3 loc)
    {
        int x = (int)Mathf.Floor((loc.x - 380) / 100);
        int y = (int)Mathf.Floor((loc.y - 10) / 100);
        AddPiece(cardInfo, new Vector2(x, y));
    }

    public void AddPiece(CardInfo cardInfo)
    {
        string cardName = cardInfo.GetCardName();
        foreach (Vector2 loc in boardInfo.typeLocations[cardInfo.GetCardType()])
        {
            string oldLocPieceName = boardInfo.locations[loc];
            if ((cardName.StartsWith("Standard ") && !oldLocPieceName.StartsWith("Standard ")) ||
                (!cardName.StartsWith("Standard ") && oldLocPieceName.StartsWith("Standard ")) ||
                (cardInfo.GetCardType() == "General" && cardName != oldLocPieceName ))
            {
                PieceAdder(cardInfo, (int)loc.x, (int)loc.y);
                break;
            }
        }
    }

    private void PieceAdder(CardInfo cardInfo, int locx, int locy)
    {
        string locName = locx.ToString() + locy.ToString();
        Vector2 loc = new Vector2(locx, locy);
        if (boardObject == null) boardObject = board.transform.Find("BoardObject(Clone)/");
        boardObject.Find(locName).Find("CardImage").GetComponent<Image>().sprite = cardInfo.image.sprite;        
        collectionManager.RemoveCollection(new Collection(cardInfo.piece,1,cardInfo.GetHealth()));
        collectionManager.AddCollection(boardInfo.cardLocations[loc]);
        boardInfo.SetCard(new Collection(cardInfo.piece, 1, cardInfo.GetHealth()), loc);        
    }

    public void AddTactic(CardInfo cardInfo)
    {
        if (current_tactics == tacticsLimit) return;
        else if (InTactics(cardInfo.GetCardName()))
        {
            // show animation;
            return;
        }
        TacticAdder(cardInfo.tactic);
        collectionManager.RemoveCollection(new Collection(cardInfo.GetCardName(),"Tactic",1,cardInfo.GetHealth()));
    }

    private void AddTactic(string TacticName)
    {
        // called by progrommer
        TacticAdder(Resources.Load<TacticAttributes>("Tactics/Info/" + TacticName + "/Attributes"));
    }

    private void TacticAdder(TacticAttributes attributes)
    {
        int index = 0;
        totalOreCost += attributes.oreCost;
        totalGoldCost += attributes.goldCost;
        if (current_tactics == 0 || LessThan(attributes, tacticAttributes[0])) index = 0;
        else if (GreaterThan(attributes, tacticAttributes[current_tactics - 1])) index = current_tactics;
        else
            for (int i = 1; i < current_tactics; i++)
                if (GreaterThan(attributes, tacticAttributes[i]) && LessThan(attributes, tacticAttributes[i + 1]))
                {
                    index = i;
                    break;
                }
        for (int i = index; i < current_tactics; i++)
            tacticObjs[i + 1].GetComponent<TacticInfo>().SetAttributes(tacticAttributes[i]);
        tacticObjs[index].GetComponent<TacticInfo>().SetAttributes(attributes);
        lineup.tactics.Insert(index, attributes.Name);
        tacticAttributes.Insert(index, attributes);
        tacticObjs[current_tactics++].SetActive(true);
        SetTexts();
    }

    public void RemoveTactic(TacticAttributes attributes)
    {
        // called by user
        if (current_tactics == 0) return;
        TacticRemover(attributes);
        collectionManager.AddCollection(new Collection(attributes.Name, "Tactic"));
    }
    
    private void RemoveTactic(string TacticName)
    {
        TacticRemover(tacticAttributes[lineup.tactics.IndexOf(TacticName)]);
    }

    private void TacticRemover(TacticAttributes attributes)
    {
        int index = lineup.tactics.IndexOf(attributes.Name);
        totalOreCost -= attributes.oreCost;
        totalGoldCost -= attributes.goldCost;
        if (current_tactics > 1)
        {
            for (int i = index; i < current_tactics - 1; i++)
                tacticObjs[i].GetComponent<TacticInfo>().SetAttributes(tacticAttributes[i + 1]);
        }
        else tacticObjs[0].GetComponent<TacticInfo>().Clear();
        lineup.tactics.Remove(attributes.Name);
        tacticAttributes.RemoveAt(index);                   
        tacticObjs[--current_tactics].SetActive(false);
        SetTexts();        
    }

    private bool LessThan(TacticAttributes attributes1, TacticAttributes attributes2)
    {
        return attributes1.oreCost < attributes2.oreCost ||
            (attributes1.oreCost == attributes2.oreCost && attributes1.goldCost < attributes2.goldCost) ||
            (attributes1.oreCost == attributes2.oreCost && attributes1.goldCost == attributes2.goldCost && attributes1.Name.CompareTo(attributes2.Name) < 0);
    }

    private bool GreaterThan(TacticAttributes attributes1, TacticAttributes attributes2)
    {
        // Because can't be the same.
        return !LessThan(attributes1, attributes2);
    }

    private bool InTactics(string attributes)
    {
        foreach (string name in lineup.tactics) if (name == attributes) return true;
        return false;
    }

    private void SetTexts()
    {
        //if (totalOreCost > 30) totalOreCostText.color = Color.red;
        //else totalOreCostText.color = Color.white;
        totalOreCostText.text = totalOreCost.ToString();
        totalGoldCostText.text = totalGoldCost.ToString();
        tacticsCountText.text = "Tactics\n" + lineup.tactics.Count.ToString() + "/10";
    }

    public void RenameLineup()
    {
        lineup.lineupName = inputField.text;
    }

    public void DeleteLineup()
    {
        // return cards
        foreach (KeyValuePair<Vector2, Collection> pair in lineup.cardLocations)
            collectionManager.AddCollection(pair.Value);
        foreach (string tactic in lineup.tactics)
            collectionManager.AddCollection(new Collection(tactic));
        ResetLineup();
        // delete from mylineups
        createLineupPanel.SetActive(false);
        lineupsManager.DeleteLineup();
        selectBoardPanel.GetComponent<BoardManager>().DestroyBoard();
    }

    public void SaveLineup()
    {
        lineup.cardLocations = boardInfo.cardLocations;
        lineup.boardName = boardInfo.attributes.boardName;
        lineupsManager.AddLineup(lineup);
        // upload to the server
        ResetLineup();
        collectionManager.SetCardsPerPage(8);
        collectionManager.RemoveStandardCards();
        createLineupPanel.SetActive(false);
        selectBoardPanel.GetComponent<BoardManager>().DestroyBoard();
    }

    public void CopyLineup()
    {
        copy = lineup;        
        CopyReminder(2);
    }

    IEnumerator CopyReminder(float delay)
    {
        copyReminder.SetActive(true);
        yield return new WaitForSeconds(delay);
        copyReminder.SetActive(false);
    }

    public void PasteLineup() { SetLineup(copy); }

    public void SetLineup(Lineup newLineup)
    {
        ResetLineup();
        inputField.text = newLineup.lineupName;
        foreach (string tactic in newLineup.tactics) AddTactic(tactic);
        boardInfo.cardLocations = newLineup.cardLocations;
        boardInfo.SetAttributes(newLineup.boardName, newLineup.cardLocations);
        SetTexts();
        lineup = newLineup;
    }

    public void ResetLineup()
    {
        lineup = new Lineup();
        inputField.text = "Custom Lineup";
        current_tactics = totalOreCost = totalGoldCost = 0;
        tacticAttributes.Clear();
        foreach (GameObject obj in tacticObjs) obj.SetActive(false);
        SetTexts();
        boardInfo.Reset();
    }

    public void ReturnToBoardSelection()
    {
        selectBoardPanel.SetActive(true);
        createLineupPanel.SetActive(false);
    }

    private string Vector2ToString(Vector2 v) { return v.x.ToString() + v.y.ToString(); }
}
