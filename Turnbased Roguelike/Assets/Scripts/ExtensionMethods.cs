using UnityEngine;

public static class ExtensionMethods
{
    public static bool InRange(this Tile[,] tiles, Vector2Int position)
    {
        return position.x >= 0 && position.x < tiles.GetLength(0) && // Inside x range
               position.y >= 0 && position.y < tiles.GetLength(1);   // Inside y range
    }
}