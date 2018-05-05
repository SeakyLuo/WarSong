using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkSoldier : Trigger
{
    private bool gained = false;

    public override void Activate()
    {
        if (link) gained = true;
    }

    public override void Revenge()
    {
        if (!gained) return;
        GameInfo.Add(new Piece("Soldier", piece.GetCastle(), true));
    }

}
