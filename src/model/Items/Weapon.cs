public class Weapon : Item
{
    public int AttackBonus { get; private set; }
    public Weapon(string name, string description, int value, int attackBonus)
    {
        Name = name;
        Description = description;
        Value = value;
        AttackBonus = attackBonus;
    }
    // public override void Use(PlayerCharacter player)
    // {
    //     currentWeapon = player.Weapon;
    //     player.Weapon = this;
    // }
}