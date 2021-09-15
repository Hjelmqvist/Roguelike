using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem : Item
{
   [NonSerialized] public Item Item;

    public override bool Interact(Player player)
    {
        if (Item.price <= player.Gold)
        {
            player.ModifyGold(-Item.price);
            Item.Interact(player); 
            Destroy(gameObject);
            return true;
        }
        return false;
    }
}