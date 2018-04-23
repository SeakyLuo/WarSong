using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class OnEnterPlayMatching : MonoBehaviour, IPointerClickHandler
{
    public Text rank;
    public Button launchWar;
    public GameObject launchWarText, settingsPanel, optionsPanel;
    public GameObject[] lineupObjects = new GameObject[LineupsManager.lineupsLimit];

    private GameObject[] xs = new GameObject[LineupsManager.lineupsLimit];
    private GameObject[] closeObjects;
    private Canvas parentCanvas;

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
        closeObjects = new GameObject[] { optionsPanel, settingsPanel };
        parentCanvas = gameObject.GetComponent<Canvas>();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
            ShowSettings();
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

    public void ShowSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        foreach (GameObject close in closeObjects)
        {
            if (close.activeSelf)
            {
                Vector2 mousePosition = AdjustedMousePosition();
                Rect rect = close.GetComponent<RectTransform>().rect;
                // rect.x and rect.y are negative
                if (mousePosition.x < rect.x || mousePosition.x > -rect.x || mousePosition.y < rect.y || mousePosition.y > -rect.y)
                    close.SetActive(false);
                break;
            }
        }
    }

    private Vector2 AdjustedMousePosition()
    {
        Vector2 mousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, Input.mousePosition, parentCanvas.worldCamera, out mousePosition);
        return mousePosition;
    }
}
