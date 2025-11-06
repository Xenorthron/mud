using System.Text.Json;

public class GameController
{
    public PlayerCharacter Player { get; private set; }
    public Map GameMap { get; private set; }
    public Room CurrentRoom { get; private set; }
    public int PlayerX { get; private set; }
    public int PlayerY { get; private set; }
    public bool IsDay { get; private set; } = true;
    public int TurnCount { get; private set; } = 0;
    public int TurnsPerCycle { get; set; } = 10;
    private bool gameOver = false;

    public GameController(PlayerCharacter player, Map map, int startRoomIndex = 0)
    {
        Player = player;
        GameMap = map;
        CurrentRoom = map.Rooms[startRoomIndex];
        
        // Find an empty tile to place the player
        for (int y = 0; y < CurrentRoom.Height; y++)
        {
            for (int x = 0; x < CurrentRoom.Width; x++)
            {
                if (CurrentRoom.Tiles[y, x].Type == ObjectType.EMPTY)
                {
                    PlayerX = x;
                    PlayerY = y;
                    return;
                }
            }
        }
    }

    public bool IsGameOver()
    {
        return gameOver;
    }

    public void EndTurn()
    {
        TurnCount++;
        Player.GameTick();

        // Check day/night cycle
        if (TurnCount % TurnsPerCycle == 0)
        {
            ToggleDayNight();
        }

        // NPCs attack player if adjacent
        AttackPlayerFromAdjacentNPCs();
    }

    private void ToggleDayNight()
    {
        IsDay = !IsDay;
        
        // Update all NPCs in the current room
        for (int y = 0; y < CurrentRoom.Height; y++)
        {
            for (int x = 0; x < CurrentRoom.Width; x++)
            {
                var tile = CurrentRoom.Tiles[y, x];
                if (tile.Type == ObjectType.CHARACTER && tile.Object is NonPlayerCharacter npc)
                {
                    if (IsDay)
                    {
                        npc.SwitchToDay();
                    }
                    else
                    {
                        npc.SwitchToNight();
                    }
                }
            }
        }
    }

    private void AttackPlayerFromAdjacentNPCs()
    {
        for (int dy = -1; dy <= 1; dy++)
        {
            for (int dx = -1; dx <= 1; dx++)
            {
                if (dx == 0 && dy == 0) continue;

                int checkX = PlayerX + dx;
                int checkY = PlayerY + dy;

                if (IsValidPosition(checkX, checkY))
                {
                    var tile = CurrentRoom.Tiles[checkY, checkX];
                    if (tile.Type == ObjectType.CHARACTER && tile.Object is NonPlayerCharacter npc && npc.IsAlive())
                    {
                        npc.AttackTarget(Player);
                        if (!Player.IsAlive())
                        {
                            gameOver = true;
                            return;
                        }
                    }
                }
            }
        }
    }

    public bool MovePlayer(int dx, int dy)
    {
        int newX = PlayerX + dx;
        int newY = PlayerY + dy;

        if (!IsValidPosition(newX, newY))
        {
            return false;
        }

        var targetTile = CurrentRoom.Tiles[newY, newX];

        // Check if tile is passable
        if (targetTile.Type == ObjectType.OBSTACLE || targetTile.Type == ObjectType.CHARACTER)
        {
            return false;
        }

        // Check for trap
        if (targetTile.Type == ObjectType.TRAP && targetTile.Object is Trap trap)
        {
            if (!trap.IsDetected)
            {
                trap.Trigger(Player);
                if (!Player.IsAlive())
                {
                    gameOver = true;
                }
            }
            else if (trap.IsArmed)
            {
                trap.Trigger(Player);
                if (!Player.IsAlive())
                {
                    gameOver = true;
                }
            }
        }

        PlayerX = newX;
        PlayerY = newY;

        // Auto-detect adjacent traps (50% chance)
        DetectAdjacentTraps();

        return true;
    }

    private void DetectAdjacentTraps()
    {
        for (int dy = -1; dy <= 1; dy++)
        {
            for (int dx = -1; dx <= 1; dx++)
            {
                if (dx == 0 && dy == 0) continue;

                int checkX = PlayerX + dx;
                int checkY = PlayerY + dy;

                if (IsValidPosition(checkX, checkY))
                {
                    var tile = CurrentRoom.Tiles[checkY, checkX];
                    if (tile.Type == ObjectType.TRAP && tile.Object is Trap trap && !trap.IsDetected)
                    {
                        trap.AttemptDetect();
                    }
                }
            }
        }
    }

    public bool AttackNPC(int dx, int dy)
    {
        int targetX = PlayerX + dx;
        int targetY = PlayerY + dy;

        if (!IsValidPosition(targetX, targetY))
        {
            return false;
        }

        var targetTile = CurrentRoom.Tiles[targetY, targetX];
        if (targetTile.Type == ObjectType.CHARACTER && targetTile.Object is NonPlayerCharacter npc)
        {
            Player.AttackTarget(npc);
            if (!npc.IsAlive())
            {
                // NPC defeated, drop gold
                int goldDrop = Random.Shared.Next(5, 50);
                Player.Gold += goldDrop;
                
                // NPC defeated, remove from tile
                targetTile.SetObject(ObjectType.EMPTY, null);
            }
            return true;
        }

        return false;
    }

    public bool OpenChest()
    {
        var tile = CurrentRoom.Tiles[PlayerY, PlayerX];
        if (tile.Type == ObjectType.CHEST && tile.Object is Chest chest)
        {
            foreach (var item in chest.Loot)
            {
                if (item != null)
                {
                    Player.Inventory.AddItem(item);
                }
            }
            tile.SetObject(ObjectType.EMPTY, null);
            return true;
        }
        return false;
    }

    public bool DetectTrap(int dx, int dy)
    {
        int targetX = PlayerX + dx;
        int targetY = PlayerY + dy;

        if (!IsValidPosition(targetX, targetY))
        {
            return false;
        }

        var tile = CurrentRoom.Tiles[targetY, targetX];
        if (tile.Type == ObjectType.TRAP && tile.Object is Trap trap)
        {
            return trap.AttemptDetect();
        }

        return false;
    }

    public bool DisarmTrap(int dx, int dy)
    {
        int targetX = PlayerX + dx;
        int targetY = PlayerY + dy;

        if (!IsValidPosition(targetX, targetY))
        {
            return false;
        }

        var tile = CurrentRoom.Tiles[targetY, targetX];
        if (tile.Type == ObjectType.TRAP && tile.Object is Trap trap && trap.IsDetected)
        {
            return trap.AttemptDisarm(Player);
        }

        return false;
    }

    public bool UseExit(int direction)
    {
        // direction: 0=North, 1=East, 2=South, 3=West
        if (!CurrentRoom.Exits[direction])
        {
            return false;
        }

        var nextRoom = GameMap.GetConnectedRoom(CurrentRoom, direction);
        if (nextRoom != null)
        {
            CurrentRoom = nextRoom;
            
            // Place player near the opposite entrance
            int oppositeDirection = (direction + 2) % 4;
            PlacePlayerNearExit(oppositeDirection);

            // Update NPCs for day/night
            if (IsDay)
            {
                UpdateRoomNPCs(npc => npc.SwitchToDay());
            }
            else
            {
                UpdateRoomNPCs(npc => npc.SwitchToNight());
            }

            return true;
        }

        return false;
    }

    private void UpdateRoomNPCs(Action<NonPlayerCharacter> action)
    {
        for (int y = 0; y < CurrentRoom.Height; y++)
        {
            for (int x = 0; x < CurrentRoom.Width; x++)
            {
                var tile = CurrentRoom.Tiles[y, x];
                if (tile.Type == ObjectType.CHARACTER && tile.Object is NonPlayerCharacter npc)
                {
                    action(npc);
                }
            }
        }
    }

    private void PlacePlayerNearExit(int exitDirection)
    {
        // Place player near the specified exit
        int x = CurrentRoom.Width / 2;
        int y = CurrentRoom.Height / 2;

        switch (exitDirection)
        {
            case 0: // North
                y = 0;
                break;
            case 1: // East
                x = CurrentRoom.Width - 1;
                break;
            case 2: // South
                y = CurrentRoom.Height - 1;
                break;
            case 3: // West
                x = 0;
                break;
        }

        // Find nearest empty tile
        for (int radius = 0; radius < Math.Max(CurrentRoom.Width, CurrentRoom.Height); radius++)
        {
            for (int dy = -radius; dy <= radius; dy++)
            {
                for (int dx = -radius; dx <= radius; dx++)
                {
                    int checkX = x + dx;
                    int checkY = y + dy;
                    if (IsValidPosition(checkX, checkY) && CurrentRoom.Tiles[checkY, checkX].Type == ObjectType.EMPTY)
                    {
                        PlayerX = checkX;
                        PlayerY = checkY;
                        return;
                    }
                }
            }
        }

        // Fallback: just place at the center
        PlayerX = CurrentRoom.Width / 2;
        PlayerY = CurrentRoom.Height / 2;
    }

    public bool IsAtGoal()
    {
        return GameMap.IsGoalRoom(CurrentRoom);
    }

    public void EndGame()
    {
        gameOver = true;
    }

    private bool IsValidPosition(int x, int y)
    {
        return x >= 0 && x < CurrentRoom.Width && y >= 0 && y < CurrentRoom.Height;
    }

    public string GetRoomDescription()
    {
        var description = CurrentRoom.Description + "\n\nExits: ";
        List<string> exitsList = new List<string>();
        if (CurrentRoom.Exits[0]) exitsList.Add("North");
        if (CurrentRoom.Exits[1]) exitsList.Add("East");
        if (CurrentRoom.Exits[2]) exitsList.Add("South");
        if (CurrentRoom.Exits[3]) exitsList.Add("West");
        description += string.Join(", ", exitsList);

        description += "\n\nIn the room, you see:\n";
        for (int y = 0; y < CurrentRoom.Height; y++)
        {
            for (int x = 0; x < CurrentRoom.Width; x++)
            {
                var tile = CurrentRoom.Tiles[y, x];
                if (tile.Type == ObjectType.CHARACTER && tile.Object is NonPlayerCharacter npc)
                {
                    description += $"- {npc.Name} ({npc.Health} health, {npc.Attack} attack, {npc.Defense} defense)\n";
                }
                else if (tile.Type == ObjectType.CHEST)
                {
                    description += "- A chest\n";
                }
                else if (tile.Type == ObjectType.TRAP && tile.Object is Trap trap && trap.IsDetected)
                {
                    description += "- A trap (detected)\n";
                }
                else if (tile.Type == ObjectType.OBSTACLE)
                {
                    description += "- An obstacle\n";
                }
            }
        }

        return description;
    }

    public string GetRoomMap()
    {
        var map = new System.Text.StringBuilder();
        for (int y = 0; y < CurrentRoom.Height; y++)
        {
            for (int x = 0; x < CurrentRoom.Width; x++)
            {
                if (x == PlayerX && y == PlayerY)
                {
                    map.Append('P');
                }
                else
                {
                    map.Append(CurrentRoom.Tiles[y, x].Symbol);
                }
            }
            map.AppendLine();
        }
        return map.ToString();
    }

    public void Save(string filename)
    {
        var saveData = new SaveData
        {
            Player = Player,
            CurrentRoomIndex = Array.IndexOf(GameMap.Rooms, CurrentRoom),
            PlayerX = PlayerX,
            PlayerY = PlayerY,
            IsDay = IsDay,
            TurnCount = TurnCount,
            TurnsPerCycle = TurnsPerCycle
        };

        var json = JsonSerializer.Serialize(saveData, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(filename, json);
    }

    public static GameController? Load(string filename, Map map)
    {
        if (!File.Exists(filename))
        {
            return null;
        }

        var json = File.ReadAllText(filename);
        var saveData = JsonSerializer.Deserialize<SaveData>(json);

        if (saveData == null || saveData.Player == null)
        {
            return null;
        }

        var controller = new GameController(saveData.Player, map, saveData.CurrentRoomIndex)
        {
            PlayerX = saveData.PlayerX,
            PlayerY = saveData.PlayerY,
            IsDay = saveData.IsDay,
            TurnCount = saveData.TurnCount,
            TurnsPerCycle = saveData.TurnsPerCycle
        };

        return controller;
    }
}

public class SaveData
{
    public PlayerCharacter? Player { get; set; }
    public int CurrentRoomIndex { get; set; }
    public int PlayerX { get; set; }
    public int PlayerY { get; set; }
    public bool IsDay { get; set; }
    public int TurnCount { get; set; }
    public int TurnsPerCycle { get; set; }
}
