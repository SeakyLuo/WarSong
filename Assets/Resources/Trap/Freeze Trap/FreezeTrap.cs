using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Trigger", menuName = "TacticTrigger")]
public class FreezeTrap : TacticTrigger
{
    public override void Activate(Vector2Int location)
    {
        GameController.FreezePiece(location, 2);
    }

}
