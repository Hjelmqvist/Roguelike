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
        if (newPosition.x < 0 || newPosition.x > tiles.GetLength(0) || // Outside x range
            newPosition.y < 0 || newPosition.y > tiles.GetLength(1) || // Outside y range
            !tiles[newPosition.x, newPosition.y].IsWalkable)           // Or not walkable
            return false;

        SetTile(tiles[newPosition.x, newPosition.y]);
        SetPosition(newPosition);

        _currentDirection = direction;
        if (direction.x != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = direction.x;
            transform.localScale = scale;
        }
        return true;
    }

    public void SetPosition(Vector2Int position)
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