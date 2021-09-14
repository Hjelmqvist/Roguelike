using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] Attack _enemyAttack;

    public void MakeMove(Tile[,] tiles, Entity player)
    {
        if (Pathfinding.TryGetPath(tiles, _currentPosition, player.Position, out List<Vector2Int> path))
        {
            Debug.Log("found path");
            TryMovePosition(tiles, path[0] - _currentPosition);
        }
    }
}