using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class OnEnterGame : MonoBehaviour {

    public static bool gameover = false;
    public GameObject gameoverImage;
    public Transform tacticBag;
    public Text roundCount, timer;
    public Text playerName, playerWin, playerRank;
    public Text opponentName, opponentWin, opponentRank;

    private Lineup lineup;
    private GameObject board;
    private List<Transform> tacticObjs = new List<Transform>();
    private BoardSetup boardSetup;

    // Use this for initialization
    void Start () {
        lineup = InfoLoader.user.lineups[InfoLoader.user.lastLineupSelected];
        board = Instantiate(Resources.Load<GameObject>("Board/Info/" + lineup.boardName + "/Board"));
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
    }

    private TacticAttributes FindTacticAttributes(string tacticName)
    {
        return Resources.Load<TacticAttributes>("Tactics/Info/"+tacticName+"/Attributes");
    }

    private void Update()
    {
        if (gameover)
        {
            if (Input.GetMouseButtonUp(0))
            {
                SceneManager.LoadScene("PlayerMatching");
                GameInfo.Clear();
            }
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

    public void Gameover()
    {
        gameover = true;
        gameoverImage.SetActive(true);
        InfoLoader.user.total.Win();
        // remove collections
    }

    public void NextTurn()
    {
        roundCount.text = (++GameInfo.round).ToString();
        GameInfo.time = GameInfo.maxTime;
    }
}
