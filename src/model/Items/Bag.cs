public class Bag : Item
{
    public int Capacity { get; private set; }
    public Item[] Contents { get; private set; }
    public Bag(string name, string description, int capacity)
    {
        Name = name;
        Description = description;
        Capacity = capacity;
        Contents = new Item[capacity];
    }
    public getValue()
    {
        int totalValue = 0;
        foreach (var item in Contents)
        {
            if (item != null)
            {
                totalValue += item.Value;
            }
        }
        return totalValue;
    }
}