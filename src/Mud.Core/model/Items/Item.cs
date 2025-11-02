public abstract class Item
{
    public string Name { get; init; }
    public string Description { get; init; }
    public int Value { get; init; }
    public Item(string name, string description, int value)
    {
        Name = name;
        Description = description;
        Value = value;
    }
    public abstract void Use(PlayerCharacter player);
    public abstract Item Clone();
    public override int GetHashCode() => HashCode.Combine(Name, Description, Value);
    public override bool Equals(object? obj)
    {
        return obj?.GetHashCode() == GetHashCode();
    }
}