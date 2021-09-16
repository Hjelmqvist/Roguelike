using System.Collections.Generic;
using Random = UnityEngine.Random;
using UnityEngine;

public class GenScript : MonoBehaviour
{
    public int baseColumns;
    public int baseRows;
    
    public int currentLevel;
    public int wallChance;  // Chance for a wall to spawn instead of a floor, on a 1/X basis
    public int maxEnemyBase; // base number of enemies, affected by currentlevel * 0.5
    
    public GameObject[] floorVariants;
    public GameObject[] wallVariants;
    
    public ExitScript exit;
    public Enemy[] enemies;
    public Player playerObject;
    
    public Item[] shopItems;  // Items that can appear in a shop
    public ShopItem shopItemHolder; //Object that holds items and handles transactions in shops
    
    private Transform _boardHolder; // Object to hold all other objects and tiles
    private Tile[,] _tiles;
    
    private float _divisionHandler; // Float to handle multiplication and division
    private int _columns;
    private int _rows;
    private int _maxEnemyAmount;
    
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
            _divisionHandler = (float) (maxEnemyBase * 0.7 * currentLevel);
            _maxEnemyAmount = (int) _divisionHandler;
        }
        else
        {
            _maxEnemyAmount = maxEnemyBase / 2;
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
                {} // to be free of walls
                else if (x == _columns - 2 && y == _rows - 2)
                {
                    ExitPlace(x,y);
                }
                else if (x == _columns - 3 && y == _rows - 2 || x == _columns - 3 && y == _rows - 3 || x == _columns - 2 && y == _rows - 3)
                {} // to be free of walls
                else if (Random.Range(0, wallChance) == 0)
                {
                   WallPlace(x,y);
                }
            }
        }
    }
    private void EnemyPlacement() // Places enemies randomly
    {

        for (int enemyCount = 0; enemyCount < _maxEnemyAmount; enemyCount++)
        {
            int randomX = Random.Range(1, _columns - 1);
            int randomY = Random.Range(1, _rows - 1);
            Tile randomTile = Tiles[randomX, randomY];
            if (randomTile.IsWalkable && !randomTile.TryGetEntity(out Entity entity)) // if a tile is already occupied, do not place enemy
            {
                Enemy baddie = Instantiate(enemies[Random.Range(0, enemies.Length)], new Vector2(randomX, randomY), Quaternion.identity);
                baddie.transform.SetParent(_boardHolder);
                baddie.SetTile(randomTile);
                baddie.SetWorldPosition(new Vector2Int(randomX,randomY));
                enemyController.AddEnemy(baddie);
            } // the fact that not all enemies need to spawn is intentional.
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
                else if(x % 3 == 0 && y == _rows - 5) // only works with the current amount of items and fixed size of the shop
                {
                    ShopItem shopItem = Instantiate(shopItemHolder, new Vector2(x,y), Quaternion.identity);  // Spawns object to hold item
                    shopItem.transform.SetParent(_boardHolder);
                    Item itemToSell = Instantiate(shopItems[itemSelection], new Vector2(x, y), Quaternion.identity); // spawns item
                    itemToSell.transform.SetParent(_boardHolder);
                    shopItem._itemToSell = itemToSell;  // sets item to shopObject
                    TextMesh displayedPrice = shopItem.GetComponentInChildren<TextMesh>(); 
                    displayedPrice.text = shopItem._itemToSell.Price.ToString();  // Displays price
                    _tiles[x, y].SetTileType(Tile.TileType.Walkable);
                    _tiles[x, y].SetItem(shopItem);
                    itemSelection += 1; // increments through list
                }
            }
        }
    }
    private void CheckIfObstructed()  // checks to see if the generated level is passable
    {                                 // about 2.5% of generated levels are impassable
        if(!Pathfinding.TryGetPath(_tiles, new Vector2Int(2, 2), new Vector2Int(_columns - 3, _rows - 3),
            out List<Vector2Int> path))
        { 
            Destroy(_boardHolder.gameObject); 
            enemyController.GetComponent<EnemyController>().ClearEnemyList();
            MapGeneration();  //does not increase currentlevel
        }
    }

   private void PlayerPlace(int x, int y)
    {
        playerObject.SetTile(_tiles[x, y]);
        playerObject.SetWorldPosition(new Vector2Int(x, y));
        playerController.SetPlayer(playerObject);
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
   private void ShopSetup() // fixed size
    {
        _columns = 16;
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
   private void MapGeneration()
    {
        if (currentLevel % 3 == 0 && currentLevel != 0)  // every level divisible by 3 is a shop
        {
            ShopSetup();
        }
        else
        {
            Initialize();
            BoardSetup();
            ObjectPlacement();
            EnemyPlacement();
            CheckIfObstructed();
        }
    }
    private void Start()
    { 
        MapGeneration();
    }
}