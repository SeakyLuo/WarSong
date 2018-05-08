﻿using System.Collections.Generic;
using UnityEngine;

public class SoldierRecruitment : TacticTrigger {

    public override void Activate(Vector2Int loc)
    {
        GameInfo.Add(new Piece("Soldier", loc, true));
        GameController.ChangeOre(-oreCost);
    }

    public override List<Vector2Int> ValidTarget()
    {
        List<Vector2Int> target = new List<Vector2Int>();
        foreach (Vector2Int loc in GameController.FindCastles("Soldier"))
            if (!GameInfo.board.ContainsKey(loc))
                target.Add(loc);
        return target;
    }
}
