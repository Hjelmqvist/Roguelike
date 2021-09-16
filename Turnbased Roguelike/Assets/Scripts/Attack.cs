using UnityEngine;

[System.Serializable]
public class Attack
{
    [SerializeField] string _attackName = "Punch";
    [SerializeField] int _damage = 3;
    [SerializeField] int _range = 1;

    [Tooltip("If the attack can damage multiple enemies in range or not")]
    [SerializeField] bool _isPiercing = false;

    public string AttackName => _attackName;
    public int Damage => _damage;
    public int Range => _range;
    public bool IsPiercing => _isPiercing;
}