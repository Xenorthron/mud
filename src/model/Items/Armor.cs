public class Armor : Item
{
    public int DefenseBonus { get; private set; }
    public Armor(string name, string description, int value, int defenseBonus)
    {
        Name = name;
        Description = description;
        Value = value;
        DefenseBonus = defenseBonus;
    }
}