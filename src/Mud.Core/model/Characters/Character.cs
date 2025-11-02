public abstract class Character
{
    public string Name { get; init; }
    public string Description { get; init; }
    public int Health { get; set; }
    public int Attack { get; set; }
    public int Defense { get; set; }

    public Character(string name, string description, int health, int attack, int defense)
    {
        Name = name;
        Description = description + " with " + health + " health, " + attack + " attack, and " + defense + " defense.";
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