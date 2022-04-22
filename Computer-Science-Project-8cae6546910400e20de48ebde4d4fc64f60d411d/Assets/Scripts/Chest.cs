using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Collectable
{
    public Sprite emptyChest;
    public int coinAmount;
    protected override void OnCollect()
    {
        if (!collected)
        {
            collected = true;
            GetComponent<SpriteRenderer>().sprite = emptyChest;
            coinAmount = Random.Range(1, 10);
            Debug.Log("Grant " + coinAmount + " coins!");
        }
    }
}
