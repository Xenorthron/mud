public class Buff : Item
{
    public int AttackBonus { get; private set; }
    public int DefenseBonus { get; private set; }
    public int Duration { get; set; } = 10; // Duration in turns
    public Buff(string name, string description, int attackBonus, int defenseBonus) : base(name, description, 0)
    {
        AttackBonus = attackBonus;
        DefenseBonus = defenseBonus;
    }
    public override void Use(PlayerCharacter player)
    {
        player.Attack += AttackBonus;
        player.Defense += DefenseBonus;
        player.ActiveBuffs.Add(this);
    }
    public override Item Clone() => new Buff(Name, Description, AttackBonus, DefenseBonus);
}