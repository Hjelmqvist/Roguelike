using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopItem : Item
{
    Item _item;

    public override void Interact(Player player)
    {
        if (_item.price <= player.Gold)
        {
            player.ModifyGold(-_item.price);
            _item.Interact(player);
            Destroy(gameObject);
        }
    }

    public void SetItem(Item item)
    {
        _item = item;
    }
}

public class Item : MonoBehaviour
{
    public int price;
    public string Type;

    public virtual void Interact(Player player)
    {
        

        //if (price != 0)
        //{
        //    if (price < CurrentGold)
        //    {
                
        //    }
        //}
        //else
        //{
            
        //}
    }
}
