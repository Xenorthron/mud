public class StartMenu : Menu
{
    private const string SaveFileName = "savegame.json";

    public void Display()
    {
        Console.Clear();
        Console.WriteLine("Welcome to MUD, the Multi-User Dungeon!");
        Console.WriteLine("1. Start New Game");
        Console.WriteLine("2. Load Game");
        Console.WriteLine("3. Exit");
        HandleInput();
    }

    public void HandleInput()
    {
        ConsoleKeyInfo keyInfo = Console.ReadKey(true);
        while (keyInfo.Key != ConsoleKey.D1 && keyInfo.Key != ConsoleKey.D2 && keyInfo.Key != ConsoleKey.D3)
        {
            Console.WriteLine("Invalid selection. Please try again.");
            keyInfo = Console.ReadKey(true);
        }
        switch (keyInfo.Key)
        {
            case ConsoleKey.D1:
                StartNewGame();
                break;
            case ConsoleKey.D2:
                LoadGame();
                break;
            case ConsoleKey.D3:
                Console.WriteLine("Exiting...");
                Environment.Exit(0);
                break;
            default:
                Console.WriteLine("Invalid selection. Please try again.");
                break;
        }
    }

    private void StartNewGame()
    {
        Console.Clear();
        Console.WriteLine("=== Create Your Character ===");
        Console.Write("Enter your character's name: ");
        string? name = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(name))
        {
            name = "Adventurer";
        }

        Console.Write("Enter your character's description: ");
        string? description = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(description))
        {
            description = "A brave adventurer";
        }

        // Ask for day/night cycle configuration
        Console.Write("\nTurns per day/night cycle (default 10): ");
        string? cycleInput = Console.ReadLine();
        int turnsPerCycle = 10;
        if (!string.IsNullOrWhiteSpace(cycleInput) && int.TryParse(cycleInput, out int parsed))
        {
            turnsPerCycle = parsed;
        }

        var player = new PlayerCharacter(name, description);
        var map = Map.CreateDefaultMap();
        var gameController = new GameController(player, map, map.StartRoomIndex)
        {
            TurnsPerCycle = turnsPerCycle
        };

        var gameMenu = new GameMenu(gameController);
        gameMenu.Display();

        // After game ends, return to main menu
        Display();
    }

    private void LoadGame()
    {
        Console.Clear();
        if (!File.Exists(SaveFileName))
        {
            Console.WriteLine("No saved game found!");
            Console.WriteLine("Press any key to return to main menu...");
            Console.ReadKey(true);
            Display();
            return;
        }

        Console.WriteLine("Loading game...");
        var map = Map.CreateDefaultMap();
        var gameController = GameController.Load(SaveFileName, map);

        if (gameController == null)
        {
            Console.WriteLine("Failed to load game!");
            Console.WriteLine("Press any key to return to main menu...");
            Console.ReadKey(true);
            Display();
            return;
        }

        Console.WriteLine("Game loaded successfully!");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey(true);

        var gameMenu = new GameMenu(gameController);
        gameMenu.Display();

        // After game ends, return to main menu
        Display();
    }
}