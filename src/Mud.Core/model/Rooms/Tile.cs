public class Tile
{
    public object? Object { get; private set; }
    public ObjectType Type { get; private set; }
    
    public Tile(ObjectType type, object? obj)
    {
        Type = type;
        Object = obj;
    }

    public void SetObject(ObjectType type, object? obj)
    {
        Type = type;
        Object = obj;
    }
}