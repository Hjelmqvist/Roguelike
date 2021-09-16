using UnityEngine;

public class HealthItem : Item
{
    [SerializeField] int healAmount = 200;

    public override bool Interact(Player player)
    {
        player.Health.ModifyHealth(healAmount);
        Destroy(gameObject);
        return true;
    }
}