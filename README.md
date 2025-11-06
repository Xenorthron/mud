# M.U.D. (Multi-User Dungeon)

A text-based dungeon crawler game implemented in C# for the .NET 9.0 platform. This project was created as an extra personal project for SWEN-262.

## Features

### Character System
- **Player Character (PC)**
  - Starting stats: 100 Health, 10 Attack, 0 Defense
  - Customizable name and description
  - Inventory system with up to 6 bags
  - Equipment slots for weapon and armor
  - Gold collection from defeated enemies

- **Non-Player Characters (NPCs)**
  - Health: 50-150
  - Attack: 5-15
  - Defense: 0-10
  - Nocturnal or diurnal behavior affecting stats
  - Drop gold when defeated

### Combat System
- Damage calculation: Attacker's Attack - Defender's Defense (minimum 1 damage)
- Turn-based combat
- Equipment modifiers for attack and defense
- Defeat enemies to collect gold

### Items
- **Weapons**: Increase attack stat
- **Armor**: Increase defense stat
- **Food**: Restore health (capped at 100)
- **Buff Potions**: Temporarily increase stats for 10 turns
- **Bags**: Store items (6 slots per bag, up to 6 bags total)

### World
- **Rooms**: Grid-based tiles (3x3 feet each)
- **Tiles**: Can contain:
  - Empty space
  - Obstacles (impassable)
  - Enemies (NPCs)
  - Chests (containing 1-5 items)
  - Traps (50% auto-detection when adjacent)
- **Map**: Connected rooms with a start and goal

### Day/Night Cycle
- Configurable turn duration (default: 10 turns per cycle)
- Diurnal NPCs: +10% stats during day, -10% at night
- Nocturnal NPCs: -20% stats during day, +20% at night

### Gameplay
- Move in cardinal directions
- Attack adjacent enemies (including diagonally)
- Loot chests
- Detect and disarm traps
- Navigate between rooms via exits
- Save and load game progress
- Win by reaching the goal room

## Building and Running

### Prerequisites
- .NET 9.0 SDK

### Build
```bash
dotnet build
```

### Run
```bash
dotnet run --project src/Mud/Mud.csproj
```

### Test
```bash
dotnet test
```

## Controls

### Main Menu
- `1` - Start New Game
- `2` - Load Game
- `3` - Exit

### In-Game
- **Movement**: W/A/S/D (Up/Left/Down/Right)
- **Attack**: 
  - Q/E/Z/C (Diagonal attacks)
  - T/F/G/H (Cardinal direction attacks)
- **Exits**: 1=North, 2=East, 3=South, 4=West
- **Actions**:
  - `I` - Open Inventory
  - `L` - Loot Chest (when on chest tile)
  - `R` - Detect Trap (select direction)
  - `U` - Disarm Trap (select direction)
  - `X` - Save and Quit
  - `V` - Victory (when at goal)

### Inventory
- `U X.Y` - Use item (e.g., "U 0.0" for first item in first bag)
- `D X.Y` - Destroy item to free up space
- `B` - Back to game

## Game Map Legend
- `P` - Player
- `E` - Enemy (NPC)
- `C` - Chest
- `^` - Trap (detected)
- `X` - Obstacle
- `#` - Empty tile

## Configuration

When starting a new game, you can configure:
- Character name
- Character description
- Turns per day/night cycle (default: 10)

## Save System

Game progress is automatically saved to `savegame.json` when you choose "Save and Quit" from the game menu. You can resume your progress by selecting "Load Game" from the main menu.

## Architecture

The project is structured into three main components:

1. **Mud.Core** - Core game logic
   - Model: Characters, Items, Rooms, Inventory
   - View: Menu system (StartMenu, GameMenu)
   - Controller: GameController, Map

2. **Mud** - Entry point application
   - Program.cs initializes and starts the game

3. **Mud.Tests** - Unit tests
   - Comprehensive tests for all game systems

## Testing

The project includes 18 comprehensive unit tests covering:
- Character creation and stats
- Combat mechanics
- Item usage and equipment
- Inventory management
- Day/night cycle
- Trap detection
- Buff system
- Food healing

Run tests with:
```bash
dotnet test
```

## License

This is an educational project for SWEN-262.
