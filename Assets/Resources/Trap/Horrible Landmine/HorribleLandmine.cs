using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorribleLandmine : TacticTrigger {

    public override void Activate(Vector2Int Vector2Int)
    {
        if(OnEnterGame.gameInfo.Destroyable(Vector2Int, "Trap"))
            GameController.Eliminate(OnEnterGame.gameInfo.board[Vector2Int]);
    }
}
