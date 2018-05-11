using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Trigger", menuName = "TacticTrigger")]
public class PlaceATrap : TacticTrigger {
    public override void Activate(Vector2Int location)
    {
        GameController.PlaceTrap(location, Database.traps[Random.Range(0, Database.traps.Count)].Name, InfoLoader.user.playerID);
    }

    public override List<Vector2Int> ValidTargets()
    {
        return MovementController.Unoccupied();
    }

}
