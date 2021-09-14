using UnityEngine;

public class Tile
{
    public enum TileType { Walkable, NotWalkable }

    TileType _type;
    Entity _entity;
    GameObject _item;

    public Entity Entity => _entity;
    public bool IsWalkable => _type == TileType.Walkable && !_entity;

    public void SetTileType(TileType type)
    {
        _type = type;
    }

    public void SetItem(GameObject item)
    {
        _item = item;
    }

    public void EnterTile(Entity entity)
    {
        _entity = entity;
    }

    public void LeaveTile()
    {
        _entity = null;
    }
}