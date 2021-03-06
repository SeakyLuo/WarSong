﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineupBuilder : MonoBehaviour {
    // Recommend Tactics || Random Tactics
    public static int current_tactics = 0;
    public static Lineup copy = new Lineup();

    public GameObject selectBoardPanel, copyReminder, fullReminder, sameTacticReminder;
    public Text tacticsCountText, totalOreCostText, totalGoldCostText;
    public InputField inputField;
    public CollectionManager collectionManager;
    public LineupsManager lineupsManager;

    private Lineup lineup;
    private Transform board, lineupBoard;
    private BoardInfo boardInfo;
    private GameObject[] tacticObjs;
    private List<TacticAttributes> tacticAttributes = new List<TacticAttributes>();    
    private int totalOreCost = 0, totalGoldCost = 0;

    private void Awake()
    {
        lineup = new Lineup();
        tacticObjs = GameObject.FindGameObjectsWithTag("Tactic");
        foreach (GameObject obj in tacticObjs) obj.SetActive(false);
        lineup.lineupName = "Custom Lineup";
    }

    private void OnEnable()
    {
        collectionManager.SetCardsPerPage(4);
        collectionManager.ShowCurrentPage();
        if (!copy.IsEmpty()) PasteLineup();
    }

    private void OnDisable()
    {
        collectionManager.SetCardsPerPage(8);
        collectionManager.ShowCurrentPage();
    }

    void Start()
    {
        board = transform.Find("BoardPanel/Board");
        lineupBoard = board.Find("LineupBoard(Clone)");
        boardInfo = lineupBoard.GetComponent<BoardInfo>();
    }

    public void AddPiece(CardInfo cardInfo, Location loc)
    {
        PieceAdder(cardInfo, loc);
    }

    public bool AddPiece(CardInfo cardInfo, Vector3 loc)
    {
        Location location = LineupBoardGestureHandler.FindLoc(loc);
        string cardType,
               locName = location.ToString();
        if (boardInfo.locationType.TryGetValue(locName, out cardType) && cardType == cardInfo.GetCardType())
        {
            PieceAdder(cardInfo, location);
            return true;
        }
        return false;
    }

    private void PieceAdder(CardInfo cardInfo, Location loc)
    {
        if (lineupBoard == null) lineupBoard = board.transform.Find("LineupBoard(Clone)");
        Collection newCollection = new Collection(cardInfo.piece, 1, cardInfo.GetHealth());
        lineup.cardLocations[loc] = newCollection;
        lineupBoard.Find(loc.ToString()).Find("CardImage").GetComponent<Image>().sprite = cardInfo.image.sprite;
        collectionManager.AddCollection(boardInfo.cardLocations[loc]);
        collectionManager.ShowCurrentPage();
        boardInfo.SetCard(newCollection, loc);        
    }

    public bool AddTactic(CardInfo cardInfo)
    {
        if (current_tactics == Lineup.tacticLimit)
        {
            StartCoroutine(FullReminder());
            return false;
        }
        else if (FindTactic(cardInfo.GetCardName()) != -1)
        {
            StartCoroutine(SameTacticReminder());
            return false;
        }
        TacticAdder(cardInfo.tactic);
        collectionManager.ShowCurrentPage();
        return true;
    }

    private void AddTactic(Tactic tactic)
    {
        // called by progrommer
        TacticAdder(Database.FindTacticAttributes(tactic.tacticName));
    }

    private void TacticAdder(TacticAttributes attributes)
    {
        totalOreCost += attributes.oreCost;
        totalGoldCost += attributes.goldCost;
        int index = 0;
        if (current_tactics == 0 || attributes < tacticAttributes[0]) index = 0;
        else if (attributes > tacticAttributes[current_tactics - 1]) index = current_tactics;
        else
        {
            for (int i = 0; i < current_tactics - 1; i++)
                if (attributes > tacticAttributes[i] && attributes < tacticAttributes[i + 1])
                {
                    index = i + 1;
                    break;
                }
        }
        lineup.tactics.Insert(index, new Tactic(attributes));
        tacticAttributes.Insert(index, attributes);
        tacticObjs[current_tactics++].SetActive(true);
        for (int i = index; i < current_tactics; i++)
            tacticObjs[i].GetComponent<TacticInfo>().SetAttributes(tacticAttributes[i]);
        SetTexts();
    }

    public void RemoveTactic(TacticAttributes attributes)
    {
        // called by user
        if (current_tactics == 0) return;
        TacticRemover(attributes);
        collectionManager.AddCollection(new Collection(attributes));
        if(collectionManager.currentPage.Key == "Tactic")
            collectionManager.ShowCurrentPage();
    }
    
    private void RemoveTactic(Tactic tactic)
    {
        TacticRemover(tacticAttributes[lineup.tactics.IndexOf(tactic)]);
    }

    private void TacticRemover(TacticAttributes attributes)
    {
        Tactic remove = new Tactic(attributes);
        int index = FindTactic(remove.tacticName);
        totalOreCost -= attributes.oreCost;
        totalGoldCost -= attributes.goldCost;
        if (current_tactics > 1)
        {
            for (int i = index; i < current_tactics - 1; i++)
                tacticObjs[i].GetComponent<TacticInfo>().SetAttributes(tacticAttributes[i + 1]);
        }
        else tacticObjs[0].GetComponent<TacticInfo>().Clear();
        lineup.tactics.RemoveAt(index);
        tacticAttributes.RemoveAt(index);                   
        tacticObjs[--current_tactics].SetActive(false);
        SetTexts();        
    }

    private int FindTactic(string tacticName)
    {
        for (int i = 0; i < lineup.tactics.Count; i++)
            if (lineup.tactics[i].tacticName == tacticName) return i;
        return -1;
    }

    private IEnumerator SameTacticReminder(float time = 1.5f)
    {
        sameTacticReminder.SetActive(true);
        yield return new WaitForSeconds(time);
        sameTacticReminder.SetActive(false);
    }

    private IEnumerator FullReminder(float time = 1.5f)
    {
        fullReminder.SetActive(true);
        yield return new WaitForSeconds(time);
        fullReminder.SetActive(false);
    }

    private void SetTexts()
    {
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
        collectionManager.ShowCurrentPage();
        gameObject.SetActive(false);
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
        lineup.boardName = boardInfo.attributes.Name;
        // Incomplete Reminder
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
            foreach (KeyValuePair<Location, Collection> pair in lineup.cardLocations)
                collectionManager.AddCollection(pair.Value);
            foreach (Tactic tactic in new List<Tactic>(lineup.tactics))
            {
                RemoveTactic(tactic);
                collectionManager.AddCollection(new Collection(tactic));
            }
            collectionManager.ShowCurrentPage();
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
        StartCoroutine(CopyReminder());
    }

    private IEnumerator CopyReminder()
    {
        copyReminder.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        copyReminder.SetActive(false);
    }

    public void PasteLineup()
    {
        SetLineup(copy, true);
    }

    public void SetLineup(Lineup newLineup, bool isCopy = false)
    {
        ResetLineup(isCopy);
        lineup.lineupName = newLineup.lineupName;
        inputField.text = newLineup.lineupName;
        lineup.boardName = newLineup.boardName;
        if (isCopy)
        {
            foreach (Tactic tactic in newLineup.tactics)
            {
                if (collectionManager.RemoveCollection(new Collection(tactic)))
                    AddTactic(tactic);
                // can add virtual card like hearthstore
            }
            foreach (KeyValuePair<Location, Collection> pair in newLineup.cardLocations)
            {
                Collection collection = pair.Value;
                if (!collectionManager.RemoveCollection(collection) || 
                    !collectionManager.RemoveCollection(new Collection(collection.name,collection.type))) //find card with the same name
                {
                    Collection standardCollection = Collection.StandardCollection(collection.type);
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
            foreach (Tactic tactic in newLineup.tactics) AddTactic(tactic);
            boardInfo.cardLocations = newLineup.cardLocations;
            lineup.cardLocations = newLineup.cardLocations;
        }
        boardInfo.SetAttributes(newLineup.boardName, boardInfo.cardLocations);
        SetTexts();        
    }

    public void SetBoardInfo(BoardInfo info) { boardInfo = info; }
}
