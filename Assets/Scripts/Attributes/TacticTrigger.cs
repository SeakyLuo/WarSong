using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "Trigger", menuName = "TacticTrigger")]
public class TacticTrigger : ScriptableObject
{
    public int afterRound = 0;
    public int lastRound = 1; // Number of rounds this effect will last
    public int effectiveStarts = 0; // Effective round number. 0 if effective immediately, 
    [HideInInspector] public int oreCost;
    [HideInInspector] public int goldCost;

    public virtual void Activate() { }  // Override this if NO targets required
    public virtual void Activate(Vector2Int loc) { } // Override this if targets required
    public virtual List<Vector2Int> ValidTarget() { return new List<Vector2Int>(); } // Offers the location of targets

    public bool Activatable()
    {
        return afterRound == 0 || afterRound >= GameInfo.round;
    }
}
