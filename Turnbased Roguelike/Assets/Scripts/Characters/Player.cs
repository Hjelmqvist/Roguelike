using UnityEngine;
using UnityEngine.Events;

public class Player : Entity
{
    [SerializeField] Transform _weaponParent;
    [SerializeField] Weapon _heldWeapon;
    [SerializeField] Attack _basicAttack;
    [SerializeField] int _gold;

    public Attack CurrentWeapon => _heldWeapon ? _heldWeapon.WeaponAttack : _basicAttack;
    public int Gold => _gold;

    public UnityEvent<int> OnGoldChanged;

    private void Start()
    {
        OnGoldChanged.Invoke(_gold);
    }

    public bool Attack(Tile[,] tiles)
    {
        return Attack(tiles, CurrentWeapon);
    }

    public bool Interact()
    {
        if (_currentTile != null)
        {
            _currentTile.Interact(this);
            return true;
        }
        return false;
    }

    public void Heal()
    {
        Health.ModifyHealth(+200);
    }
    public void EquipWeapon(Weapon weapon)
    {
        if (_heldWeapon)
            Destroy(_heldWeapon.gameObject);
        _heldWeapon = weapon;
        _heldWeapon.Equip(_weaponParent);
    }

    public void ModifyGold(int amount)
    {
        _gold = Mathf.Clamp(_gold + amount, 0, 999);
        OnGoldChanged.Invoke(_gold);
    }
}