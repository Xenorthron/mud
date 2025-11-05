public class StartMenu : Menu
{
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
                Console.WriteLine("Starting a new game...");
                // Logic to start a new game
                break;
            case ConsoleKey.D2:
                Console.WriteLine("Loading game...");
                // Logic to load a game
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
}