using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archbishop : MonoBehaviour {


    public override void Activate(Vector2Int location)
    {
        GameController.Eliminate(location);
    }

}
