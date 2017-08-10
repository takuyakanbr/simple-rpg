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

        private static Entity AddEntity(Entity entity)
        {
            Entities[entity.ID] = entity;
            return entity;
        }

        private static Item AddItem(Item item)
        {
            Items[item.ID] = item;
            return item;
        }

        private static Monster AddMonster(Monster monster)
        {
            Monsters[monster.ID] = monster;
            return monster;
        }

        private static Quest AddQuest(Quest quest)
        {
            Quests[quest.ID] = quest;
            return quest;
        }

        private static Tile AddTile(Tile tile)
        {
            Tiles[tile.ID] = tile;
            return tile;
        }

        private static void AddTileGrid(TileInfo tileInfo, int startId, int width, int height)
        {
            int id = startId;
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    AddTile(new Tile(tileInfo, id));
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

        private static TileInfo AddTileInfo(TileInfo tileInfo)
        {
            TileInfos[tileInfo.ID] = tileInfo;
            return tileInfo;
        }

        private static Vendor AddVendor(Vendor vendor)
        {
            Vendors[vendor.ID] = vendor;
            return vendor;
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

        }

        private static void PopulateMonsters()
        {

        }

        private static void PopulateQuests()
        {

        }

        private static void PopulateVendors()
        {

        }

        private static void PopulateEntities()
        {

        }

        private static void PopulateTiles()
        {

        }
    }
}
