using System;
using UnityEngine;

public class PathNode : IEquatable<PathNode>
{
    PathNode _parent;
    Vector2Int _pos;
    float _g; // Distance between current node and start node
    float _h; // Distance between current node and end node

    public PathNode Parent => _parent;
    public Vector2Int Position => _pos;
    public float F => _g + _h;

    public PathNode(Vector2Int position, Vector2Int startPosition, Vector2Int endPosition, PathNode parent)
    {
        _pos = position;
        _g = Vector2Int.Distance(position, startPosition);
        _h = Vector2Int.Distance(position, endPosition);
        _parent = parent;
    }

    // Nodes with the same position will be equal, to make it easier with list.Contains()
    public bool Equals(PathNode other)
    {
        return _pos == other._pos;
    }
}