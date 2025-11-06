public class Food : Item
{
    public int HealthRestore { get; private set; }
    public Food(string name, string description, int value, int healthRestore) : base(name, description, value)
    {
        HealthRestore = healthRestore;
    }
    public override void Use(PlayerCharacter player)
    {
        player.Heal(HealthRestore);
    }
    public override Item Clone() => new Food(Name, Description, Value, HealthRestore);
}