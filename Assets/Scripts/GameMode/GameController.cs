﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

    public static OnEnterGame onEnterGame;
    private static BoardSetup boardSetup;

    // Use this for initialization
    void Start () {
        onEnterGame = GameObject.Find("UIPanel").GetComponent<OnEnterGame>();
        boardSetup = onEnterGame.boardSetup;
    }

    public static void Reactivate(Piece piece)
    {
        boardSetup.pieces[piece.GetCastle()].SetActive(true);
        GameInfo.Add(piece, true);
    }

    public static void Deactivate(Piece piece)
    {
        Vector2Int loc = piece.GetCastle();
        boardSetup.pieces[loc].SetActive(false);
        GameInfo.Remove(piece, loc);
    }

    public static void Deactivate(Vector2Int loc)
    {
        Vector2Int castle = GameInfo.board[loc].GetCastle();
        boardSetup.pieces[castle].SetActive(false);
        GameInfo.Remove(GameInfo.board[loc], loc);
    }

    public static void ChangeOre(int deltaAmount)
    {
        GameInfo.ores[InfoLoader.user.playerID] += deltaAmount;
        onEnterGame.SetOreText();
    }

    public static void ChangeCoin(int deltaAmount)
    {
        InfoLoader.user.coins += deltaAmount;
        onEnterGame.SetCoinText();
    }
}
