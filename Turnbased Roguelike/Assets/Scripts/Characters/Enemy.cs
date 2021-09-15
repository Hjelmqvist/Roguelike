using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] Attack _enemyAttack;
    [SerializeField] float _chaseRange = 5;

    public void MakeMove(Tile[,] tiles, Entity player)
    {
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.left, Vector2Int.down, Vector2Int.right };
        float distanceToPlayer = Vector2Int.Distance(_currentPosition, player.Position);

        if (distanceToPlayer <= _enemyAttack.Range) // Close enough to attack
        {
            SetDirection(player.Position - _currentPosition);
            Attack(tiles, _enemyAttack);
        }
        else if (distanceToPlayer <= _chaseRange) // Close enough to chase
        {
            Debug.Log("move to player");
            TryMoveTowardPlayer(tiles, player);
        }
        else // Move randomly
        {
            
        }
    }

    private bool TryMoveTowardPlayer(Tile[,] tiles, Entity player)
    {
        if (Pathfinding.TryGetPath(tiles, _currentPosition, player.Position, out List<Vector2Int> path))
        {
            if (TryMovePosition(tiles, path[1] - _currentPosition))
                SetDirection(player.Position - _currentPosition);
            return true;
        }
        return false;
    }
}