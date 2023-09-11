using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    /* VARIABLES */
    public int deathCount;
    public int cash;
    public int gold;
    public bool blood;

    /* ITEMS */
    public bool keepItemState;
    public bool[] unlockedItem;
    public GameManager.Item ItemState;


    public GameData()
    {
        this.deathCount = 0;
        this.cash = 100;
        this.gold = 0;
        this.keepItemState = true;
        this.blood = false;

        this.ItemState = GameManager.Item.Empty;
        this.unlockedItem = new bool[12];
    }
}
