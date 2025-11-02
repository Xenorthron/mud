public class PlayerCharacter : Character
{
    public Inventory Inventory { get; private set; }
    public Weapon? Weapon { get; set; }
    public Armor? Armor { get; set; }
    public PlayerCharacter(string name, string description)
    {
        Name = name;
        Description = description;
        Health = 100;
        Attack = 10;
        Defense = 0;
        Inventory = new Inventory();
        Weapon = null;
        Armor = null;
    }
    public void Heal(int amount)
    {
        Health += amount;
        if (Health > 100)
        {
            Health = 100; // Max health cap
        }
    }
    public override void AttackTarget(Character target)
    {
        int totalAttack = this.Attack + (Weapon?.AttackBonus ?? 0);
        int totalDefense = target.Defense + (target is PlayerCharacter pc && pc.Armor != null ? pc.Armor.DefenseBonus : 0);
        int damage = totalAttack - totalDefense;
        if (damage > 0)
        {
            target.Health -= damage;
        }
        else
        {
            target.Health -= 1; // Minimum damage
        }
    }
}