using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : Item
{
    public override bool Interact(Player player)
    {
        player.Heal();
        return true;
    }
}
