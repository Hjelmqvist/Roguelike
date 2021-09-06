using System.Collections;
using System.Collections.Generic;
using UnityEngine;

 [CreateAssetMenu(fileName = "New weapon", menuName = "Weapons/Create new weapon")]
public class WeaponSO : ScriptableObject
{
    [SerializeField] string _weaponName = "Dull Sword";
    [SerializeField] int _damage = 3;
    [SerializeField] int _range = 1;

    public string WeaponName => _weaponName;
    public int Damage => _damage;
    public int Range => _range;
}
