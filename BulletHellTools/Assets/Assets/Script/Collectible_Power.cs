using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible_Power : Collectible
{
    protected int powerValue = 1;
    
    protected override void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            GameManager.Instance.currentPower += powerValue;
        }

        base.OnTriggerEnter2D(coll);
    }
}
