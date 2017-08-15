using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Data
{
    public static class World
    {
        private static Entity[] Entities = new Entity[25];
        private static Item[] Items = new Item[25];
        private static Monster[] Monsters = new Monster[25];
        private static Quest[] Quests = new Quest[10];
        private static Tile[] Tiles = new Tile[100];
        private static TileInfo[] TileInfos = new TileInfo[50];
        private static Vendor[] Vendors = new Vendor[10];
        
        private static Entity AddEntity(int id, string name, string description)
        {
            Entities[id] = new Entity(id, name, description);
            return Entities[id];
        }

        private static Item Add(Item item)
        {
            Items[item.ID] = item;
            return item;
        }

        private static Item AddItem(int id, string name, string description, int price, ItemFlags flags = 0)
        {
            Items[id] = new Item(id, name, description, price, flags);
            return Items[id];
        }
        
        private static Monster AddMonster(int id, string name, int hitPoints, int minDamage, int maxDamage,
            int defence, int xp, int minGold, int maxGold)
        {
            Monsters[id] = new Monster(id, name, hitPoints, minDamage, maxDamage, defence, xp, minGold, maxGold);
            return Monsters[id];
        }

        private static Quest AddQuest(int id, string name, string description, int level, int xp, int gold)
        {
            Quests[id] = new Quest(id, name, description, level, xp, gold);
            return Quests[id];
        }
        
        private static Tile AddTile(int id, TileInfo data)
        {
            Tiles[id] = new Tile(id, data);
            return Tiles[id];
        }

        private static void AddTileGrid(TileInfo tileInfo, int startId, int width, int height)
        {
            int id = startId;
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    AddTile(id, tileInfo);
                    if (i > 0)
                    {
                        Tiles[id - 1].East = Tiles[id];
                        Tiles[id].West = Tiles[id - 1];
                    }
                    if (j > 0)
                    {
                        Tiles[id - width].South = Tiles[id];
                        Tiles[id].North = Tiles[id - width];
                    }
                    id++;
                }
            }
        }

        private static TileInfo AddTileInfo(int id, string name, string geoName, string description)
        {
            TileInfos[id] = new TileInfo(id, name, geoName, description);
            return TileInfos[id];
        }

        private static Vendor AddVendor(int id, string name)
        {
            Vendors[id] = new Vendor(id, name);
            return Vendors[id];
        }

        public static Entity GetEntity(int id) { return Entities[id]; }
        public static Item GetItem(int id) { return Items[id]; }
        public static Monster GetMonster(int id) { return Monsters[id]; }
        public static Quest GetQuest(int id) { return Quests[id]; }
        public static Tile GetTile(int id) { return Tiles[id]; }
        public static TileInfo GetTileInfo(int id) { return TileInfos[id]; }
        public static Vendor GetVendor(int id) { return Vendors[id]; }

        static World()
        {
            PopulateItems();
            PopulateMonsters();
            PopulateQuests();
            PopulateVendors();
            PopulateEntities();
            PopulateTiles();
        }

        private static void PopulateItems()
        {
            AddItem(0, "Butterfly wing", "I hear someone is collecting this.", 1);
            AddItem(1, "Rat tail", "Eew. Why did I take this with me?", 1);
            Add(new ItemConsumable(2, "Potion", "Heals 20 HP.", 10, 20));
            Add(new ItemEquipment(3, "Wooden stick", "It looks like it's rotting.",
                5, EquipmentType.MainHand, 1, 0, 1, 4, 0));
            Add(new ItemEquipment(4, "Metal bar", "It looks like it's rusting.",
                10, EquipmentType.MainHand, 2, 0, 2, 8, 0));
            Add(new ItemEquipment(5, "Sandals", "Protection against sand.",
                5, EquipmentType.Feet, 1, 0, 0, 0, 1));
        }

        private static void PopulateMonsters()
        {
            AddMonster(0, "Butterfly", 5, 0, 0, 0, 2, 0, 0)
                .AddLoot(GetItem(0), 1, 1, 0.4);
            AddMonster(1, "Rat", 10, 0, 2, 0, 4, 0, 0)
                .AddLoot(GetItem(1), 1, 1, 0.4);
        }

        private static void PopulateQuests()
        {
            AddQuest(0, "Collect butterfly wings", "Help Madam Jacqueline collect butterfly wings.", 1, 25, 10);
        }

        private static void PopulateVendors()
        {

        }

        private static void PopulateEntities()
        {
            AddEntity(0, "Madam Jacqueline", "She's eagerly watching you.")
                .OnInteract = delegate (GameState s, Player p)
                {
                    if (p.IsQuestComplete(0))
                    {
                        if (s.InteractCounter == 0)
                            s.ShowDialog("Thank you so much for the butterfly wings!", new string[] { "What did you use them for?" });
                        else if (s.InteractCounter == 1)
                            s.ShowDialog("I'm incorporating them in my latest art project. It'll be a sight to behold!");
                        return;
                    }

                    if (p.HasQuest(0))
                    {
                        if (p.HasItem(0, 8))
                        {
                            s.ShowDialog("I see you've brought me the 8 butterfly wings. Here's 10 gold, for your troubles.");
                            p.RemoveItemFromInventory(0, 8);
                            p.CompleteQuest(0);
                            return;
                        }
                        s.ShowDialog("Don't forget to bring me 8 butterfly wings! I'll be waiting here for you.");
                        return;
                    }

                    switch (s.InteractCounter)
                    {
                        case 0:
                            s.ShowDialog("My hero! You're just the person I'm looking for!", new string[] { "What?" });
                            break;
                        case 1:
                            s.ShowDialog("You see, I'm in a bit of a bind right now. I'm in need of 100 butterfly wings but I'm too busy to collect them myself. Say... will you help me please?", new string[] { "No way, I'm not some dumb protagonist in a videogame." });
                            break;
                        case 2:
                            s.ShowDialog("Okay, okay, I understand if it may be a bit too much. How about just 8 butterfly wings? I'll be sure to reward you!", new string[] { "Alright then.", "No, sorry." });
                            break;
                        case 3:
                            if (s.InteractChoice == 1)
                            {
                                s.ShowDialog("Great! Here's a Wooden stick to speed up the process of beating up the butterflies.");
                                p.UpdateQuest(0, 0);
                                p.AddItemToInventory(3);
                            }
                            else
                            {
                                s.ShowDialog("I guess I'll just have to find someone else. Do come back if you change your mind!");
                            }
                            break;
                    }
                };
            var e1 = AddEntity(1, "Seashell", "She sells seashells by the sea shore.");
            e1.OnCreate = delegate (GameState s, Player p)
            {
                return RandomGenerator.NextDouble() < 0.25;
            };
            e1.OnInteract = delegate (GameState s, Player p)
            {
                s.TakeDamage(3);
                s.RaiseMessage("Ouch! A crab climbs out of the shell and bites your finger.");
            };
        }

        private static void PopulateTiles()
        {
            var ti0 = AddTileInfo(0, "Landing Pad", "Western Knoxville, Kingdom of Fulgar", "Why is there a convenient landing pad here? A spawn point for me?!").AddEntity(0);
            var ti1 = AddTileInfo(1, "Plains", "Western Knoxville, Kingdom of Fulgar", "You see a small town to the east.").AddMonsterSpawn(0, 0.75);
            var ti2 = AddTileInfo(2, "Gate", "Knoxville, Kingdom of Fulgar", "A guard waves at you, 'Welcome to Knoxville, traveller.'");
            var ti3 = AddTileInfo(3, "Town", "Knoxville, Kingdom of Fulgar", "A peaceful, small town. There's 2 gates, to the north, and to the west. A small dock sits at the shore to the east.");
            var ti4 = AddTileInfo(4, "Town", "Knoxville, Kingdom of Fulgar", "A peaceful, small town. There's 2 gates, to the north, and to the west. A small dock sits at the shore to the east.");
            var ti5 = AddTileInfo(5, "Docks", "Knoxville, Kingdom of Fulgar", "A nice place to fish, and to catch the sunrise.");
            var ti6 = AddTileInfo(6, "Market", "Knoxville, Kingdom of Fulgar", "Fresh produce, and lots of fish.");
            var ti7 = AddTileInfo(7, "House", "Knoxville, Kingdom of Fulgar", "A nice, spacious home. In fact, as far as you can see, it's the nicest one in this town.");
            var ti8 = AddTileInfo(8, "House", "Knoxville, Kingdom of Fulgar", "A small place, with tools used by craftsmen.");
            var ti9 = AddTileInfo(9, "Beach", "Western Knoxville, Kingdom of Fulgar", "The sound of waves crashing against the shore is so soothing...").AddEntity(1);
            var ti10 = AddTileInfo(10, "Mine Entrance", "Western Knoxville, Kingdom of Fulgar", "There's sounds of metal striking against rock coming from inside.");
            var ti11 = AddTileInfo(11, "Mines", "Western Knoxville, Kingdom of Fulgar", "Hot, dirty, and full of rats. Why would anyone choose to work here?");

            AddTileGrid(ti1, 1, 3, 3); // plains 1-9
            AddTile(0, ti0) // landing pad
                .ConnectSouth(GetTile(1));
            AddTileGrid(ti3, 11, 3, 1); // town 11-13
            AddTileGrid(ti3, 15, 1, 4); // town 15-18
            AddTileGrid(ti5, 19, 2, 1); // docks 19-20
            AddTileGrid(ti9, 24, 5, 2); // beach 24-33
            GetTile(18).ConnectSouth(GetTile(13));
            GetTile(32).ConnectNorth(GetTile(7));
            GetTile(33).ConnectNorth(GetTile(8));
            AddTile(10, ti2) // gate
                .ConnectWest(GetTile(6)).ConnectEast(GetTile(11));
            AddTile(14, ti4) // town
                .ConnectWest(GetTile(13)).ConnectEast(GetTile(19));
            AddTile(21, ti6) // market
                .ConnectNorth(GetTile(13));
            AddTile(22, ti7) // nice house
                .ConnectNorth(GetTile(11));
            AddTile(23, ti8) // craft house
                .ConnectWest(GetTile(17));
            AddTile(34, ti10) // mine entrance
                .ConnectSouth(GetTile(24));

        }
    }
}
