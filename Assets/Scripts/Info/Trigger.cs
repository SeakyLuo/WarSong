using System.Collections;
public class Trigger {

    public virtual void StartOfTheGame() { }
    public virtual void Link() { }
    public virtual void Revenge() { }
    public virtual void Bloodthirsty() { }
    public virtual void Destroyed() { }
    public virtual void Sacrifice() { }
    public virtual void BeforeMove() { }
    public virtual void AfterMove() { }
    public virtual void WhenPlayed() { }
    public virtual void CantBeDestroyed() { }
    public virtual void PossibleMovements() { }
    public virtual void StartOfTheTurn() { }
    public virtual void EndofTheTurn() { }
    public virtual void StartofTheNextRound() { }
    public virtual void AfterTurn(int turn) { }
    public virtual void InSelfRegion() { }
    public virtual void InSelfPalace() { }
    public virtual void AtSelfBottom() { }
    public virtual void EnterEnemyRegion() { }
    public virtual void EnterEnemyPalace() { }
    public virtual void EnterEnemyBottom() { }
    public virtual void EndOfTheGame() { }

}