public class NonPlayerCharacter : Character
{
    public NonPlayerCharacter(string name, string description, int health, int attack, int defense) : base(name, description, health, attack, defense) {}
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
}