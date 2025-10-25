public abstract class Character
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public int Health { get; private set; }
    public int Attack { get; private set; }
    public int Defense { get; private set; }
    public void AttackTarget(Character target)
    {
        int damage = this.Attack - target.Defense;
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