using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using Random = UnityEngine.Random;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

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

    public class Tile
    {
        public GameObject Gobject;
    }
    public int columns;
    public int rows;
    
    public int itemChance; // Change to shop system?
    public float enemyBaseChance;
    public GameObject exit;
    public GameObject player;
    public GameObject saveFile; //Bad name
    public GameObject[] items;
    public Enemy[] enemies;
    public GameObject[] floorVariants;
    public GameObject[] wallVariants;
    private int _currentLevel;
    private Transform _boardHolder;
    private Tile[,] _tiles;
    void Initialize()
    {
        _tiles = new Tile[columns, rows];
        for (int x = 0; x < columns; x++)
        {
            for (int y = 0; y < rows; y++)
            {
                _tiles[x, y] = new Tile();
            }
        }
    }

    void HandleSaveFile()
    {
        _currentLevel = saveFile.GetComponent<InfoToSave>().CurrentLevel;
        DontDestroyOnLoad(saveFile); 
    }
    void BoardSetup()
    {
        bool wall = false;
        _boardHolder = new GameObject("Board").transform;
        for (int x = 0; x < columns ; x++)
        {
            for (int y = 0; y < rows ; y++)
            {
                GameObject toInstantiate = floorVariants[Random.Range(0, floorVariants.Length)];
                if (x == 0 || x == columns -1 || y == 0 || y == rows -1)
                {
                    toInstantiate = wallVariants[Random.Range(0, wallVariants.Length)];
                    wall = true;
                }

                GameObject instance = Instantiate(toInstantiate, new Vector2(x,y), Quaternion.identity);
                instance.transform.SetParent(_boardHolder);
                if (wall)
                { 
                    _tiles[x, y].Gobject = instance;
                }
                wall = false;
            }            
        }
    }

    void ObjectPlacement()
    {
        for (int x = 1; x < columns -1; x++)
        {
            for (int y = 1; y < rows -1; y++)
            {
                GameObject toInstantiate = items[Random.Range(0, items.Length)];
                if (x == 1 && y == 1)
                {
                    GameObject playerObject = Instantiate(player, new Vector2(x,y), Quaternion.identity);
                }
                else if (x == 2 && y == 1 || x == 2 && y == 2 || x == 1 && y == 2)
                {}
                else if (x == columns - 2 && y == rows - 2)
                {
                    GameObject exitObject = Instantiate(exit, new Vector2(x,y), Quaternion.identity);
                }
                else if (Random.Range(0, itemChance) == 0)
                {
                    GameObject instance = Instantiate(toInstantiate, new Vector2(x,y), Quaternion.identity);
                    instance.transform.SetParent(_boardHolder);
                    _tiles[x, y].Gobject = instance;
                }
                else if (Random.Range(0, enemyBaseChance) == 0)
                {
                    Enemy baddie = Instantiate(enemies[Random.Range(0, enemies.Length)], new Vector2(x,y), Quaternion.identity);
                    baddie.transform.SetParent(_boardHolder);
                    EnemyController._enemies.Add(baddie);
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        BoardSetup();
        HandleSaveFile();
        ObjectPlacement();
    }
}
