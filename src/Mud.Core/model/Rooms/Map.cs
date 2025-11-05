public class Map
{
    public Room[] Rooms { get; private set; }
    private Dictionary<Room, Dictionary<int, Room>> connections;
    public int StartRoomIndex { get; private set; }
    public int GoalRoomIndex { get; private set; }

    public Map(Room[] rooms, int startRoomIndex, int goalRoomIndex)
    {
        Rooms = rooms;
        StartRoomIndex = startRoomIndex;
        GoalRoomIndex = goalRoomIndex;
        connections = new Dictionary<Room, Dictionary<int, Room>>();

        // Initialize empty connection dictionaries for each room
        foreach (var room in rooms)
        {
            connections[room] = new Dictionary<int, Room>();
        }
    }

    public void ConnectRooms(int roomIndex1, int direction1, int roomIndex2, int direction2)
    {
        var room1 = Rooms[roomIndex1];
        var room2 = Rooms[roomIndex2];

        connections[room1][direction1] = room2;
        connections[room2][direction2] = room1;
    }

    public Room? GetConnectedRoom(Room currentRoom, int direction)
    {
        if (connections.ContainsKey(currentRoom) && connections[currentRoom].ContainsKey(direction))
        {
            return connections[currentRoom][direction];
        }
        return null;
    }

    public bool IsGoalRoom(Room room)
    {
        return Array.IndexOf(Rooms, room) == GoalRoomIndex;
    }

    public static Map CreateDefaultMap()
    {
        // Create a simple 3-room dungeon
        var room1 = new Room(10, 10, "A dark entrance hall", new bool[] { false, true, false, false });
        room1.PopulateTiles();

        var room2 = new Room(12, 12, "A large chamber with high ceilings", new bool[] { true, false, true, true });
        room2.PopulateTiles();

        var room3 = new Room(8, 8, "The treasure room", new bool[] { false, false, false, true });
        room3.PopulateTiles();

        var rooms = new Room[] { room1, room2, room3 };
        var map = new Map(rooms, 0, 2);

        // Connect rooms
        // Room 0 (entrance) East -> Room 1 West
        map.ConnectRooms(0, 1, 1, 3);
        
        // Room 1 East -> Room 2 West
        map.ConnectRooms(1, 1, 2, 3);

        return map;
    }
}
