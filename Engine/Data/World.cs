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
        private static Tile[] Tiles = new Tile[25];
        private static TileInfo[] TileInfos = new TileInfo[25];
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
                .OnInteract = delegate(GameState s, Player p)
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
        }

        private static void PopulateTiles()
        {
            var ti1 = AddTileInfo(1, "Plains", "Western Knoxville, Kingdom of Fulgar", "You see a small town to the east.").AddMonsterSpawn(GetMonster(0), 0.75);
            AddTileGrid(ti1, 1, 3, 3); // tiles 1 - 9

            var ti0 = AddTileInfo(0, "Landing Pad", "Western Knoxville, Kingdom of Fulgar", "Why is there a convenient landing pad here? A spawn point for me?!").AddEntity(GetEntity(0));
            AddTile(0, ti0).ConnectSouth(GetTile(1));

            var ti2 = AddTileInfo(2, "Gate", "Knoxville, Kingdom of Fulgar", "A guard waves at you, 'Welcome to Knoxville, traveller.'");
            AddTile(10, ti2).ConnectWest(GetTile(6));
        }
    }
}
