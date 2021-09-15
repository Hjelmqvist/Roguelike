using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using Random = UnityEngine.Random;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
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

    public int baseColumns;
    public int baseRows;
    
    public int wallChance;
    public int enemyBaseChance;
    public GameObject exit;
    public Player player;
    public GameObject saveObject; //Bad name
    public Enemy[] enemies;
    public GameObject[] floorVariants;
    public GameObject[] wallVariants;
    public Item[] storeItems;
    private int _currentLevel;
    private float _divisionHandler;
    private int _enemyTotalChance;
    private Transform _boardHolder;
    private InfoToSave _saveFile;
    private Tile[,] _tiles;
    private static bool _saved;
    private int _columns;
    private int _rows;

    public Tile[,] Tiles => _tiles;

    [SerializeField] PlayerController playerController;
    [SerializeField] EnemyController enemyController;

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
                    _tiles[x, y].SetTileType(Tile.TileType.NotWalkable);
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
                if (x == 1 && y == 1)
                {
                    Player playerObject = Instantiate(player, new Vector2(x, y), Quaternion.identity);
                    playerObject.SetTile(_tiles[x, y]);
                    playerObject.SetWorldPosition(new Vector2Int(x, y));
                    playerController.SetPlayer(playerObject);
                }
                else if (x == 2 && y == 1 || x == 2 && y == 2 || x == 1 && y == 2)
                {}
                else if (x == _columns - 2 && y == _rows - 2)
                {
                    GameObject exitObject = Instantiate(exit, new Vector2(x,y), Quaternion.identity);
                }
                else if (x == _columns - 3 && y == _rows - 2 || x == _columns - 3 && y == _rows - 3 || x == _columns - 2 && y == _rows - 3)
                {}
                else if (Random.Range(0, wallChance) == 0)
                {
                    GameObject instance = Instantiate(wallVariants[Random.Range(0, wallVariants.Length)], new Vector2(x,y), Quaternion.identity);
                    instance.transform.SetParent(_boardHolder);
                    _tiles[x, y].SetTileType(Tile.TileType.NotWalkable);
                }
                else if (Random.Range(0, _enemyTotalChance) == 0)
                {
                    Enemy baddie = Instantiate(enemies[Random.Range(0, enemies.Length)], new Vector2(x, y), Quaternion.identity);
                    baddie.transform.SetParent(_boardHolder);
                    baddie.SetTile(_tiles[x, y]);
                    baddie.SetWorldPosition(new Vector2Int(x, y));
                    
                    enemyController.AddEnemy(baddie);
                }
            }
        }
    }
    void ShopObjectPlacement()
    {
        int itemSelection = 0;
        for (int x = 1; x < _columns - 1; x++)
        {
            for (int y = 1; y < _rows - 1; y++)
            {
                if (x == 1 && y == 1)
                {
                    Player playerObject = Instantiate(player, new Vector2(x, y), Quaternion.identity);
                    playerObject.SetTile(_tiles[x, y]);
                    playerObject.SetWorldPosition(new Vector2Int(x, y));
                    playerController.SetPlayer(playerObject);
                }
                else if (x == _columns - 2 && y == _rows - 2)
                {
                    GameObject exitObject = Instantiate(exit, new Vector2(x,y), Quaternion.identity);
                }
                else if(x % 3 == 0 && y == _rows - 4)
                {
                    Item item = Instantiate(storeItems[itemSelection], new Vector2(x,y), Quaternion.identity);
                    item.transform.SetParent(_boardHolder);
                    TextMesh displayedPrice = item.GetComponentInChildren<TextMesh>();
                    displayedPrice.text = item.GetComponent<Item>().price.ToString();
                    _tiles[x, y].SetTileType(Tile.TileType.Walkable);
                    _tiles[x, y].SetItem(item);
                    itemSelection += 1;
                }
            }
        }
    }
    void CheckIfObstructed()
    {
        if(!Pathfinding.TryGetPath(_tiles, new Vector2Int(2, 2), new Vector2Int(_columns - 3, _rows - 3),
            out List<Vector2Int> path))
        {
            SceneManager.LoadScene(1);
        }
    }
    void ShopSetup()
    {
        _columns = 15 + 1;
        _rows = 10;
        _tiles = new Tile[_columns, _rows];
        
        for (int x = 0; x < _columns; x++)
        {
            for (int y = 0; y < _rows; y++)
            {
                _tiles[x, y] = new Tile();
            }
        }
        BoardSetup();
        ShopObjectPlacement();
    }
    // Start is called before the first frame update
    void Start()
    {
        HandleSaveFile();
        if (_currentLevel % 3 == 0 && _currentLevel != 0)
        {
            ShopSetup();
        }
        else
        {
            Initialize();
            BoardSetup();
            ObjectPlacement();
            CheckIfObstructed();
        }
    }
}