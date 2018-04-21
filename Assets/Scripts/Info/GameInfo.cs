﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameInfo
{
    public static Dictionary<Vector2Int, Piece> board = new Dictionary<Vector2Int, Piece>();
    public static Dictionary<Vector2Int, string> landmines = new Dictionary<Vector2Int, string>();
    public static List<string> tactics = new List<string>();
    public static List<string> usedTactics = new List<string>();
    public static List<Piece> activeAlly = new List<Piece>(),
                           inactiveAlly = new List<Piece>(),
                           activeEnemy = new List<Piece>(),
                           inactiveEnemy = new List<Piece>();

    public static string user = ""; //whose turn
    public static int round = 1,
                      time = 120,
                      maxTime = 120;

    public static void Add(Piece piece, Vector2Int loc, bool reactivate = false)
    {
        piece.SetActive(true);
        board.Add(loc, piece);
        if (piece.IsAlly())
        {
            activeAlly.Add(piece);
            if (reactivate) inactiveAlly.Remove(piece);
        }
        else
        {
            activeEnemy.Add(piece);
            if (reactivate) inactiveEnemy.Remove(piece);
        }
    }

    public static void Remove(Piece piece, Vector2Int loc)
    {
        piece.SetActive(false);
        board.Remove(loc);
        if (piece.IsAlly())
        {
            activeAlly.Remove(piece);
            inactiveAlly.Add(piece);
        }
        else
        {
            activeEnemy.Remove(piece);
            inactiveEnemy.Add(piece);
        }
    }

    public static void Clear()
    {
        board.Clear();
        tactics.Clear();
        usedTactics.Clear();
        activeAlly.Clear();
        inactiveAlly.Clear();
        activeEnemy.Clear();
        inactiveEnemy.Clear();
        round = 1;
    }
}