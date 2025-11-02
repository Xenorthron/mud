public class NonPlayerCharacter : Character
{
    public int ConcreteAttack { get; init; }
    public int ConcreteDefense { get; init; }
    public bool IsNocturnal { get; init; }
    public NonPlayerCharacter(string name, string description, int health, int attack, int defense, bool IsNocturnal) : base(name, description, health, attack, defense)
    {
        this.ConcreteAttack = attack;
        this.ConcreteDefense = defense;
        this.IsNocturnal = IsNocturnal;
    }
    public void SwitchToDay()
    {
        if (IsNocturnal)
        {
            base.Attack = (int)(ConcreteAttack * 0.8);
            base.Defense = (int)(ConcreteDefense * 0.8);
        }
        else
        {
            base.Attack = (int)(ConcreteAttack * 1.1);
            base.Defense = (int)(ConcreteDefense * 1.1);
        }
    }
    public void SwitchToNight()
    {
        if (IsNocturnal)
        {
            base.Attack = (int)(ConcreteAttack * 1.2);
            base.Defense = (int)(ConcreteDefense * 1.2);
        }
        else
        {
            base.Attack = (int)(ConcreteAttack * 0.9);
            base.Defense = (int)(ConcreteDefense * 0.9);
        }
    }
    public override void AttackTarget(Character target)
    {
        int totalDefense = target.Defense + (target is PlayerCharacter pc && pc.Armor != null ? pc.Armor.DefenseBonus : 0);
        int damage = this.Attack - totalDefense;
        if (damage > 0)
        {
            target.Health -= damage;
        }
        else
        {
            target.Health -= 1; // Minimum damage
        }
    }
    public NonPlayerCharacter Clone() => new NonPlayerCharacter(Name, Description, Health, ConcreteAttack, ConcreteDefense, IsNocturnal);
}