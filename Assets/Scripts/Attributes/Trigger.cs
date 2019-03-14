using System.Collections.Generic;
using UnityEngine;

//[UnityEngine.CreateAssetMenu(fileName = "Trigger", menuName = "PieceTrigger")]
public class Trigger: ScriptableObject {
    public static string BLOOD_THIRSTY = "BloodThirsty", 
                        AFTER_MOVE = "AfterMove", 
                        IN_ENEMY_REGION = "InEnemyRegion", 
                        IN_ENEMY_PALACE = "InEnemyPalace", 
                        IN_ENEMY_CASTLE = "InEnemyCastle", 
                        AT_ENEMY_BOTTOM = "AtEnemyBottom";

    public int effectiveRound = 1;
    public int limitedUse = -1; // -1 if unlimited
    public string passive = ""; // Can only be Tactic or Piece or ""
    public bool revenge = false;
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
    public delegate List<Location> ValidLocations(int x, int y, bool link = false);
    public ValidLocations validLocations;
    public delegate void AssignRevenge();
    public AssignRevenge activateRevenge;

    protected bool link = false;

    public virtual void StartOfGame() { }
    public virtual void Activate() { }  // Override this if NO targets required
    public virtual void Activate(Location location) { } // Override this if target Piece required
    public virtual void Revenge() { if (activateRevenge != null) activateRevenge(); } // triggered when eliminated
    public virtual void BloodThirsty() { } // triggered when kills someone
    public virtual List<Location> ValidLocs(bool link = false)
    {
        if (validLocations == null) return MovementController.ValidLocs(piece.location.x, piece.location.y, piece.GetPieceType(), link);
        else return validLocations(piece.location.x, piece.location.y, link);
    }
    public virtual List<Location> ValidTargets() { return new List<Location>(); }  // Offers the location of targets
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
    public virtual bool Activatable()
    {
        return limitedUse != 0 && (activatable || Link()); // fuck silence
    }

    public bool ReceiveMesseage(string message)
    {
        if (message == BLOOD_THIRSTY) return bloodThirsty;
        if (message == AFTER_MOVE) return afterMove;
        if (message == IN_ENEMY_REGION) return inEnemyRegion;
        if (message == IN_ENEMY_PALACE) return inEnemyPalace;
        if (message == IN_ENEMY_CASTLE) return inEnemyCastle;
        if (message == AT_ENEMY_BOTTOM) return atEnemyBottom;
        return false;
    }
    public bool Link() { link = MovementController.IsLink(piece, ValidLocs(true)); return link; }
}