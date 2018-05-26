using UnityEngine;

public class FrightenBomb : TacticTrigger {

    public override void Activate(Vector2Int Vector2Int)
    {
        GameController.ChangePieceHealth(Vector2Int, -1);
    }
}
