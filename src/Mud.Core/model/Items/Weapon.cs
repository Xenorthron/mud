public class Weapon : Item
{
    public int AttackBonus { get; private set; }
    public Weapon(string name, string description, int value, int attackBonus) : base(name, description, value)
    {
        AttackBonus = attackBonus;
    }
    public override void Use(PlayerCharacter player)
    {
        var currentWeapon = player.Weapon;
        player.Weapon = this;
        if (currentWeapon != null)
        {
            player.Inventory.AddItem(currentWeapon);
        }
    }
    public override Item Clone() => new Weapon(Name, Description, Value, AttackBonus);
}