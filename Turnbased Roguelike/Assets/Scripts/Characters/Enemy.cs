using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] Attack _enemyAttack;

    public void MakeMove(Entity player)
    {
        //if (Pathfinding.TryGetPath(grid, _currentPosition, player.Position, out List<Vector2Int> path))
        //{

        //}
    }

    private void Player_OnPlayerMoved(Player player)
    {
        // do movement stuff
    }
}