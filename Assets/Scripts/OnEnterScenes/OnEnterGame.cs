using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

public class OnEnterGame : MonoBehaviour, IPointerClickHandler
{
    public static bool gameover = false;

    public GameObject victoryImage, defeatImage, settingsPanel, yourTurnImage, youHaveMoved;
    public Transform tacticBag;
    public Button endTurn;
    public Text roundCount, timer;
    public Text playerName, playerWin, playerRank;
    public Text opponentName, opponentWin, opponentRank;
    public Text endTurnText;

    private Lineup lineup;
    private GameObject board;
    private Canvas parentCanvas;
    private List<Transform> tacticObjs = new List<Transform>();
    private BoardSetup boardSetup;
    private Dictionary<String, int> credits = new Dictionary<string, int>()
    {
        { "Chariot", 8 }, { "Horse",4}, {"Elephant",3},{"Advisor",2},{"General",10},{"Cannon",4},{"Soldier",2}
    };

    // Use this for initialization
    void Start () {
        lineup = InfoLoader.user.lineups[InfoLoader.user.lastLineupSelected];
        board = Instantiate(Resources.Load<GameObject>("Board/Info/" + lineup.boardName + "/Board"));
        board.transform.SetSiblingIndex(1);
        boardSetup = board.GetComponent<BoardSetup>();
        boardSetup.Setup(lineup, true);  // Set up Player Lineup
        boardSetup.Setup(new EnemyLineup(), false);  // Set up Enemy Lineup
        // Set up Player Info
        playerName.text = InfoLoader.user.username;
        playerWin.text = "Win%: "+InfoLoader.user.total.percentage.ToString();
        playerRank.text = "Rank: " + InfoLoader.user.rank.ToString();
        // Set up Opponent Info
        opponentName.text = "Opponent";
        opponentWin.text = "Win%: 80.00";
        opponentRank.text = "Rank: 9900";
        // SetupTactics
        for (int i = 0; i < LineupBuilder.tacticsLimit; i++)
        {
            tacticObjs.Add(tacticBag.Find(String.Format("TacticSlot{0}/Tactic", i)));
            tacticObjs[i].GetComponent<TacticInfo>().SetAttributes(FindTacticAttributes(lineup.tactics[i]));
        }
        StartCoroutine(Timer());
        parentCanvas = transform.Find("Canvas").GetComponent<Canvas>();
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
            settingsPanel.SetActive(true);
    }

    private TacticAttributes FindTacticAttributes(string tacticName)
    {
        return Resources.Load<TacticAttributes>("Tactics/Info/"+tacticName+"/Attributes");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (gameover)
        {
            gameover = false;
            SceneManager.LoadScene("PlayerMatching");
            GameInfo.Clear();
        }
    }

    private IEnumerator Timer()
    {
        while (true)
        {
            if (gameover) break;
            string seconds = ((int)(GameInfo.time % 60)).ToString();
            if (seconds.Length == 1) seconds = "0" + seconds;
            timer.text = ((int)(GameInfo.time / 60)).ToString() + ":" + seconds;
            yield return new WaitForSeconds(1.0f);
            if (--GameInfo.time < 0)
            {
                NextTurn();
            }
        }
    }

    public void Victory()
    {        
        victoryImage.SetActive(true);
        InfoLoader.user.total.Win();
        GameOver();
    }

    public void Defeat()
    {
        defeatImage.SetActive(true);
        InfoLoader.user.total.Lost();
        GameOver();
    }

    public void GameOver()
    {
        GameInfo.time = GameInfo.maxTime;
        gameover = true;
        if (settingsPanel.activeSelf) settingsPanel.SetActive(false);
        // CalculateNewRank(); // should be done by server
        // remove collections
    }

    public void Concede()
    {
        Defeat();
    }

    private void CalculateNewRank()
    {
        int credit = 0;
        foreach(Piece piece in GameInfo.activeAlly)
            credit += credits[piece.GetPieceType()];
        foreach (Piece piece in GameInfo.inactiveEnemy)
            credit += credits[piece.GetPieceType()];
    }

    public void EndTurn()
    {
        MovementController.PutDownPiece();
        endTurn.interactable = false;
        endTurnText.text = "Enemy Turn";
        NextTurn();
        // need to put down piece
        // Enemy turn;
        YourTurn();
    }

    public void YourTurn()
    {
        StartCoroutine(ShowYourTurn());
        endTurn.interactable = true;
        endTurnText.text = "End Turn";
        GameInfo.pieceMoved = false;
    }

    public void ShowMoved()
    {
        StartCoroutine(ShowYouHaveMoved());
    }

    private IEnumerator ShowYouHaveMoved()
    {
        MovementController.PutDownPiece();
        youHaveMoved.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        youHaveMoved.SetActive(false);
    }

    private IEnumerator ShowYourTurn()
    {
        yourTurnImage.SetActive(true);
        yield return new WaitForSeconds(1.0f);
        yourTurnImage.SetActive(false);
    }

    public void NextTurn()
    {
        roundCount.text = (++GameInfo.round).ToString();
        GameInfo.time = GameInfo.maxTime;
        GameInfo.pieceMoved = false;
        GameInfo.tacticUsed = false;
        GameInfo.abilityActivated = false;
    }
}
