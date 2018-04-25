﻿using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class OnEnterPlayMatching : MonoBehaviour
{
    public Text rank;
    public Button launchWar;
    public GameObject launchWarText, settingsPanel;
    public GameObject[] lineupObjects = new GameObject[LineupsManager.lineupsLimit];

    private GameObject[] xs = new GameObject[LineupsManager.lineupsLimit];

    private void Start()
    {
        rank.text = InfoLoader.user.rank.ToString();
        int lineupsCount = InfoLoader.user.lineups.Count;
        for (int i = 0; i < LineupsManager.lineupsLimit; i++)
        {
            xs[i] = lineupObjects[i].transform.Find("Unavailable").gameObject;
            if (i < lineupsCount)
            {
                lineupObjects[i].GetComponentInChildren<Text>().text = InfoLoader.user.lineups[i].lineupName;
                lineupObjects[i].GetComponent<Button>().interactable = InfoLoader.user.lineups[i].complete;
                xs[i].SetActive(!InfoLoader.user.lineups[i].complete);
            }
            else lineupObjects[i].SetActive(false);
        }
        if (InfoLoader.user.lastLineupSelected == -1)
        {
            launchWarText.SetActive(false);
            launchWar.interactable = false;
        }
        else
        {
            lineupObjects[InfoLoader.user.lastLineupSelected].GetComponent<Button>().Select();
        }
        SelectLineup(InfoLoader.user.lastLineupSelected);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
            settingsPanel.SetActive(!settingsPanel.activeSelf);
    }

    public void Back()
    {
        SceneManager.LoadScene("Main");
    }

    public void EnterCollection()
    {
        InfoLoader.switchSceneCaller = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene("Collection");
    }

    public void Match()
    {
        // Upload Lineup Info to the server and match according to the board
        LaunchWar();
    }

    private void LaunchWar()
    {
        SceneManager.LoadScene("GameMode");
    }

    public void SelectLineup(int number)
    {        
        if(number != -1)
        {
            if (!launchWar.interactable)
            {
                launchWarText.SetActive(true);
                launchWar.interactable = true;
            }
            lineupObjects[number].GetComponent<Image>().sprite = lineupObjects[number].GetComponent<Button>().spriteState.highlightedSprite;
            if(InfoLoader.user.lastLineupSelected != -1)
                lineupObjects[InfoLoader.user.lastLineupSelected].GetComponent<Image>().sprite = lineupObjects[InfoLoader.user.lastLineupSelected].GetComponent<Button>().spriteState.disabledSprite;
        }
        InfoLoader.user.lastLineupSelected = number;
    }
}
