public class GameMenu : Menu
{
    private GameController? gameController;

    public GameMenu(GameController controller)
    {
        gameController = controller;
    }

    public void Display()
    {
        while (gameController != null && !gameController.IsGameOver())
        {
            Console.Clear();
            
            // Display game state
            Console.WriteLine("=== M.U.D. Game ===");
            Console.WriteLine($"Time of Day: {(gameController.IsDay ? "Day" : "Night")}");
            Console.WriteLine($"Turn: {gameController.TurnCount}");
            Console.WriteLine($"Health: {gameController.Player.Health}/100");
            Console.WriteLine($"Attack: {gameController.Player.Attack} | Defense: {gameController.Player.Defense}");
            Console.WriteLine();
            
            // Display room description
            Console.WriteLine(gameController.GetRoomDescription());
            Console.WriteLine();
            
            // Display room map
            Console.WriteLine("Room Map:");
            Console.WriteLine(gameController.GetRoomMap());
            Console.WriteLine("Legend: P=Player, E=Enemy, C=Chest, ^=Trap(detected), X=Obstacle, #=Empty");
            Console.WriteLine();

            // Check if at goal
            if (gameController.IsAtGoal())
            {
                Console.WriteLine("*** You have reached the goal! ***");
                Console.WriteLine("Press 'V' to win and exit, or continue exploring.");
            }

            // Display menu options
            Console.WriteLine("\n=== Actions ===");
            Console.WriteLine("Movement: W/A/S/D (Up/Left/Down/Right)");
            Console.WriteLine("Attack: Q/E/Z/C (Diagonal), T/F/G/H (Adjacent)");
            Console.WriteLine("Exits: 1=North, 2=East, 3=South, 4=West");
            Console.WriteLine("Other: I=Inventory, L=Loot Chest, R=Detect Trap, U=Disarm Trap");
            Console.WriteLine("       X=Save and Quit");
            
            HandleInput();
        }

        // Game over
        Console.Clear();
        if (gameController == null || !gameController.Player.IsAlive())
        {
            Console.WriteLine("=== GAME OVER ===");
            Console.WriteLine("You have been defeated!");
        }
        else if (gameController.IsAtGoal())
        {
            Console.WriteLine("=== VICTORY! ===");
            Console.WriteLine("You have completed the dungeon!");
        }
        else
        {
            Console.WriteLine("Thanks for playing!");
        }
        
        Console.WriteLine("\nPress any key to return to main menu...");
        Console.ReadKey();
    }

    public void HandleInput()
    {
        var key = Console.ReadKey(true);
        bool validAction = false;

        switch (key.Key)
        {
            // Movement
            case ConsoleKey.W:
                validAction = gameController!.MovePlayer(0, -1);
                break;
            case ConsoleKey.S:
                validAction = gameController!.MovePlayer(0, 1);
                break;
            case ConsoleKey.A:
                validAction = gameController!.MovePlayer(-1, 0);
                break;
            case ConsoleKey.D:
                validAction = gameController!.MovePlayer(1, 0);
                break;

            // Attack (using different keys for 8 directions)
            case ConsoleKey.Q: // Up-Left
                validAction = gameController!.AttackNPC(-1, -1);
                break;
            case ConsoleKey.E: // Up-Right
                validAction = gameController!.AttackNPC(1, -1);
                break;
            case ConsoleKey.Z: // Down-Left
                validAction = gameController!.AttackNPC(-1, 1);
                break;
            case ConsoleKey.C: // Down-Right
                validAction = gameController!.AttackNPC(1, 1);
                break;
            case ConsoleKey.T: // Up
                validAction = gameController!.AttackNPC(0, -1);
                break;
            case ConsoleKey.G: // Down
                validAction = gameController!.AttackNPC(0, 1);
                break;
            case ConsoleKey.F: // Left
                validAction = gameController!.AttackNPC(-1, 0);
                break;
            case ConsoleKey.H: // Right
                validAction = gameController!.AttackNPC(1, 0);
                break;

            // Exits
            case ConsoleKey.D1:
            case ConsoleKey.NumPad1:
                validAction = gameController!.UseExit(0); // North
                break;
            case ConsoleKey.D2:
            case ConsoleKey.NumPad2:
                validAction = gameController!.UseExit(1); // East
                break;
            case ConsoleKey.D3:
            case ConsoleKey.NumPad3:
                validAction = gameController!.UseExit(2); // South
                break;
            case ConsoleKey.D4:
            case ConsoleKey.NumPad4:
                validAction = gameController!.UseExit(3); // West
                break;

            // Inventory
            case ConsoleKey.I:
                ShowInventory();
                return; // Don't end turn

            // Loot chest
            case ConsoleKey.L:
                validAction = gameController!.OpenChest();
                if (validAction)
                {
                    Console.WriteLine("\nYou looted the chest!");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey(true);
                }
                break;

            // Detect trap
            case ConsoleKey.R:
                Console.WriteLine("\nWhich direction? (W/A/S/D)");
                var dir = Console.ReadKey(true);
                int dx = 0, dy = 0;
                if (dir.Key == ConsoleKey.W) dy = -1;
                else if (dir.Key == ConsoleKey.S) dy = 1;
                else if (dir.Key == ConsoleKey.A) dx = -1;
                else if (dir.Key == ConsoleKey.D) dx = 1;
                
                if (gameController!.DetectTrap(dx, dy))
                {
                    Console.WriteLine("You detected a trap!");
                    validAction = true;
                }
                else
                {
                    Console.WriteLine("No trap detected or detection failed.");
                }
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey(true);
                break;

            // Disarm trap
            case ConsoleKey.U:
                Console.WriteLine("\nWhich direction? (W/A/S/D)");
                var dirD = Console.ReadKey(true);
                int dxD = 0, dyD = 0;
                if (dirD.Key == ConsoleKey.W) dyD = -1;
                else if (dirD.Key == ConsoleKey.S) dyD = 1;
                else if (dirD.Key == ConsoleKey.A) dxD = -1;
                else if (dirD.Key == ConsoleKey.D) dxD = 1;
                
                if (gameController!.DisarmTrap(dxD, dyD))
                {
                    Console.WriteLine("You successfully disarmed the trap!");
                    validAction = true;
                }
                else
                {
                    Console.WriteLine("Disarm failed or no trap there.");
                }
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey(true);
                break;

            // Save and quit
            case ConsoleKey.X:
                Console.WriteLine("\nSaving game...");
                gameController!.Save("savegame.json");
                Console.WriteLine("Game saved!");
                gameController.EndGame();
                return;

            // Win condition
            case ConsoleKey.V when gameController!.IsAtGoal():
                gameController!.EndGame();
                return;

            default:
                Console.WriteLine("\nInvalid action!");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey(true);
                return; // Don't end turn for invalid actions
        }

        if (!validAction && key.Key != ConsoleKey.I)
        {
            Console.WriteLine("\nCannot perform that action!");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
            return; // Don't end turn if action failed
        }

        // End turn
        if (key.Key != ConsoleKey.I)
        {
            gameController!.EndTurn();
        }
    }

    private void ShowInventory()
    {
        Console.Clear();
        Console.WriteLine("=== Inventory ===");
        Console.WriteLine($"Player: {gameController!.Player.Name}");
        Console.WriteLine($"Health: {gameController.Player.Health}/100");
        Console.WriteLine();

        // Show equipped items
        Console.WriteLine("Equipped Weapon: " + (gameController.Player.Weapon?.Name ?? "None"));
        Console.WriteLine("Equipped Armor: " + (gameController.Player.Armor?.Name ?? "None"));
        Console.WriteLine();

        // Show bags
        var inventory = gameController.Player.Inventory;
        var ratio = inventory.GetUsedSlotsRatio();
        Console.WriteLine($"Inventory: {ratio[0]}/{ratio[1]} slots used");
        Console.WriteLine($"Total Value: {inventory.GetValue()} gold");
        Console.WriteLine();

        for (int i = 0; i < inventory.Contents.Length; i++)
        {
            var bag = inventory.Contents[i];
            if (bag != null)
            {
                var bagRatio = bag.GetUsedSlotsRatio();
                Console.WriteLine($"Bag {i + 1}: {bag.Name} ({bagRatio[0]}/{bagRatio[1]} slots, {bag.GetValue()} gold)");
                for (int j = 0; j < bag.Contents.Length; j++)
                {
                    var item = bag.Contents[j];
                    if (item != null)
                    {
                        Console.WriteLine($"  [{i}.{j}] {item.Name} - {item.Description} (Value: {item.Value})");
                    }
                }
            }
        }

        Console.WriteLine("\n=== Actions ===");
        Console.WriteLine("U X.Y - Use item (e.g., U 0.0)");
        Console.WriteLine("D X.Y - Destroy item");
        Console.WriteLine("B - Back to game");

        var input = Console.ReadLine();
        if (input != null && input.Length > 0)
        {
            if (input.ToUpper() == "B")
            {
                return;
            }
            else if (input.ToUpper().StartsWith("U "))
            {
                var parts = input.Substring(2).Split('.');
                if (parts.Length == 2 && int.TryParse(parts[0], out int bagIdx) && int.TryParse(parts[1], out int itemIdx))
                {
                    inventory.UseItem(bagIdx, itemIdx, gameController.Player);
                    Console.WriteLine("Item used!");
                }
            }
            else if (input.ToUpper().StartsWith("D "))
            {
                var parts = input.Substring(2).Split('.');
                if (parts.Length == 2 && int.TryParse(parts[0], out int bagIdx) && int.TryParse(parts[1], out int itemIdx))
                {
                    if (bagIdx < inventory.Contents.Length && inventory.Contents[bagIdx] != null)
                    {
                        inventory.Contents[bagIdx].Contents[itemIdx] = null;
                        Console.WriteLine("Item destroyed!");
                    }
                }
            }
        }

        Console.WriteLine("Press any key to continue...");
        Console.ReadKey(true);
        ShowInventory(); // Re-display inventory
    }
}