﻿using System.Collections.Generic;
using UnityEngine;

//[UnityEngine.CreateAssetMenu(fileName = "Trigger", menuName = "PieceTrigger")]
public class Trigger: ScriptableObject {

    public int effectiveRound = 1;
    public int limitedUse = -1; // -1 if unlimited
    public string passive = ""; // Can only be Tactic or Piece or ""
    public bool activatable = false;
    public bool bloodThirsty = false;
    public bool afterMove = false;
    public bool inEnemyRegion = false;
    public bool inEnemyPalace = false;
    public bool inEnemyCastle = false;
    public bool atEnemyBottom = false;
    public List<string> cantBeDestroyedBy = new List<string>();
    [HideInInspector] public bool silenced = false;
    [HideInInspector] public Piece piece;
    public delegate List<Vector2Int> ValidLocations(int x, int y, bool link = false);
    public ValidLocations validLocations;

    protected bool link = false;

    public virtual void StartOfGame() { }
    public virtual void Activate() { }  // Override this if NO targets required
    public virtual void Activate(Vector2Int location) { } // Override this if target Piece required
    public virtual void Revenge() { } // triggered when eliminated
    public virtual void BloodThirsty() { } // triggered when kills someone
    public virtual List<Vector2Int> ValidLocs(bool link = false)
    {
        if (validLocations == null) return MovementController.ValidLocs(piece.location.x, piece.location.y, piece.GetPieceType(), link);
        else return validLocations(piece.location.x, piece.location.y, link);
    }
    public virtual List<Vector2Int> ValidTargets() { return new List<Vector2Int>(); }  // Offers the location of targets
    public virtual List<Tactic> ValidTargets(int oreCost, int goldCost) {return OnEnterGame.gameInfo.unusedTactics[Login.playerID]; } // Offers target tactics
    public virtual void Passive(Tactic tactic) { } // For instance, your tacitics cost 1 Ore less
    public virtual void Passive(Piece piece) { } // For instance, your minion abilities cost 1 Ore less
    public virtual bool PassiveCriteria(Tactic tactic) { return false; }
    public virtual bool PassiveCriteria(Piece piece) { return false; }
    public virtual void StartOfTurn() { }
    public virtual void EndOfTurn() { }
    public virtual void AfterMove() { }
    public virtual void InEnemyRegion() { }
    public virtual void InEnemyPalace() { }
    public virtual void InEnemyCastle() { }
    public virtual void AtEnemyBottom() { }
    public virtual void EndOfGame() { }

    public bool ReceiveMesseage(string message)
    {
        if (message == "BloodThirsty") return bloodThirsty;
        if (message == "AfterMove") return afterMove;
        if (message == "InEnemyRegion") return inEnemyRegion;
        if (message == "InEnemyPalace") return inEnemyPalace;
        if (message == "InEnemyCastle") return inEnemyCastle;
        if (message == "AtEnemyBottom") return atEnemyBottom;
        return false;
    }
    public bool Link() { link = MovementController.IsLink(piece, ValidLocs(true)); return link; }
    public bool Activatable()
    {
        return limitedUse != 0 && (activatable || Link()); // fuck silence
    }

}