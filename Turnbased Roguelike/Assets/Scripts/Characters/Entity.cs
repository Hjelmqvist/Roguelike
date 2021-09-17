using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Health), typeof(SpriteRenderer))]
public abstract class Entity : MonoBehaviour
{
    [SerializeField] float _yOffset; // Different sprite sizes etc.
    [SerializeField] float _attackDuration = 0.1f;
    [SerializeField] float _attackJumpDistance = 0.5f;

    protected Vector2Int _currentPosition = new Vector2Int();
    protected Vector2Int _currentDirection = Vector2Int.right;
    protected Tile _currentTile;

    public Vector2Int Position => _currentPosition;
    public Health Health { get; private set; }
    public SpriteRenderer SpriteRenderer { get; private set; }

    public UnityEvent<Entity> OnEntityDeath;

    protected virtual void Awake()
    {
        Health = GetComponent<Health>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
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
        Vector3 worldPosition = new Vector3(_currentPosition.x, _currentPosition.y + _yOffset, transform.position.z);
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

        StopAllCoroutines();
        SetWorldPosition(_currentPosition); // Fixes moving outside the tile
        StartCoroutine(AttackCoroutine());

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

    IEnumerator AttackCoroutine()
    {
        float duration = _attackDuration / 2;
        Vector2 startPosition = transform.position;
        Vector2 endPosition = transform.position + (Vector3)(Vector2)_currentDirection * _attackJumpDistance; // Cursed typecasting
        bool movingForward = true;

        for (float time = 0; time <= duration; time += Time.deltaTime)
        {
            if (movingForward)
            {
                transform.position = Vector2.Lerp(startPosition, endPosition, time / duration);
                if (time >= duration)
                {
                    time = 0;
                    movingForward = false;
                }
            }
            else
                transform.position = Vector2.Lerp(endPosition, startPosition, time / duration);
            yield return null;
        }
        transform.position = startPosition;
    }
}