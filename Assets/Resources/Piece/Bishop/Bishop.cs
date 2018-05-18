using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bishop : MonoBehaviour {

    private bool gained = false;

    public override void Activate()
    {
        if (link)
        {
            gained = true;
            limitedUse = 0;
        }
    }

    public override void Deploy()
    {
        if (!gained)
            return;
        GameController.Eliminate(Advisor);
    }
}
