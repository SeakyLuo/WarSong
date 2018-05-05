using System.Collections.Generic;
using System.IO;
using UnityEngine;

[CreateAssetMenu(fileName = "Trigger", menuName = "Trigger")]
public class Trigger: ScriptableObject {

    public int afterTurn = 0;
    public bool link = false;
    public bool activatable = false;
    public bool limited = false;
    public bool silenced = false;
    [HideInInspector] public Piece piece;

    public Trigger()
    {
        afterTurn = 0;
        link = false;
        activatable = false;
        limited = false;
        silenced = false;
    }

    public virtual void StartOfGame() { }
    public virtual void Activate() { }
    public virtual void Activate(Vector2Int loc) { }
    public virtual void Link() { link = MovementController.IsLink(piece, ValidLoc(true)); }
    public virtual void Revenge() { }
    public virtual void Bloodthirsty() { }
    public virtual void Sacrifice() { }
    public virtual void BeforeMove() { }
    public virtual void AfterMove() { }
    public virtual string CantBeDestroyedBy() { return ""; }
    public virtual List<Vector2Int> ValidLoc(bool link = false) { return MovementController.ValidLoc(piece.GetLocation().x, piece.GetLocation().y, piece.GetPieceType(), link); }
    public virtual List<Vector2Int> ValidTarget() { return new List<Vector2Int>(); }
    public virtual void StartOfTurn() { }
    public virtual void EndOfTurn() { }
    public virtual void InAllyField() { }
    public virtual void InPalace() { }
    public virtual void AtBottom() { }
    public virtual void InEnemyRegion() { }
    public virtual void InEnemyPalace() { }
    public virtual void AtEnemyBottom() { }
    public virtual void EndOfGame() { }

    public bool Activatable()
    {
        return (link || activatable || !limited); // fuck silence
    }

}