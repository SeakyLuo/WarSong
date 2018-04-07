﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LineupsManager : MonoBehaviour {

    public GameObject createLineupButton, selectBoardPanel, collectionPanel, createLineupPanel;
    public Text myLineups;
    public static int lineupsLimit = 9;
    public UserInfo user;
    public GameObject[] lineupObjects = new GameObject[lineupsLimit];

    private int lineupsCount = 0;
    private int modifyLineup = -1;
    private BoardManager boardManager;
    private LineupBuilder lineupBuilder;

    // Use this for initialization
    void Start () {
        user = InfoLoader.user;
        lineupsCount = user.lineups.Count;
        if (lineupsCount == lineupsLimit) createLineupButton.SetActive(false);
        for (int i = 0; i < lineupsLimit; i++)
        {
            if (i < lineupsCount)
            {                
                lineupObjects[i].GetComponentInChildren<Text>().text = user.lineups[i].lineupName;
                // Assign Data
            }
            else lineupObjects[i].SetActive(false);
        }
        myLineups.text = "My Lineups\n" + lineupsCount.ToString() + "/9";
        boardManager = selectBoardPanel.GetComponent<BoardManager>();
        lineupBuilder = createLineupPanel.GetComponent<LineupBuilder>();
    }

    public void AddLineup(Lineup lineup)
    {
        if (modifyLineup == -1)
        {
            user.lineups.Add(lineup);
            lineupObjects[lineupsCount].SetActive(true);
            lineupObjects[lineupsCount++].GetComponentInChildren<Text>().text = lineup.lineupName;
            myLineups.text = "My Lineups\n" + lineupsCount.ToString() + "/9";
            if (lineupsCount == lineupsLimit) createLineupButton.SetActive(false);
            else createLineupButton.SetActive(true);
        }
        else
        {
            user.lineups[modifyLineup] = lineup;
            lineupObjects[modifyLineup].GetComponentInChildren<Text>().text = lineup.lineupName;
            modifyLineup = -1;
        }          
    }

    public void DeleteLineup()
    {
        createLineupButton.SetActive(true);
        if (modifyLineup == -1) modifyLineup = lineupsCount - 1;
        lineupObjects[--lineupsCount].SetActive(false);
        user.lineups.RemoveAt(modifyLineup);
        for (int i = modifyLineup; i < lineupsCount; i++)
            lineupObjects[i].GetComponentInChildren<Text>().text = user.lineups[i].lineupName;
        myLineups.text = "My Lineups\n" + lineupsCount.ToString() + "/9";
        modifyLineup = -1;        
    }

    public void CreateLineup()
    {
        createLineupButton.SetActive(false);
        selectBoardPanel.SetActive(true);
    }

    public void OpenLineup(int number)
    {
        modifyLineup = number;
        createLineupPanel.SetActive(true);
        boardManager.LoadBoard(user.lineups[number]);        
        lineupBuilder.SetLineup(user.lineups[number]);
    }

}
