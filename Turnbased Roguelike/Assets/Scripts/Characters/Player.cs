using UnityEngine;
using UnityEngine.Events;

public class Player : Entity
{
    [SerializeField] Weapon _heldWeapon;
    [SerializeField] Attack _basicAttack;
    [SerializeField] int _gold;

    public Attack CurrentWeapon => _heldWeapon ? _heldWeapon.WeaponAttack : _basicAttack;
    public int Gold => _gold;

    public UnityEvent<int> OnGoldChanged;

    public void Attack()
    {
        Attack(CurrentWeapon);
    }

    public void ModifyGold(int amount)
    {
        _gold = Mathf.Clamp(_gold + amount, 0, 999);
        OnGoldChanged.Invoke(_gold);
    }
}