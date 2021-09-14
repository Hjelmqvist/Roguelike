using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Health))]
public abstract class Entity : MonoBehaviour
{
    SpriteRenderer _spriteRenderer;
    Health _health;

    protected Vector2Int _currentPosition = new Vector2Int();
    protected Vector2Int _currentDirection = new Vector2Int();

    public Health Health => _health;

    public Vector2Int Position => _currentPosition;

    protected virtual void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _health = GetComponent<Health>();
    }

    /// <summary>
    /// Tries to set the position of the entity.
    /// Returns false if position is not allowed.
    /// </summary>
    public bool TryMovePosition(Vector2Int direction)
    {
        // check if allowed in grid or by raycasting?
        //if (!grid.InRange(posToCheck.x, posToCheck.y))
        // return false;

        _currentPosition += direction;
        Vector3 worldPosition = new Vector3(_currentPosition.x, _currentPosition.y, transform.position.z);
        transform.position = worldPosition;

        _currentDirection = direction;
        if (direction.x != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = direction.x;
            transform.localScale = scale;
        }

        return true;
    }

    protected void Attack(Attack attack)
    {
        for (int i = 1; i <= attack.Range; i++)
        {
            Vector2Int posToCheck = _currentPosition + _currentDirection * i;

            // Check if the positions are valid and contains an entity.
            //if (grid.InRange(posToCheck.x, posToCheck.y) && grid.GetEntityOnPosition(posToCheck, out Entity foundEntity))
            //{
            //    // Do damage to the found entity
            //    foundEntity.Health.ModifyHealth(-attack.Damage);
            //    if (!attack.IsPiercing)
            //        break;
            //}
        }
    }
}