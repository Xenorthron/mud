using System.Linq;

public class Chest
{
    public Item[] Loot { get; set; }
    private Item[] ItemPool = new Item[]
    {
        // Common Items, 40% chance
        new Weapon("Common Sword", "A normal looking sword.", 5, 5),
        new Armor("Common Armor", "A normal set of armor.", 5, 5),
        new Food("Apple", "A fresh red apple.", 5, 15),
        new Food("Bread Loaf", "A loaf of freshly baked bread.", 5, 15),
        new Food("Cheese Wedge", "A wedge of aged cheese.", 5, 15),

        // Rare Items, 30% chance
        new Weapon("Rare Sword", "A finely crafted sword with intricate designs.", 20, 15),
        new Armor("Rare Armor", "A sturdy set of armor made from reinforced materials.", 20, 15),
        new Food("Roasted Chicken", "A delicious roasted chicken leg.", 15, 30),
        new Food("Grilled Salmon", "A perfectly grilled salmon fillet.", 15, 30),

        // Epic Items, 15% chance
        new Weapon("Epic Sword", "A sword of epic proportions, radiating power.", 50, 30),
        new Armor("Epic Armor", "Armor worn by the greatest heroes, offering powerful protection.", 50, 30),
        new Food("Hearty Stew", "A bowl of warm, hearty stew.", 30, 50),

        // Legendary Items, 10% chance
        new Weapon("Legendary Sword", "A sword of legends, said to be wielded by heroes of old.", 100, 50),
        new Armor("Legendary Armor", "Armor that has withstood the test of time, offering unparalleled protection.", 100, 50),
        new Food("Feast Platter", "An extravagant platter filled with a variety of delicacies.", 50, 75),

        // Mythical Items, 5% chance
        new Weapon("Mythical Sword", "A sword imbued with mystical powers, glowing with an ethereal light.", 200, 75),
        new Armor("Mythical Armor", "Armor forged in ancient times, rumored to be indestructible.", 200, 75),
        new Food("Ambrosia", "The food of the gods, said to grant immortality to those who consume it.", 100, 100)
    };

    public Chest()
    {
        Loot = new Item[Random.Shared.Next(1, 6)];
        GenerateLoot();
    }

    private void GenerateLoot()
    {
        for (int i = 0; i < Loot.Length; i++)
        {
            int roll = Random.Shared.Next(1, 101);

            // Determine pool range (start inclusive, end exclusive)
            int start, end;
            if (roll <= 40) { start = 0; end = 5; }
            else if (roll <= 70) { start = 5; end = 9; }
            else if (roll <= 85) { start = 9; end = 12; }
            else if (roll <= 95) { start = 12; end = 15; }
            else { start = 15; end = 18; }

            var possible = Enumerable.Range(start, end - start).ToList();

            // Prefer candidates that don't create duplicate Weapon/Armor entries in Loot
            var filtered = possible.Where(idx =>
                // keep the candidate if it's Food (duplicates allowed),
                // or if it's Weapon/Armor and not already present in Loot
                !(ItemPool[idx] is Weapon || ItemPool[idx] is Armor) || !Loot.Contains(ItemPool[idx])
            ).ToList();

            int chosenIndex;
            if (filtered.Count > 0)
            {
                chosenIndex = filtered[Random.Shared.Next(filtered.Count)];
            }
            else
            {
                // No non-duplicate candidate in this rarity range. Try global pool
                var global = Enumerable.Range(0, ItemPool.Length)
                    .Where(idx => !(ItemPool[idx] is Weapon || ItemPool[idx] is Armor) || !Loot.Contains(ItemPool[idx]))
                    .ToList();

                if (global.Count > 0)
                {
                    chosenIndex = global[Random.Shared.Next(global.Count)];
                }
                else
                {
                    // Last resort: allow duplicates from the original possible list
                    chosenIndex = possible[Random.Shared.Next(possible.Count)];
                }
            }

            Loot[i] = ItemPool[chosenIndex];
        }
    }
}