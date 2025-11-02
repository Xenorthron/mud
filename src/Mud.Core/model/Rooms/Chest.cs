public class Chest
{
    public Item[] Items { get; set; }
    private Item[] ItemPool = new Item[]
    {
        new Weapon("Common Sword", "A normal looking sword.", 5, 5),
        new Armor("Common Armor", "A normal set of armor.", 5, 5),
        new Food("Apple", "A fresh red apple.", 5, 15),
        new Food("Bread Loaf", "A loaf of freshly baked bread.", 5, 15),
        new Food("Cheese Wedge", "A wedge of aged cheese.", 5, 15),

        new Weapon("Rare Sword", "A finely crafted sword with intricate designs.", 20, 15),
        new Armor("Rare Armor", "A sturdy set of armor made from reinforced materials.", 20, 15),
        new Food("Roasted Chicken", "A delicious roasted chicken leg.", 15, 30),
        new Food("Grilled Salmon", "A perfectly grilled salmon fillet.", 15, 30),

        new Weapon("Epic Sword", "A sword of epic proportions, radiating power.", 50, 30),
        new Armor("Epic Armor", "Armor worn by the greatest heroes, offering powerful protection.", 50, 30),
        new Food("Hearty Stew", "A bowl of warm, hearty stew.", 30, 50),

        new Weapon("Legendary Sword", "A sword of legends, said to be wielded by heroes of old.", 100, 50),
        new Armor("Legendary Armor", "Armor that has withstood the test of time, offering unparalleled protection.", 100, 50),
        new Food("Feast Platter", "An extravagant platter filled with a variety of delicacies.", 50, 75),

        new Weapon("Mythical Sword", "A sword imbued with mystical powers, glowing with an ethereal light.", 200, 75),
        new Armor("Mythical Armor", "Armor forged in ancient times, rumored to be indestructible.", 200, 75),
        new Food("Ambrosia", "The food of the gods, said to grant immortality to those who consume it.", 100, 100)
    };

    public Chest()
    {
        Items = new Item[Random.Shared.Next(1, 6)];
    }
}