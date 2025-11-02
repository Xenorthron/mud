public class Tile
{
    public object? Object { get; private set; }
    public ObjectType Type { get; private set; }
    public char Symbol
    {
        get
        {
            return Type switch
            {
                ObjectType.EMPTY => '#',
                ObjectType.TRAP when (Object is Trap trap && trap.IsDetected) => '^',
                ObjectType.TRAP when (Object is Trap trap && !trap.IsDetected) => '#',
                ObjectType.CHEST => 'C',
                ObjectType.CHARACTER => 'E',
                ObjectType.OBSTACLE => 'X',
                _ => '#',
            };
        }
    }
    
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