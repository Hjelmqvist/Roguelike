public class Tile
{
    public enum TileType { Walkable, NotWalkable }

    Entity _entity;
    Item _item;

    public TileType Type { get; private set; }
    public bool IsWalkable => Type == TileType.Walkable && !_entity;

    public void SetItem(Item item)
    {
        _item = item;
    }

    public void SetTileType(TileType type)
    {
        Type = type;
    }

    public bool TryGetEntity(out Entity entity)
    {
        entity = _entity;
        return entity != null;
    }

    public virtual bool Interact(Player player)
    {
        if (_item != null)
            return _item.Interact(player);
        return false;
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