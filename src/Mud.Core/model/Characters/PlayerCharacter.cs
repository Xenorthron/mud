public class PlayerCharacter : Character
{
    public List<Buff> ActiveBuffs { get; private set; } = new List<Buff>();
    public Inventory Inventory { get; private set; }
    public Weapon? Weapon { get; set; }
    public Armor? Armor { get; set; }
    public int Gold { get; set; } = 0;
    public PlayerCharacter(string name, string description) : base(name, description, 100, 10, 0)
    {
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
    public void GameTick()
    {
        for (int i = ActiveBuffs.Count - 1; i >= 0; i--)
        {
            ActiveBuffs[i].Duration--;
            if (ActiveBuffs[i].Duration <= 0)
            {
                // Remove buff effects
                this.Attack -= ActiveBuffs[i].AttackBonus;
                this.Defense -= ActiveBuffs[i].DefenseBonus;
                ActiveBuffs.RemoveAt(i);
            }
        }
    }
}