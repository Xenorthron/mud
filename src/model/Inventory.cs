public class Inventory
{
    public Bag[] Contents { get; set; }
    public Inventory()
    {
        Contents = new Bag[6];
    }
    public int getValue()
    {
        int totalValue = 0;
        foreach (var bag in Contents)
        {
            if (bag != null)
            {
                foreach (var item in bag.Contents)
                {
                    if (item != null)
                    {
                        totalValue += item.Value;
                    }
                }
            }
        }
        return totalValue;
    }
}