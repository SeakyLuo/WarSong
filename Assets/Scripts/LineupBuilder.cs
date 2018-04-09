﻿using System;
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
        boardInfo = boardObject.GetComponent<BoardInfo>();
        tacticObjs = GameObject.FindGameObjectsWithTag("Tactic");
        foreach (GameObject obj in tacticObjs) obj.SetActive(false);
        lineup.lineupName = "Custom Lineup";        
    }

    public void AddPiece(CardInfo cardInfo, Vector2 loc)
    {
        PieceAdder(cardInfo, loc, (int)loc.x, (int)loc.y);
    }

    public void AddPiece(CardInfo cardInfo, Vector3 loc)
    {
        int x = (int)Mathf.Floor((loc.x - 380) / 100);
        int y = (int)Mathf.Floor((loc.y - 10) / 100);
        string cardType,
               locName = x.ToString() + y.ToString();
        if (boardInfo.locationType.TryGetValue(locName, out cardType) && cardType == cardInfo.GetCardType())
        {
            PieceAdder(cardInfo, new Vector2(x, y), x, y);
        }
    }

    private void PieceAdder(CardInfo cardInfo, Vector2 loc, int locx, int locy)
    {
        string locName = locx.ToString() + locy.ToString();
        if (boardObject == null) boardObject = board.transform.Find("BoardObject(Clone)/");
        Collection newCollection = new Collection(cardInfo.piece, 1, cardInfo.GetHealth());
        lineup.cardLocations[loc] = newCollection;
        boardObject.Find(locName).Find("CardImage").GetComponent<Image>().sprite = cardInfo.image.sprite;
        collectionManager.RemoveCollection(newCollection);
        // Bug with next line, count is not 1
        collectionManager.AddCollection(boardInfo.cardLocations[loc]);
        boardInfo.SetCard(newCollection, loc);        
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
        collectionManager.RemoveCollection(new Collection(cardInfo.GetCardName()));
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
        {
            for (int i = 1; i < current_tactics; i++)
            {
                if (GreaterThan(attributes, tacticAttributes[i]) && LessThan(attributes, tacticAttributes[i + 1]))
                {
                    index = i;
                    break;
                }
            }
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

    private void ResumeCollections()
    {
        collectionManager.RemoveStandardCards();
        collectionManager.SetCardsPerPage(8);
        createLineupPanel.SetActive(false);
        selectBoardPanel.GetComponent<BoardManager>().DestroyBoard();
    }

    public void DeleteLineup()
    {
        lineupsManager.DeleteLineup();
        // upload to the server
        ResetLineup(true);
        ResumeCollections();
    }

    public void SaveLineup()
    {
        lineup.cardLocations = boardInfo.cardLocations;
        lineup.boardName = boardInfo.attributes.boardName;
        lineupsManager.AddLineup(lineup);
        // upload to the server
        ResetLineup();
        ResumeCollections();
    }

    public void ResetLineup(bool returnCards = false)
    {
        if (returnCards)
        {
            // return cards
            foreach (KeyValuePair<Vector2, Collection> pair in lineup.cardLocations)
                collectionManager.AddCollection(pair.Value);
            foreach (string tactic in lineup.tactics)
                collectionManager.AddCollection(new Collection(tactic));
            collectionManager.RemoveStandardCards();
        }
        lineup = new Lineup();
        inputField.text = "Custom Lineup";
        current_tactics = totalOreCost = totalGoldCost = 0;
        tacticAttributes.Clear();
        foreach (GameObject obj in tacticObjs) obj.SetActive(false);
        SetTexts();
        //boardInfo.Reset();
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

    public void PasteLineup()
    {
        SetLineup(copy, true);
    }

    public void SetLineup(Lineup newLineup, bool copy = false)
    {
        ResetLineup(copy);
        lineup.lineupName = newLineup.lineupName;
        inputField.text = newLineup.lineupName;
        lineup.boardName = newLineup.boardName;
        if (copy)
        {
            foreach (string tactic in newLineup.tactics)
            {
                if (collectionManager.RemoveCollection(new Collection(tactic)))
                {
                    AddTactic(tactic);
                }
            }
            foreach (KeyValuePair<Vector2, Collection> pair in newLineup.cardLocations)
            {
                Collection collection = pair.Value;
                if (!collectionManager.RemoveCollection(collection))
                {
                    // Can try find card with the same name
                    Collection standardCollection = new Collection("Standard " + collection.type, collection.type);
                    boardInfo.cardLocations[pair.Key] = standardCollection;
                    lineup.cardLocations[pair.Key] = standardCollection;
                }
                else
                {
                    boardInfo.cardLocations[pair.Key] = newLineup.cardLocations[pair.Key];
                    lineup.cardLocations[pair.Key] = newLineup.cardLocations[pair.Key];
                }
            }
        }
        else
        {
            foreach (string tactic in newLineup.tactics) AddTactic(tactic);
            boardInfo.cardLocations = newLineup.cardLocations;
            lineup.cardLocations = newLineup.cardLocations;
        }
        boardInfo.SetAttributes(newLineup.boardName, boardInfo.cardLocations);
        SetTexts();        
    }

    public void ReturnToBoardSelection()
    {
        selectBoardPanel.SetActive(true);
        createLineupPanel.SetActive(false);
    }

    public void SetBoardInfo(BoardInfo info) { boardInfo = info; }

    private string Vector2ToString(Vector2 v) { return v.x.ToString() + v.y.ToString(); }
}
