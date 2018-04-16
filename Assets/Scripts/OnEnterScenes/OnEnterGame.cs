using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnEnterGame : MonoBehaviour {

    private Lineup lineup;
    private GameObject board;

	// Use this for initialization
	void Start () {
        lineup = InfoLoader.user.lineups[InfoLoader.user.lastLineupSelected];
        board = Instantiate(Resources.Load<GameObject>("Board/Info/" + lineup.boardName + "/Board"));
        board.GetComponent<BoardSetup>().Setup(lineup);
	}
	
}
