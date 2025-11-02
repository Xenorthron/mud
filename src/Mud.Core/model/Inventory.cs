public class Inventory
{
    public Bag[] Contents { get; set; }
    public Inventory()
    {
        Contents = new Bag[6];
    }
    public int GetValue()
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
    public int[] GetUsedSlotsRatio()
    {
        int usedSlots = 0;
        int totalSlots = 0;
        foreach (var bag in Contents)
        {
            if (bag != null)
            {
                foreach (var item in bag.Contents)
                {
                    totalSlots++;
                    if (item != null)
                    {
                        usedSlots++;
                    }
                }
            }
        }
        return new int[] { usedSlots, totalSlots };
    }
    public void UseItem(int bagIndex, int itemIndex, PlayerCharacter player)
    {
        Bag bag = Contents[bagIndex];
        if (bag != null)
        {
            Item? item = bag.Contents[itemIndex];
            if (item != null)
            {
                item.Use(player);
                bag.Contents[itemIndex] = null; // Remove item after use
            }
        }
    }
    public bool AddItem(Item item)
    {
        foreach (var bag in Contents)
        {
            if (bag != null)
            {
                for (int i = 0; i < bag.Capacity; i++)
                {
                    if (bag.Contents[i] == null)
                    {
                        bag.Contents[i] = item;
                        return true;
                    }
                }
            }
        }
        return false; // Inventory full
    }
}