public class Tile
{
    public enum TileType { Walkable, NotWalkable }

    Entity _entity;
    Item _item;
    TileType _type;

    public TileType Type => _type;
    public bool IsWalkable => _type == TileType.Walkable && !_entity;

    public void SetItem(Item item)
    {
        _item = item;
    }

    public void SetTileType(TileType type)
    {
        _type = type;
    }

    public bool TryGetEntity(out Entity entity)
    {
        entity = _entity;
        return entity != null;
    }

    //public Item TryGetItem()
    //{
    //    Item item = _item;
    //    return item;
    //}

    public virtual void Interact(Player player)
    {
        if (_item != null)
            _item.Interact(player);
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