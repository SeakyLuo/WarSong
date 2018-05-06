using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "Trigger", menuName = "Trigger")]
public class Trigger: ScriptableObject {

    public int afterRound = 0;
    public bool activatable = false;
    public int limitedUse = -1; // -1 if unlimited
    [HideInInspector] public bool silenced = false;
    [HideInInspector] public Piece piece;

    protected bool link = false;

    public Trigger()
    {
        afterRound = 0;
        activatable = false;
        limitedUse = -1;
        silenced = false;
        link = false;
    }

    public virtual void StartOfGame() { }
    public virtual void Activate() { }
    public virtual void Activate(Vector2Int loc) { }
    public virtual bool Link() { link = MovementController.IsLink(piece, ValidLoc(true)); return link; }
    public virtual void Revenge() { }
    public virtual void Bloodthirsty() { }
    public virtual void Sacrifice() { }
    public virtual void BeforeMove() { }
    public virtual void AfterMove() { }
    public virtual List<string> CantBeDestroyedBy() { return new List<string>(); }
    public virtual List<Vector2Int> ValidLoc(bool link = false) { return MovementController.ValidLoc(piece.location.x, piece.location.y, piece.GetPieceType(), link); }
    public virtual List<Vector2Int> ValidTarget() { return new List<Vector2Int>(); }
    public virtual void StartOfTurn() { }
    public virtual void EndOfTurn() { }
    public virtual void InAllyField() { }
    public virtual void InCastle() { }
    public virtual void InPalace() { }
    public virtual void AtBottom() { }
    public virtual void InEnemyRegion() { }
    public virtual void InEnemyPalace() { }
    public virtual void InEnemyCastle() { }
    public virtual void AtEnemyBottom() { }
    public virtual void EndOfGame() { }

    public bool Activatable()
    {
        return (activatable || limitedUse == 0 || Link()); // fuck silence
    }

}