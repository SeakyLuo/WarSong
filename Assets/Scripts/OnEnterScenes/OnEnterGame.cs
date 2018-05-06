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

    public GameObject victoryImage, defeatImage, drawImage, settingsPanel, yourTurnImage;
    public GameObject pathDot, targetDot, oldLocation;
    public Transform tacticBag;
    public Text roundCount, timer, modeName;
    public Text playerName, playerWin, playerRank;
    public Text opponentName, opponentWin, opponentRank;
    public Text oreText, coinText;
    [HideInInspector] public GameObject board;
    [HideInInspector] public BoardSetup boardSetup;

    private Lineup lineup;
    private List<Transform> tacticObjs = new List<Transform>();
    private Dictionary<String, int> credits = new Dictionary<string, int>()
    {
        { "Chariot", 8 }, { "Horse", 4}, {"Elephant", 3}, {"Advisor", 2}, {"General", 10}, {"Cannon", 4}, {"Soldier", 2}
    };

    // Use this for initialization
    void Start () {
        lineup = InfoLoader.user.lineups[InfoLoader.user.lastLineupSelected];
        board = Instantiate(Resources.Load<GameObject>("Board/" + lineup.boardName + "/Board"));
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
        modeName.text = InfoLoader.user.lastModeSelected;
        GameInfo.SetOrder(InfoLoader.user.playerID, 100000000);
        GameInfo.SetGameID(1);
        foreach (KeyValuePair<Vector2Int, GameObject> pair in boardSetup.pieces)
        {
            Trigger trigger = pair.Value.GetComponent<PieceInfo>().trigger;
            if (trigger != null) trigger.StartOfGame();
        }
        SetOreText();
        SetCoinText();
        StartCoroutine(Timer());
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            settingsPanel.SetActive(true);
            MovementController.PutDownPiece();
        }
    }

    private TacticAttributes FindTacticAttributes(string tacticName)
    {
        return Resources.Load<TacticAttributes>("Tactics/"+tacticName+"/Attributes");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (gameover)
        {
            gameover = false;
            SceneManager.LoadScene("PlayerMatching");
            GameInfo.Clear();
            Destroy(board);
        }
    }

    private IEnumerator Timer()
    {
        while (true)
        {
            if (gameover) break;
            string seconds = (GameInfo.time % 60).ToString();
            if (seconds.Length == 1) seconds = "0" + seconds;
            timer.text = (GameInfo.time / 60).ToString() + ":" + seconds;
            if (GameInfo.time < 15) timer.color = Color.red;
            else timer.color = Color.white;
            yield return new WaitForSeconds(1.0f);
            if (--GameInfo.time < 0) NextTurn();
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

    public void Draw()
    {
        drawImage.SetActive(true);
        InfoLoader.user.total.Draw();
        GameOver();
    }

    public void GameOver()
    {
        GameInfo.time = GameInfo.maxTime;
        gameover = true;
        if (settingsPanel.activeSelf) settingsPanel.SetActive(false);
        foreach(KeyValuePair<Vector2Int,GameObject> pair in boardSetup.pieces)
        {
            Trigger trigger = pair.Value.GetComponent<PieceInfo>().trigger;
            if (trigger != null) trigger.EndOfGame();
        }
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
        foreach(Piece piece in GameInfo.activeAllies)
            credit += credits[piece.GetPieceType()];
        foreach (Piece piece in GameInfo.inactiveEnemies)
            credit += credits[piece.GetPieceType()];
    }

    public void YourTurn()
    {
        //StartCoroutine(ShowYourTurn());
        GameInfo.actionTaken = false;
        foreach (KeyValuePair<Vector2Int, GameObject> pair in boardSetup.pieces)
        {
            Trigger trigger = pair.Value.GetComponent<PieceInfo>().trigger;
            if (trigger != null) trigger.StartOfTurn();
        }
    }

    private IEnumerator ShowYourTurn()
    {
        yourTurnImage.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        yourTurnImage.SetActive(false);
    }

    public void NextTurn()
    {
        foreach (KeyValuePair<Vector2Int, GameObject> pair in boardSetup.pieces)
        {
            Trigger trigger = pair.Value.GetComponent<PieceInfo>().trigger;
            if (trigger != null) trigger.EndOfTurn();
        }
        roundCount.text = (++GameInfo.round).ToString();
        if(GameInfo.round == 150)
        {
            Draw();
            return;
        }
        GameInfo.time = GameInfo.maxTime;
        GameInfo.actionTaken = true;
        YourTurn();
    }

    public void SetOreText()
    {
        oreText.text = GameInfo.ores[InfoLoader.user.playerID].ToString();
    }

    public void SetCoinText()
    {
        coinText.text = InfoLoader.user.coins.ToString();
    }
}
