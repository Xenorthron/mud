public class PlayerCharacter : Character
{
    public Inventory Inventory { get; private set; }
    public Weapon Weapon { get; set; }
    public Armor Armor { get; set; }
    public PlayerCharacter(string name, string description)
    {
        Name = name;
        Description = description;
        Health = 100;
        Attack = 10;
        Defense = 0;
        Inventory = new Inventory();
    }
    public void Heal(int amount)
    {
        Health += amount;
        if (Health > 100)
        {
            Health = 100; // Max health cap
        }
    }
}