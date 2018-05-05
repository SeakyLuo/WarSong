using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FatSoldier : Trigger
{
	public override void EndOfGame()
    {
        if (piece.IsActive())
            InfoLoader.user.coins += 1;
    }
}
