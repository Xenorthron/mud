public class Bag : Item
{
    public int Capacity { get; private set; }
    public Item?[] Contents { get; private set; }
    public Bag(string name, string description, int capacity) : base(name, description, 0)
    {
        Capacity = capacity;
        Contents = new Item?[capacity];
    }
    public int GetValue()
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
    public int[] GetUsedSlotsRatio()
    {
        int usedSlots = 0;
        for (int i = 0; i < Capacity; i++)
        {
            if (Contents[i] != null)
            {
                usedSlots++;
            }
        }
        return new int[] { usedSlots, Capacity };
    }
    public override void Use(PlayerCharacter player)
    {
        var Bags = player.Inventory.Contents;
        for (int i = 0; i < Bags.Length; i++)
        {
            if (Bags[i] == null)
            {
                Bags[i] = this;
                break;
            }
            if (Bags[i].Capacity < this.Capacity)
            {
                var currentBag = Bags[i];
                for (int j = 0; j < currentBag.Capacity; j++)
                {
                    if (currentBag.Contents[j] != null)
                    {
                        Contents[j] = currentBag.Contents[j];
                        currentBag.Contents[j] = null;
                    }
                }
                Bags[i] = this;
                player.Inventory.AddItem(currentBag);
                break;
            }
        }
    }
    public override Item Clone() => new Bag(Name, Description, Capacity);
}