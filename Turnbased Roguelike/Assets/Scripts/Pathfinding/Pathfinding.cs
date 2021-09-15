using System;
using System.Collections.Generic;
using UnityEngine;

public static class Pathfinding
{
    public static bool TryGetPath(Tile[,] grid, Vector2Int start, Vector2Int end, out List<Vector2Int> path)
    {
        PathNode startNode = new PathNode(start, start, end, null);
        PathNode endNode = new PathNode(end, start, end, null);
        List<PathNode> nodesToCheck = new List<PathNode>() { startNode };
        List<PathNode> checkedNodes = new List<PathNode>();

        while (nodesToCheck.Count > 0)
        {
            PathNode currentNode = nodesToCheck[0];
            int currentIndex = 0;
            GetNodeWithLowestFCost(nodesToCheck, ref currentNode, ref currentIndex);
            nodesToCheck.RemoveAt(currentIndex);
            checkedNodes.Add(currentNode);

            // If we arrived at the end then get the path that led there
            if (currentNode.Position == endNode.Position)
            {
                path = currentNode.GetPath();
                return true;
            }

            nodesToCheck.AddRange(currentNode.GetChildren(grid, start, end, nodesToCheck, checkedNodes));
        }
        path = null;
        return false;
    }

    private static void GetNodeWithLowestFCost(List<PathNode> nodesToCheck, ref PathNode currentNode, ref int currentIndex)
    {
        for (int i = 0; i < nodesToCheck.Count; i++)
        {
            if (nodesToCheck[i].F < currentNode.F)
            {
                currentNode = nodesToCheck[i];
                currentIndex = i;
            }
        }
    }

    /// <summary>
    /// Returns the path from the start to this node
    /// </summary>
    private static List<Vector2Int> GetPath(this PathNode node)
    {
        List<Vector2Int> path = new List<Vector2Int>();
        PathNode current = node;
        while (current != null)
        {
            path.Add(current.Position);
            current = current.Parent;
        }
        path.Reverse();
        return path;
    }

    /// <summary>
    /// Returns all valid directions to move from this node
    /// </summary>
    private static List<PathNode> GetChildren(this PathNode node, Tile[,] grid, Vector2Int start, Vector2Int end, List<PathNode> nodesToCheck, List<PathNode> checkedNodes)
    {
        List<PathNode> children = new List<PathNode>();
        Vector2Int[] _directions = { Vector2Int.up, Vector2Int.left, Vector2Int.down, Vector2Int.right };

        foreach (Vector2Int dir in _directions)
        {
            Vector2Int position = node.Position + dir;
            PathNode newNode = new PathNode(position, start, end, node);

            if (!grid.InRange(position) || grid[position.x, position.y].Type == Tile.TileType.NotWalkable || 
                nodesToCheck.Contains(newNode) || 
                checkedNodes.Contains(newNode))
                continue;

            children.Add(newNode);
        }
        return children;
    }

    private static bool NodesContainsPosition(List<PathNode> nodes, Vector2Int position)
    {
        foreach (PathNode node in nodes)
        {
            if (node.Position == position)
                return true;
        }
        return false;
    }
}