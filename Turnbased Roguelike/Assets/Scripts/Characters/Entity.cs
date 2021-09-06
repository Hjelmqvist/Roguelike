using UnityEngine;
using UnityEngine.Events;

public abstract class Entity : MonoBehaviour
{
    [SerializeField] int _maxHealth = 100;
    [SerializeField] int _health = 100;

    public int MaxHealth => _maxHealth;
    public int CurrentHealth => _health;

    public UnityEvent<Entity> OnHealthChanged;

    protected Vector2Int _currentPosition = new Vector2Int();

    public virtual void ModifyHealth(int value)
    {
        _health = Mathf.Clamp(_health + value, 0, _maxHealth);
        OnHealthChanged.Invoke(this);
    }

    /// <summary>
    /// Tries to set the position of the entity.
    /// Returns false if position is not allowed.
    /// </summary>
    public bool TrySetPosition(Vector2Int position)
    {
        // check if allowed in grid or by raycasting?

        _currentPosition = position;

        Vector3 worldPosition = new Vector3(position.x, position.y, transform.position.z);
        transform.position = worldPosition;
        return true;
    }

    public void AttackPosition(Vector2Int position)
    {

    }
}