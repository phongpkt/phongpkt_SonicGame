using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighscoreEntryBase
{
    public string currentTime;
    public int gold;

    public HighscoreEntryBase(string currentTime, int coin)
    {
        this.currentTime = currentTime;
        this.gold = coin;
    }
}
