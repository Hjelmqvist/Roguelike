using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using Random = UnityEngine.Random;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    public int baseColumns;
    public int baseRows;
    
    public int itemChance; // Change to shop system?
    public int enemyBaseChance;
    public GameObject exit;
    public GameObject player;
    public GameObject saveObject; //Bad name
    public GameObject[] items;
    public Enemy[] enemies;
    public GameObject[] floorVariants;
    public GameObject[] wallVariants;
    private int _currentLevel;
    private float _divisionHandler;
    private int _enemyTotalChance;
    private Transform _boardHolder;
    private InfoToSave _saveFile;
    private Tile[,] _tiles;
    private static bool _saved;
    private int _columns;
    private int _rows;
    
        void Initialize()
        {
            _columns = baseColumns + _currentLevel;
            _rows = baseRows + _currentLevel;
                
                _tiles = new Tile[_columns, _rows];
        for (int x = 0; x < _columns; x++)
        {
            for (int y = 0; y < _rows; y++)
            {
                _tiles[x, y] = new Tile();
            }
        }
        if (_currentLevel != 0)
        {
            _divisionHandler = enemyBaseChance / _currentLevel + 1;
            _enemyTotalChance = (int)_divisionHandler;
        }
        else
        {
            _enemyTotalChance = enemyBaseChance + 1;
        }
    }

    void HandleSaveFile()
    {
        if (!_saved)
        {
            GameObject saveInstance = Instantiate(saveObject, new Vector2(0, 0), Quaternion.identity);
            _saveFile = saveInstance.GetComponent<InfoToSave>();
            DontDestroyOnLoad(saveInstance);
            _saved = true;
        }
        else
        {
            _saveFile = GameObject.FindWithTag("Save").GetComponent<InfoToSave>();
        }
        _currentLevel = _saveFile.CurrentLevel;
    }
    void BoardSetup()
    {
        bool wall = false;
        _boardHolder = new GameObject("Board").transform;
        for (int x = 0; x < _columns ; x++)
        {
            for (int y = 0; y < _rows ; y++)
            {
                GameObject toInstantiate = floorVariants[Random.Range(0, floorVariants.Length)];
                if (x == 0 || x == _columns -1 || y == 0 || y == _rows -1)
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
        for (int x = 1; x < _columns -1; x++)
        {
            for (int y = 1; y < _rows -1; y++)
            {
                GameObject toInstantiate = items[Random.Range(0, items.Length)];
                if (x == 1 && y == 1)
                {
                    GameObject playerObject = Instantiate(player, new Vector2(x,y), Quaternion.identity);
                }
                else if (x == 2 && y == 1 || x == 2 && y == 2 || x == 1 && y == 2)
                {}
                else if (x == _columns - 2 && y == _rows - 2)
                {
                    GameObject exitObject = Instantiate(exit, new Vector2(x,y), Quaternion.identity);
                }
                else if (Random.Range(0, itemChance) == 0)
                {
                    GameObject instance = Instantiate(toInstantiate, new Vector2(x,y), Quaternion.identity);
                    instance.transform.SetParent(_boardHolder);
                    _tiles[x, y].Gobject = instance;
                }
                else if (Random.Range(0, _enemyTotalChance) == 0)
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
        HandleSaveFile();
        Initialize();
        BoardSetup();
        ObjectPlacement();
    }
}
