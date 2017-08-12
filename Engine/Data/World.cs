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

        private static Item AddItem(Item item)
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
            AddItem(new ItemConsumable(2, "Potion", "Heals 20 HP.", 10, 20));
            AddItem(new ItemEquipment(3, "Wooden stick", "It looks like it's rotting.",
                5, EquipmentType.MainHand, 1, 0, 1, 4, 0));
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

        }

        private static void PopulateVendors()
        {

        }

        private static void PopulateEntities()
        {
            AddEntity(0, "Madam Jacqueline", "She's eagerly watching you.")
                .OnInteract = delegate(GameState state, Player player)
                {
                    if (state.InteractCounter == 0)
                        state.ShowDialog("Hello, my dear.", new string[] { "Hello." });
                    else if (state.InteractCounter == 1)
                        state.ShowDialog("Yes, yes. Please go away now.", null);
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
