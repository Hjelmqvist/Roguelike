using UnityEngine;

[RequireComponent(typeof(SpriteRenderer), typeof(Health))]
public abstract class Entity : MonoBehaviour
{
    [SerializeField] float yOffset;
    SpriteRenderer _spriteRenderer;
    Health _health;

    protected Vector2Int _currentPosition = new Vector2Int();
    protected Vector2Int _currentDirection = new Vector2Int();
    protected Tile _currentTile;

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
    public bool TryMovePosition(Tile[,] tiles, Vector2Int direction)
    {
        Vector2Int newPosition = _currentPosition + direction;
        if (!tiles.InRange(newPosition) || !tiles[newPosition.x, newPosition.y].IsWalkable)
            return false;

        SetTile(tiles[newPosition.x, newPosition.y]);
        SetWorldPosition(newPosition);
        SetDirection(direction);
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

    protected void Attack(Tile[,] tiles, Attack attack)
    {
        for (int i = 1; i <= attack.Range; i++)
        {
            Vector2Int posToCheck = _currentPosition + _currentDirection * i;

            // Check if the position is valid and contains an entity.
            if (tiles.InRange(posToCheck) && tiles[posToCheck.x, posToCheck.y].TryGetEntity(out Entity entity))
            {
                entity.Health.ModifyHealth(-attack.Damage);
                if (!attack.IsPiercing)
                    break;
            }
        }
    }
}