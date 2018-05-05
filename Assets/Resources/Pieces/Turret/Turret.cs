﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret: Trigger {

    private void Awake()
    {
        activatable = true;
    }

    public override List<Vector2Int> ValidLoc(bool link = false)
    {
        if (silenced) return MovementController.ValidLoc(piece.GetLocation().x, piece.GetLocation().y, piece.GetPieceType());
        return new List<Vector2Int>();
    }

    public override List<Vector2Int> ValidTarget()
    {
        if (silenced) return new List<Vector2Int>();
        return MovementController.CannonTarget(piece.GetLocation().x, piece.GetLocation().y);
    }

    public override void Activate(Vector2Int loc)
    {
        GameController.Deactivate(loc);
        GameController.ChangeOre(-piece.GetOreCost());
    }
}