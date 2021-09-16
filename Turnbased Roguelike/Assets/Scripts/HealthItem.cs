using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : Item
{
    [SerializeField] int healAmount = 200;

    public override bool Interact(Player player)
    {
        player.Health.ModifyHealth(healAmount);
        return true;
    }
}
