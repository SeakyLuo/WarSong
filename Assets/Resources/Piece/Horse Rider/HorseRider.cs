﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorseRider : Trigger {

	public override void Activate (Vector2Int location)
	{
        //GameController.boardSetup.pieces[location].GetComponent<PieceInfo>().trigger.ValidLocs = MovementController.HorseLoc;
	}

	public override List<Vector2Int> ValidTargets ()
	{
		List<Vector2Int> targets = new List<Vector2Int> ();
		foreach (Piece piece in GameInfo.activePieces[InfoLoader.playerID])
            if (piece.GetPieceType() == "Soldier")
                targets.Add(piece.location);
        return targets;
	}
}