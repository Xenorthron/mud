public class Trap
{
    public int Attack { get; private set; }
    public bool IsArmed { get; private set; } = true;
    public bool IsDetected { get; private set; } = false;
    public Trap(int attack)
    {
        Attack = attack;
    }
    public bool AttemptDetect()
    {
        if (Random.Shared.Next(0, 1) == 0) // 50% chance to detect
        {
            IsDetected = true;
            return true;
        }
        return false;
    }
    public bool AttemptDisarm(PlayerCharacter player)
    {
        if (Random.Shared.Next(0, 1) == 0) // 50% chance to disarm
        {
            IsArmed = false;
            return true;
        }
        Trigger(player);
        return false;
    }
    public void Trigger(PlayerCharacter player)
    {
        if (IsArmed)
        {
            var damage = Attack - (player.Armor?.DefenseBonus ?? 0);
            player.Health -= (damage > 0) ? damage : 1; // Minimum damage of 1
            IsArmed = false; // Trap is triggered and disarmed
        }
    }
}