﻿using System.Collections.Generic;
using UnityEngine;

public class BuildACannon : TacticTrigger
{
    public override void Activate(Vector2Int loc)
    {
        GameController.AddPiece(Collection.Cannon, loc, Login.playerID);
    }

    public override List<Vector2Int> ValidTargets()
    {
        List<Vector2Int> target = new List<Vector2Int>();
        foreach (Vector2Int loc in GameController.FindCastles("Cannon"))
            if (!OnEnterGame.gameInfo.board.ContainsKey(loc))
                target.Add(loc);
        return target;
    }
}
