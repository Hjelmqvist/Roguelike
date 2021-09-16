using System;

public class ShopItem : Item
{
   [NonSerialized] public Item _itemToSell;

    public override bool Interact(Player player)
    {
        if (_itemToSell.Price <= player.Gold)
        {
            player.ModifyGold(-_itemToSell.Price);
            _itemToSell.Interact(player); 
            Destroy(gameObject);
            return true;
        }
        return false;
    }
}