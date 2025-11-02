public class Room
{
    public readonly int Width;
    public readonly int Height;
    public readonly string Description;
    public Tile[,] Tiles { get; private set; }
    public readonly bool[] Exits; // Indicates the walls that have exits, in the order: North, East, South, West

    public Room(int width, int height, string description, bool[] exits)
    {
        Width = width;
        Height = height;
        Description = description;
        Exits = exits;
        Tiles = new Tile[height, width];
    }

    private void PopulateTiles()
    {
        // Initialize all tiles in the room
        // Use the following probabilities for different tile types:
        // 60% EMPTY, 20% OBSTACLE, 10% CHARACTER, 6% CHEST, 4% TRAP
        for (int y = 0; y < Height; y++)
        {
            for (int x = 0; x < Width; x++)
            {
                int roll = Random.Shared.Next(100);
                if (roll < 60)
                {
                    Tiles[y, x] = new Tile(ObjectType.EMPTY, null);
                }
                else if (roll < 80)
                {
                    Tiles[y, x] = new Tile(ObjectType.OBSTACLE, new object());
                }
                else if (roll < 90)
                {
                    Tiles[y, x] = new Tile(ObjectType.CHARACTER, CharacterPool[Random.Shared.Next(CharacterPool.Length)].Clone());
                }
                else if (roll < 96)
                {
                    Tiles[y, x] = new Tile(ObjectType.CHEST, new Chest());
                }
                else
                {
                    Tiles[y, x] = new Tile(ObjectType.TRAP, new Trap(Random.Shared.Next(5, 30)));
                }
            }
        }
    }

    private readonly NonPlayerCharacter[] CharacterPool = new NonPlayerCharacter[]
    {
        new NonPlayerCharacter("Goblin", "A sneaky goblin", 30, 5, 2, false),
        new NonPlayerCharacter("Orc", "A brutish orc", 50, 10, 5, false),
        new NonPlayerCharacter("Troll", "A large troll", 80, 15, 8, false),
        new NonPlayerCharacter("Ent", "A towering ent", 100, 12, 10, false),
        new NonPlayerCharacter("Treant", "A massive tree-like creature", 120, 10, 12, false),
        new NonPlayerCharacter("Slime", "A gooey slime monster", 20, 4, 1, false),
        new NonPlayerCharacter("Skeleton", "A rattling skeleton", 25, 7, 3, true),
        new NonPlayerCharacter("Zombie", "A shambling zombie", 40, 8, 4, true),
        new NonPlayerCharacter("Bandit", "A cunning bandit", 35, 12, 3, false),
        new NonPlayerCharacter("Assassin", "A stealthy assassin", 30, 14, 2, false),
        new NonPlayerCharacter("Giant Spider", "A giant spider with venomous fangs", 40, 10, 5, true),
        new NonPlayerCharacter("Vampire", "A bloodthirsty vampire", 55, 18, 8, true),
        new NonPlayerCharacter("Werewolf", "A ferocious werewolf", 65, 22, 10, true),
        new NonPlayerCharacter("Lich", "An ancient lich wielding dark magic", 90, 30, 12, true),
        new NonPlayerCharacter("Banshee", "A wailing banshee that drains life", 50, 15, 5, true),
        new NonPlayerCharacter("Mummy", "A cursed mummy wrapped in bandages", 60, 12, 6, true)
    };
}