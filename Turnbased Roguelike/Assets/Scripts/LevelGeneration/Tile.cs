using UnityEngine;

public class Tile
{
    public enum TileType { Walkable, NotWalkable }

    TileType _type;
    Entity _entity;
    Item _item;

    public TileType Type => _type;
    public bool IsWalkable => _type == TileType.Walkable && !_entity;

    public bool TryGetEntity(out Entity entity)
    {
        entity = _entity;
        return entity != null;
    }

    public void SetTileType(TileType type)
    {
        _type = type;
    }

    public void SetItem(Item item)
    {
        _item = item;
    }

    public Item TryGetItem()
    {
        Item item = _item;
        return item;
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