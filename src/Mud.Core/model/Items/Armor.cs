public class Armor : Item
{
    public int DefenseBonus { get; private set; }
    public Armor(string name, string description, int value, int defenseBonus) : base(name, description, value)
    {
        DefenseBonus = defenseBonus;
    }
    public override void Use(PlayerCharacter player)
    {
        var currentArmor = player.Armor;
        player.Armor = this;
        if (currentArmor != null)
        {
            player.Inventory.AddItem(currentArmor);
        }
    }
    public override Item Clone() => new Armor(Name, Description, Value, DefenseBonus);
}