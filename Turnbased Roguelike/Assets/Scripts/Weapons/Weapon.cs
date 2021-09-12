using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] Attack _attack = null;

    public Attack WeaponAttack => _attack;

    public void Pickup(Transform parent)
    {
        transform.SetParent(parent);

        // Match the player characters current direction
        Vector3 scale = transform.localScale;
        scale.x *= transform.root.localScale.x;
        transform.localScale = scale;
    }
}