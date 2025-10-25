public abstract class Item
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public int Value { get; private set; }
    public abstract void Use(PlayerCharacter player);
}