public abstract class Character
{
    public string Name { get; init; }
    public string Description { get; init; }
    public int Health { get; set; }
    public int Attack { get; init; }
    public int Defense { get; init; }

    public Character(string name, string description, int health, int attack, int defense)
    {
        Name = name;
        Description = description;
        Health = health;
        Attack = attack;
        Defense = defense;
    }
    public abstract void AttackTarget(Character target);
    public bool IsAlive()
    {
        return Health > 0;
    }
}