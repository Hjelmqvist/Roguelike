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
    public int baseColumns;
    public int baseRows;
    
    public int currentLevel;
    public int wallChance;
    public int enemyBaseChance;
    public ExitScript exit;
    public Player player;
    public ShopItem shopItemHolder;
    public Enemy[] enemies;
    public GameObject[] floorVariants;
    public GameObject[] wallVariants;
    public Item[] storeItems;
    private float _divisionHandler;
    private int _enemyTotalChance;
    private Player _playerObject;
    private Transform _boardHolder;
    private Tile[,] _tiles;
    private static bool _saved;
    private int _columns;
    private int _rows;

    public Tile[,] Tiles => _tiles;
    
    
    [SerializeField] PlayerController playerController;
    [SerializeField] EnemyController enemyController;
    private void Initialize()
    {
        _columns = baseColumns + currentLevel;
        _rows = baseRows + currentLevel;
        _tiles = new Tile[_columns, _rows];
        
        for (int x = 0; x < _columns; x++)
        {
            for (int y = 0; y < _rows; y++)
            {
                _tiles[x, y] = new Tile();
            }
        }
        if (currentLevel != 0)
        {
            _divisionHandler = enemyBaseChance / currentLevel + 1;
            _enemyTotalChance = (int)_divisionHandler;
        }
        else
        {
            _enemyTotalChance = enemyBaseChance + 1;
        }
    }
    
    private void BoardSetup()
    {
        _boardHolder = new GameObject("Board").transform;
        for (int x = 0; x < _columns ; x++)
        {
            for (int y = 0; y < _rows ; y++)
            {
                GameObject toInstantiate = floorVariants[Random.Range(0, floorVariants.Length)];
                if (x == 0 || x == _columns -1 || y == 0 || y == _rows -1)
                {
                    WallPlace(x,y);
                }
                else
                {
                    GameObject instance = Instantiate(toInstantiate, new Vector2(x,y), Quaternion.identity);
                    instance.transform.SetParent(_boardHolder);    
                }
            }            
        }
    }
    private void ObjectPlacement()
    {
        for (int x = 1; x < _columns -1; x++)
        {
            for (int y = 1; y < _rows -1; y++)
            {
                if (x == 1 && y == 1)
                {
                    PlayerPlace(x,y);
                }
                else if (x == 2 && y == 1 || x == 2 && y == 2 || x == 1 && y == 2)
                {}
                else if (x == _columns - 2 && y == _rows - 2)
                {
                    ExitPlace(x,y);
                }
                else if (x == _columns - 3 && y == _rows - 2 || x == _columns - 3 && y == _rows - 3 || x == _columns - 2 && y == _rows - 3)
                {}
                else if (Random.Range(0, wallChance) == 0)
                {
                   WallPlace(x,y);
                }
                else if (Random.Range(0, _enemyTotalChance) == 0)  // a bit too random
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
    private void ShopObjectPlacement()
    {
        int itemSelection = 0;
        for (int x = 1; x < _columns - 1; x++)
        {
            for (int y = 1; y < _rows - 1; y++)
            {
                if (x == 1 && y == 1)
                {
                   PlayerPlace(x,y);
                }
                else if (x == _columns - 2 && y == _rows - 2)
                {
                    ExitPlace(x,y);
                }
                else if(x % 3 == 0 && y == _rows - 5)
                {
                    ShopItem shopItem = Instantiate(shopItemHolder, new Vector2(x,y), Quaternion.identity);
                    shopItem.transform.SetParent(_boardHolder);
                    Item itemToSell = Instantiate(storeItems[itemSelection], new Vector2(x, y), Quaternion.identity);
                    itemToSell.transform.SetParent(_boardHolder);
                    shopItem._itemToSell = itemToSell;
                    TextMesh displayedPrice = shopItem.GetComponentInChildren<TextMesh>();
                    displayedPrice.text = shopItem._itemToSell.Price.ToString();
                    _tiles[x, y].SetTileType(Tile.TileType.Walkable);
                    _tiles[x, y].SetItem(shopItem);
                    itemSelection += 1;
                }
            }
        }
    }
    private void CheckIfObstructed()
    {
        if(!Pathfinding.TryGetPath(_tiles, new Vector2Int(2, 2), new Vector2Int(_columns - 3, _rows - 3),
            out List<Vector2Int> path))
        {
           MapGeneration();
        }
    }

   private void PlayerPlace(int x, int y)
    {
        if (_playerObject == null)
        {
            _playerObject = Instantiate(player, new Vector2(x, y), Quaternion.identity);
        }
        _playerObject.SetTile(_tiles[x, y]);
        _playerObject.SetWorldPosition(new Vector2Int(x, y));
        playerController.SetPlayer(_playerObject);
    }

   private void ExitPlace(int x, int y)
    {
        ExitScript exitObject = Instantiate(exit, new Vector2(x,y), Quaternion.identity);
        _tiles[x, y].SetTileType(Tile.TileType.Walkable);
        _tiles[x, y].SetItem(exitObject);
        exitObject.transform.SetParent(_boardHolder);
    }

   private void WallPlace(int x, int y)
    {
        GameObject instance = Instantiate(wallVariants[Random.Range(0, wallVariants.Length)], new Vector2(x,y), Quaternion.identity);
        instance.transform.SetParent(_boardHolder);
        _tiles[x, y].SetTileType(Tile.TileType.NotWalkable);
    }

    public void NextLevel()
    {
        currentLevel += 1;
        Destroy(_boardHolder.gameObject);
        enemyController.GetComponent<EnemyController>().ClearEnemyList();
        MapGeneration();
    }
   private void ShopSetup()
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
    private void MapGeneration()
    {
        if (currentLevel % 3 == 0 && currentLevel != 0)
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
    private void Start()
    { 
        MapGeneration();
    }
}