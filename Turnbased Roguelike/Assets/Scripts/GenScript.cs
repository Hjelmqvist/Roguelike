using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using Random = UnityEngine.Random;
using UnityEngine;
using UnityEngine.Serialization;

public class GenScript : MonoBehaviour
{
    [Serializable]
    public class Count
    {
        public int minimum;
        public int maximum;
        public Count (int min, int max)
        {
            minimum = min;
            maximum = max;
        }
    }
    public int columns;
    public int rows;
    
    public int itemChance; // Change to shop system?
    public int enemyChance; // make a function of LVL
    public GameObject exit;
    public GameObject player;
    public GameObject[] items;
    public GameObject[] enemies;
    public GameObject[] floorVariants;
    public GameObject[] wallVariants;
    private Transform _boardHolder;
    private Transform _enemyController;
    private List<Vector2> _boardpositions = new List<Vector2>();

    void InitializeList()
    {
        _boardpositions.Clear();
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                _boardpositions.Add(new Vector2(x,y));
            }
        }
    }

    void BoardSetup()
    {
        _boardHolder = new GameObject("Board").transform;
        _enemyController = new GameObject("EnemyController").transform;
        for (int x = - 1; x < columns + 1; x++)
        {
            for (int y = - 1; y < rows + 1; y++)
            {
                GameObject toInstanciate = floorVariants[Random.Range(0, floorVariants.Length)];
                if (x == -1 || x == columns || y == -1 || y == rows)
                {
                    toInstanciate = wallVariants[Random.Range(0, wallVariants.Length)];
                }

                GameObject instance = Instantiate(toInstanciate, new Vector2(x,y), Quaternion.identity);
                instance.transform.SetParent(_boardHolder);
            }            
        }
    }

    void ObjectPlacement()
    {
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                GameObject toInstanciate = items[Random.Range(0, items.Length)];
                if (x == 0 && y == 0)
                {
                    GameObject playerObject = Instantiate(player, new Vector2(x,y), Quaternion.identity);
                }
                else if (x == columns - 1 && y == rows - 1)
                {
                    GameObject exitObject = Instantiate(exit, new Vector2(x,y), Quaternion.identity);
                }
                else if (Random.Range(0, itemChance) == 0)
                {
                    GameObject instance = Instantiate(toInstanciate, new Vector2(x,y), Quaternion.identity);
                    instance.transform.SetParent(_boardHolder); 
                }
                else if (Random.Range(0, enemyChance) == 0)
                {
                    GameObject baddie = Instantiate(enemies[Random.Range(0, enemies.Length)], new Vector2(x,y), Quaternion.identity);
                    baddie.transform.SetParent(_enemyController); 
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeList();
        BoardSetup();
        ObjectPlacement();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
