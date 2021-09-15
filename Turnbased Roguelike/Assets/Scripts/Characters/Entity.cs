using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SpriteRenderer), typeof(Health))]
public abstract class Entity : MonoBehaviour
{
    [SerializeField] float yOffset; // Different sprite sizes etc.

    protected Vector2Int _currentPosition = new Vector2Int();
    protected Vector2Int _currentDirection = new Vector2Int();
    protected Tile _currentTile;

    public Vector2Int Position => _currentPosition;
    public Health Health { get; private set; }
    public SpriteRenderer SpriteRenderer { get; private set; }

    public UnityEvent<Entity> OnEntityDeath;

    protected virtual void Awake()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();
        Health = GetComponent<Health>();
    }

    public void OnHealthChanged(Health health)
    {
        if (health.CurrentHealth <= 0)
        {
            _currentTile.LeaveTile();
            OnEntityDeath.Invoke(this);
            Destroy(gameObject);
        }    
    }

    /// <summary>
    /// Tries to set the position of the entity.
    /// Returns false if position is not allowed.
    /// </summary>
    public bool TryMovePosition(Tile[,] tiles, Vector2Int direction)
    {
        SetDirection(direction);

        Vector2Int newPosition = _currentPosition + direction;
        if (!tiles.InRange(newPosition) || !tiles[newPosition.x, newPosition.y].IsWalkable)
            return false;

        SetTile(tiles[newPosition.x, newPosition.y]);
        SetWorldPosition(newPosition);
        return true;
    }

    public void SetDirection(Vector2Int direction)
    {
        direction.Clamp(-Vector2Int.one, Vector2Int.one);
        _currentDirection = direction;
        if (direction.x != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = direction.x;
            transform.localScale = scale;
        }
    }

    public void SetWorldPosition(Vector2Int position)
    {
        _currentPosition = position;
        Vector3 worldPosition = new Vector3(_currentPosition.x, _currentPosition.y + yOffset, transform.position.z);
        transform.position = worldPosition;
    }

    public void SetTile(Tile tile)
    {
        if (_currentTile != null)
            _currentTile.LeaveTile();
        _currentTile = tile;
         tile.EnterTile(this);
    }

    /// <summary>
    /// Returns if the entity could attack something
    /// </summary>
    protected bool Attack(Tile[,] tiles, Attack attack)
    {
        bool hitSomething = false;
        for (int i = 1; i <= attack.Range; i++)
        {
            Vector2Int posToCheck = _currentPosition + _currentDirection * i;

            // Check if the position is valid and contains an entity.
            if (tiles.InRange(posToCheck) && tiles[posToCheck.x, posToCheck.y].TryGetEntity(out Entity entity))
            {
                entity.Health.ModifyHealth(-attack.Damage);
                hitSomething = true;
                if (!attack.IsPiercing)
                    break;
            }
        }
        return hitSomething;
    }
}