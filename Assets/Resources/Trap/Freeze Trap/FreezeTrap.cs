using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeTrap : TacticTrigger
{
    public override void Activate(Vector2Int Vector2Int)
    {
        GameController.FreezePiece(Vector2Int, 3);
    }
}
