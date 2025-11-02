public abstract class Character
{
    public required string Name { get; init; }
    public required string Description { get; init; }
    public required int Health { get; set; }
    public required int Attack { get; init; }
    public required int Defense { get; init; }
    public abstract void AttackTarget(Character target);
    public bool IsAlive()
    {
        return Health > 0;
    }
}