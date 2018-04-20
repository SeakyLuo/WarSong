using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats {

    public int win = 0, lose = 0, draw = 0, percentage = 0;
    public Stats(int winCount, int loseCount, int drawCount)
    {
        win = winCount;
        lose = loseCount;
        draw = drawCount;
        SetPercentage();
    }

    public void Win()
    {
        win++;
        SetPercentage();
    }

    public void Lost()
    {
        lose++;
        SetPercentage();
    }

    public void Draw()
    {
        draw++;
        SetPercentage();
    }

    public void SetPercentage()
    {
        if (lose + draw == 0 && win != 0) percentage = 100;
        else percentage = win / (lose + draw) * 100;
    }
}
