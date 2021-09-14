using UnityEngine;

public class Player : Entity
{
    [SerializeField] Weapon _heldWeapon;
    [SerializeField] Attack _basicAttack;

    public Attack CurrentWeapon => _heldWeapon ? _heldWeapon.WeaponAttack : _basicAttack;

    public void Attack()
    {
        Attack(CurrentWeapon);
    }
}