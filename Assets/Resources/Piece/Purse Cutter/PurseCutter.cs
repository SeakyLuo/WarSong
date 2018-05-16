using UnityEngine;
[CreateAssetMenu(fileName = "Trigger", menuName = "PieceTrigger")]

public class PurseCutter : Trigger {

    public override void InEnemyCastle()
    {
        GameController.ChangeCoin(1);
    }
}
