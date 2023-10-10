using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : Collectable
{
    public Sprite emptyChest;
    public int coinsAmount = 5;

    protected override void OnCollect(){
        if (!collected){
            collected = true;
            GetComponent<SpriteRenderer>().sprite = emptyChest;
            GameManager.instance.coins += coinsAmount;
            GameManager.instance.ShowText("+" + coinsAmount + " coins!", 25, Color.yellow, transform.position, Vector3.up * 50, 2.5f );
        }
    }
}
