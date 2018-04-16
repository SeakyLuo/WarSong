using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class OnEnterPlayMatching : MonoBehaviour {

    public Text ranking;
    public Button launchWar;
    public GameObject launchWarText;

    public GameObject[] lineupObjects = new GameObject[LineupsManager.lineupsLimit];

    private void Start()
    {
        ranking.text = InfoLoader.user.ranking.ToString();
        int lineupsCount = InfoLoader.user.lineups.Count;
        for (int i = 0; i < LineupsManager.lineupsLimit; i++)
        {
            if (i < lineupsCount)
            {
                lineupObjects[i].GetComponentInChildren<Text>().text = InfoLoader.user.lineups[i].lineupName;
                // Assign Data
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

    public void Back()
    {
        SceneManager.LoadScene("Main");
    }

    public void EnterCollection()
    {
        SceneManager.LoadScene("Collections");
    }

    public void Match()
    {
        // Match With 
        LaunchWar();
    }

    public void LaunchWar()
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
                lineupObjects[InfoLoader.user.lastLineupSelected].GetComponent<Image>().sprite = lineupObjects[number].GetComponent<Button>().spriteState.disabledSprite;
        }
        InfoLoader.user.lastLineupSelected = number;
    }
}
