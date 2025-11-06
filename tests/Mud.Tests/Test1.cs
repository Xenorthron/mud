namespace Mud.Tests;

[TestClass]
public sealed class Test1
{
    [TestMethod]
    public void TestPlayerCreation()
    {
        var player = new PlayerCharacter("TestHero", "A test hero");
        Assert.AreEqual("TestHero", player.Name);
        Assert.AreEqual(100, player.Health);
        Assert.AreEqual(10, player.Attack);
        Assert.AreEqual(0, player.Defense);
        Assert.IsNotNull(player.Inventory);
    }

    [TestMethod]
    public void TestNPCCreation()
    {
        var npc = new NonPlayerCharacter("Goblin", "A sneaky goblin", 30, 5, 2, false);
        Assert.AreEqual("Goblin", npc.Name);
        Assert.AreEqual(30, npc.Health);
        Assert.AreEqual(5, npc.Attack);
        Assert.AreEqual(2, npc.Defense);
    }

    [TestMethod]
    public void TestCombat()
    {
        var player = new PlayerCharacter("TestHero", "A test hero");
        var npc = new NonPlayerCharacter("Goblin", "A sneaky goblin", 30, 5, 2, false);
        
        int initialHealth = npc.Health;
        player.AttackTarget(npc);
        
        // Player attack (10) - NPC defense (2) = 8 damage
        Assert.AreEqual(initialHealth - 8, npc.Health);
    }

    [TestMethod]
    public void TestMinimumDamage()
    {
        var player = new PlayerCharacter("TestHero", "A test hero");
        var npc = new NonPlayerCharacter("Tank", "A heavily armored tank", 100, 5, 50, false);
        
        int initialHealth = npc.Health;
        player.AttackTarget(npc);
        
        // Player attack (10) - NPC defense (50) = -40, but minimum is 1
        Assert.AreEqual(initialHealth - 1, npc.Health);
    }

    [TestMethod]
    public void TestWeaponEquip()
    {
        var player = new PlayerCharacter("TestHero", "A test hero");
        var weapon = new Weapon("Sword", "A sharp sword", 10, 5);
        
        weapon.Use(player);
        Assert.AreEqual(weapon, player.Weapon);
    }

    [TestMethod]
    public void TestArmorEquip()
    {
        var player = new PlayerCharacter("TestHero", "A test hero");
        var armor = new Armor("Chain Mail", "Heavy armor", 20, 5);
        
        armor.Use(player);
        Assert.AreEqual(armor, player.Armor);
    }

    [TestMethod]
    public void TestFoodHealing()
    {
        var player = new PlayerCharacter("TestHero", "A test hero");
        player.Health = 50;
        
        var food = new Food("Apple", "A red apple", 5, 20);
        food.Use(player);
        
        Assert.AreEqual(70, player.Health);
    }

    [TestMethod]
    public void TestFoodHealingCap()
    {
        var player = new PlayerCharacter("TestHero", "A test hero");
        player.Health = 95;
        
        var food = new Food("Apple", "A red apple", 5, 20);
        food.Use(player);
        
        // Should cap at 100
        Assert.AreEqual(100, player.Health);
    }

    [TestMethod]
    public void TestBuff()
    {
        var player = new PlayerCharacter("TestHero", "A test hero");
        int initialAttack = player.Attack;
        int initialDefense = player.Defense;
        
        var buff = new Buff("Strength Potion", "Increases attack", 5, 3);
        buff.Use(player);
        
        Assert.AreEqual(initialAttack + 5, player.Attack);
        Assert.AreEqual(initialDefense + 3, player.Defense);
        Assert.AreEqual(1, player.ActiveBuffs.Count);
    }

    [TestMethod]
    public void TestBuffExpiry()
    {
        var player = new PlayerCharacter("TestHero", "A test hero");
        int initialAttack = player.Attack;
        
        var buff = new Buff("Strength Potion", "Increases attack", 5, 0);
        buff.Use(player);
        
        // Tick 10 times to expire buff
        for (int i = 0; i < 10; i++)
        {
            player.GameTick();
        }
        
        Assert.AreEqual(initialAttack, player.Attack);
        Assert.AreEqual(0, player.ActiveBuffs.Count);
    }

    [TestMethod]
    public void TestInventoryAddItem()
    {
        var player = new PlayerCharacter("TestHero", "A test hero");
        var weapon = new Weapon("Sword", "A sharp sword", 10, 5);
        
        bool added = player.Inventory.AddItem(weapon);
        Assert.IsTrue(added);
    }

    [TestMethod]
    public void TestRoomCreation()
    {
        var room = new Room(10, 10, "A test room", new bool[] { true, false, false, false });
        Assert.AreEqual(10, room.Width);
        Assert.AreEqual(10, room.Height);
        Assert.IsTrue(room.Exits[0]); // North exit
    }

    [TestMethod]
    public void TestMapCreation()
    {
        var map = Map.CreateDefaultMap();
        Assert.IsNotNull(map);
        Assert.IsTrue(map.Rooms.Length >= 2);
    }

    [TestMethod]
    public void TestGameControllerCreation()
    {
        var player = new PlayerCharacter("TestHero", "A test hero");
        var map = Map.CreateDefaultMap();
        var controller = new GameController(player, map);
        
        Assert.IsNotNull(controller);
        Assert.AreEqual(player, controller.Player);
        Assert.IsNotNull(controller.CurrentRoom);
    }

    [TestMethod]
    public void TestDayNightCycle()
    {
        var player = new PlayerCharacter("TestHero", "A test hero");
        var map = Map.CreateDefaultMap();
        var controller = new GameController(player, map);
        
        Assert.IsTrue(controller.IsDay);
        
        // Advance 10 turns
        for (int i = 0; i < 10; i++)
        {
            controller.EndTurn();
        }
        
        Assert.IsFalse(controller.IsDay); // Should be night now
    }

    [TestMethod]
    public void TestNPCDayNightStats()
    {
        var npc = new NonPlayerCharacter("Vampire", "A bloodthirsty vampire", 55, 18, 8, true);
        
        // Initially nocturnal, so at day stats should decrease
        npc.SwitchToDay();
        Assert.AreEqual((int)(18 * 0.8), npc.Attack);
        Assert.AreEqual((int)(8 * 0.8), npc.Defense);
        
        // At night, stats should increase
        npc.SwitchToNight();
        Assert.AreEqual((int)(18 * 1.2), npc.Attack);
        Assert.AreEqual((int)(8 * 1.2), npc.Defense);
    }

    [TestMethod]
    public void TestTrapDetection()
    {
        const int MAX_DETECTION_ATTEMPTS = 100;
        var trap = new Trap(10);
        Assert.IsFalse(trap.IsDetected);
        
        // Try detection multiple times (50% chance)
        bool detected = false;
        for (int i = 0; i < MAX_DETECTION_ATTEMPTS; i++)
        {
            var testTrap = new Trap(10);
            if (testTrap.AttemptDetect())
            {
                detected = true;
                Assert.IsTrue(testTrap.IsDetected);
                break;
            }
        }
        Assert.IsTrue(detected); // Should detect at least once in 100 tries
    }

    [TestMethod]
    public void TestChestLoot()
    {
        var chest = new Chest();
        Assert.IsNotNull(chest.Loot);
        Assert.IsTrue(chest.Loot.Length >= 1 && chest.Loot.Length <= 5);
    }
}
