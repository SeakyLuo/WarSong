using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceAFlag : TacticTrigger
{
    public override void Activate(Location location)
    {
        GameController.PlaceFlag(location, Login.playerID);
    }

    public override List<Location> ValidTargets()
    {
        return MovementController.Unoccupied();
    }

}
