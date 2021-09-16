using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] Attack _enemyAttack;
    [SerializeField] float _chaseRange = 5;

    public void MakeMove(Tile[,] tiles, Entity player)
    {
        float distanceToPlayer = Vector2Int.Distance(_currentPosition, player.Position);

        if (distanceToPlayer <= _enemyAttack.Range) // Close enough to attack
        {
            SetDirection(player.Position - _currentPosition);
            Attack(tiles, _enemyAttack);
        }
        else
        {
            // Move towards player if possible
            if (distanceToPlayer <= _chaseRange && TryMoveTowardPlayer(tiles, player))
                return;

            // Else just move randomly
            MoveInRandomDirection(tiles);
        }
    }

    private bool TryMoveTowardPlayer(Tile[,] tiles, Entity player)
    {
        if (Pathfinding.TryGetPath(tiles, _currentPosition, player.Position, out List<Vector2Int> path))
        {
            if (TryMovePosition(tiles, path[1] - _currentPosition))
            {
                SetDirection(player.Position - _currentPosition);
                return true;
            }
        }
        return false;
    }

    private void MoveInRandomDirection(Tile[,] tiles)
    {
        List<Vector2Int> directions = new List<Vector2Int>() { Vector2Int.up, Vector2Int.left, Vector2Int.down, Vector2Int.right };
        for (int i = 0; i < directions.Count; i++)
        {
            int randomIndex = Random.Range(0, directions.Count);
            Vector2Int dir = directions[randomIndex];
            directions.RemoveAt(randomIndex);

            if (TryMovePosition(tiles, dir))
                break;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Vector3 pos = new Vector3(_currentPosition.x, _currentPosition.y);
        Gizmos.DrawWireSphere(pos, _chaseRange);
    }
}