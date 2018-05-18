using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DreadfulHorse : MonoBehaviour {

    public override void Activate(Vector2Int location)
    {
        GameController.Eliminate(location);
    }
}
