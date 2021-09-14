//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Test : MonoBehaviour
//{
//    Vector2Int[,] grid;
//    int x = 100;
//    int y = 100;

//    private void Start()
//    {
//        grid = new Vector2Int[x, y];
//        for (int i = 0; i < x; i++)
//        {
//            for (int j = 0; j < y; j++)
//            {
//                grid[i, j] = new Vector2Int(i, j);
//            }
//        }

//        Debug.Log(Time.realtimeSinceStartup);
//        if (Pathfinding.TryGetPath(grid, Vector2Int.zero, new Vector2Int(99, 50), out List<Vector2Int> path))
//        {
//            foreach (Vector2Int pos in path)
//            {
//                Debug.Log(pos);
//            }
//        }
//        Debug.Log(Time.realtimeSinceStartup);
//    }
//}
