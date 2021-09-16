using UnityEngine;

public class Weapon : Item
{
    [SerializeField] Attack _attack = null;

    public Attack WeaponAttack => _attack;

    public override bool Interact(Player player)
    {
        player.EquipWeapon(this);
        return true;
    }

    public void Equip(Transform parent)
    {
        transform.SetParent(parent);
        transform.localPosition = Vector2.zero;

        // Match the player characters current direction
        Vector3 scale = transform.localScale;
        scale.x *= transform.root.localScale.x;
        transform.localScale = scale;
    }
}