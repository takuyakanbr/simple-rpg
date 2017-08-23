using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine.Data
{
    public static class World
    {
        private static Entity[] Entities = new Entity[25];
        private static Item[] Items = new Item[200];
        private static Monster[] Monsters = new Monster[25];
        private static Quest[] Quests = new Quest[10];
        private static Tile[] Tiles = new Tile[100];
        private static Vendor[] Vendors = new Vendor[10];
        
        private static Entity Add(Entity entity)
        {
            Entities[entity.ID] = entity;
            return entity;
        }

        private static Item Add(Item item)
        {
            Items[item.ID] = item;
            return item;
        }
        
        private static Monster Add(Monster monster)
        {
            Monsters[monster.ID] = monster;
            return monster;
        }

        private static Quest Add(Quest quest)
        {
            Quests[quest.ID] = quest;
            return quest;
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
        
        private static Vendor Add(Vendor vendor)
        {
            Vendors[vendor.ID] = vendor;
            return vendor;
        }
        
        public static Entity GetEntity(int id) { return Entities[id]; }
        public static Item GetItem(int id) { return Items[id]; }
        public static Monster GetMonster(int id) { return Monsters[id]; }
        public static Quest GetQuest(int id) { return Quests[id]; }
        public static Tile GetTile(int id) { return Tiles[id]; }
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
            Add(new Item(0, "Raw anchovy", "A small, common fish (Lvl 1)", 1));
            Add(new Item(1, "Raw herring", "Silvery colored fish with a single dorsal fin (Lvl 11)", 2));
            Add(new Item(2, "Raw bass", "Who dropped the bass? (Lvl 21)", 3));
            Add(new ItemConsumable(10, "Anchovy", "Heals 5 HP (Lvl 1)", 2, 5));
            Add(new ItemConsumable(11, "Herring", "Heals 10 HP (Lvl 11)", 5, 10));
            Add(new ItemConsumable(12, "Bass", "Heals 18 HP (Lvl 21)", 10, 18));
            Add(new Item(20, "Copper ore", "There's copper in here (Lvl 1)", 1));
            Add(new Item(21, "Silver ore", "There's silver in here (Lvl 11)", 2));
            Add(new Item(22, "Gold ore", "There's gold in here! (Lvl 21)", 3));
            Add(new Item(30, "Copper bar", "A soft, malleable, ductile metal (Lvl 1)", 1));
            Add(new Item(31, "Silver bar", "A soft, white, lustrous metal (Lvl 11)", 2));
            Add(new Item(32, "Gold bar", "Shiiinyyyy! (Lvl 21)", 3));
            Add(new Item(40, "Pine wood", "Wood from a pine tree (Lvl 1)", 1));
            Add(new Item(41, "Birch wood", "Wood from a birch tree (Lvl 11)", 2));
            Add(new Item(42, "Spruce wood", "Wood from a spruce tree (Lvl 21)", 3));

            Add(new Item(51, "Butterfly wing", "I hear someone is collecting this.", 1));
            Add(new Item(52, "Rat tail", "Eew. Why did I take this with me?", 1));
            Add(new ItemEquipment(53, "Wooden stick", "It looks like it's rotting (1-4 Atk)",
                5, EquipmentType.MainHand, 1, 0, 1, 4, 0));
            Add(new Item(54, "Gold pendant", "Found on a skeleton.", 25, ItemFlags.Unique));
            Add(new ItemEquipment(55, "Metal bar", "It looks like it's rusting (2-7 Atk)",
                10, EquipmentType.MainHand, 2, 0, 2, 7, 0));
            Add(new ItemEquipment(56, "Sandals", "Protection against sand (0 Def)",
                3, EquipmentType.Feet, 1, 0, 0, 0, 0));

            Add(new ItemEquipment(101, "Copper gloves", "Gloves made from copper (1 Def)", 5, EquipmentType.Hands, 1, 0, 0, 0, 1));
            Add(new ItemEquipment(102, "Copper boots", "Boots made from copper (1 Def)", 5, EquipmentType.Feet, 1, 0, 0, 0, 1));
            Add(new ItemEquipment(103, "Copper helmet", "Helmet made from copper (1 Def)", 5, EquipmentType.Head, 2, 0, 0, 0, 1));
            Add(new ItemEquipment(104, "Copper legs", "Platelegs made from copper (2 Def)", 6, EquipmentType.Legs, 3, 0, 0, 0, 2));
            Add(new ItemEquipment(105, "Copper body", "Platebody made from copper (2 Def)", 6, EquipmentType.Body, 4, 0, 0, 0, 2));
            Add(new ItemEquipment(106, "Copper sword", "Sword made from copper (3-8 Atk)", 5, EquipmentType.MainHand, 3, 0, 3, 8, 0));
        }

        private static void PopulateMonsters()
        {
            Add(new Monster(0, "Butterfly", 5, 0, 0, 0, 2, 0, 0))
                .AddLoot(51, 1, 1, 0.4);
            Add(new Monster(1, "Rat", 10, 0, 2, 0, 4, 0, 0))
                .AddLoot(52, 1, 1, 0.4);
            Add(new Monster(2, "Giant rat", 20, 1, 6, 1, 10, 2, 5))
                .AddLoot(52, 1, 1, 0.99);
        }

        private static void PopulateQuests()
        {
            Add(new Quest(0, "Collect butterfly wings", "Help Madam Jacqueline collect butterfly wings.", 1, 25, 10));
            Add(new Quest(1, "The gold pendant", "You found a gold pendant on a skeleton.", 1, 25, 40));
        }

        private static void PopulateVendors()
        {
            Add(new Vendor(0, "Store owner")).AddItems(new[] { 0, 10 }, 50);
            Add(new Vendor(1, "Craftsman")).AddItems(new[] { 101, 102, 103, 104, 105, 106 }, 10);
        }

        private static void PopulateEntities()
        {
            Add(new Entity(0, "Madam Jacqueline", "She's eagerly watching you."))
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
                            p.RemoveItemFromInventory(51, 8);
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
                                p.AddItemToInventory(53);
                            }
                            else
                            {
                                s.ShowDialog("I guess I'll just have to find someone else. Do come back if you change your mind!");
                            }
                            break;
                    }
                };
            var e1 = Add(new Entity(1, "Seashell", "She sells seashells by the sea shore."));
            e1.OnCreate = delegate (GameState s, Player p)
            {
                return RandomGenerator.NextDouble() < 0.25;
            };
            e1.OnInteract = delegate (GameState s, Player p)
            {
                s.RaiseMessage("Ouch! A crab climbs out of the shell and bites your finger.");
                s.TakeDamage(3, "crab");
            };
            Add(new Entity(2, "Miner", "He's mining."))
                .OnInteract = delegate (GameState s, Player p)
                {
                    s.RaiseMessage("He's too focused on his work to talk to you.");
                };
            Add(new Entity(3, "Skeleton", "He's dead, I can feel it in my bones."))
                .OnInteract = delegate (GameState s, Player p)
                {
                    if (p.HasQuest(1))
                    {
                        s.RaiseMessage("Just a skeleton, nothing to see here.");
                        return;
                    }

                    if (s.InteractCounter == 0)
                    {
                        s.ShowDialog("(You see a gold pendant around the skeleton's neck. It looks important.)", new string[] { "Take it!", "Leave it." });
                    }
                    else
                    {
                        if (s.InteractChoice == 1)
                        {
                            s.ShowDialog("(You carefully remove the gold pendant from around the skeleton's neck. You hear the skeleton groaning. Or was that your imagination?)");
                            p.UpdateQuest(1, 0);
                            p.AddItemToInventory(54);
                        } 
                        else
                        {
                            s.EndDialog();
                        }
                    }
                };
            Add(new Entity(4, "Store owner", "He can probably sell you fish."))
                .OnInteract = delegate (GameState s, Player p)
                {
                    s.ShowVendor(0);
                };
            Add(new Entity(5, "Craftsman", "He probably owns this place."))
                .OnInteract = delegate (GameState s, Player p)
                {
                    s.ShowVendor(1);
                };
            Add(new Entity(6, "Businessman", "Reeks of arrogance."));
            Add(new Entity(7, "Servant", "Servant."));
            Add(new Entity(8, "Fisherman", "He spends the whole day fishing."));
        }

        private static void PopulateTiles()
        {
            PopulateTilesKnoxville();
        }

        private static void PopulateTilesKnoxville()
        {
            string knoxville = "Knoxville, Kingdom of Fulgar";
            string westKnoxville = "Western Knoxville, Kingdom of Fulgar";

            var landingPad = new TileInfo("Landing Pad", westKnoxville, "Why is there a convenient landing pad here? A spawn point for me?!").AddEntity(0);
            var plains = new TileInfo("Plains", westKnoxville, "You see a small town to the east.").AddMonsterSpawn(0, 0.75);
            var gate = new TileInfo("Gate", knoxville, "A guard waves at you, 'Welcome to Knoxville, traveller.'");
            var town1 = new TileInfo("Town", knoxville, "A peaceful, small town. There's 2 gates, to the north, and to the west. A small dock sits at the shore to the east.");
            var town2 = town1.Clone();
            var docks = new TileInfo("Docks", knoxville, "A nice place to fish, and to catch the sunrise.").AddEntity(8);
            var market = new TileInfo("Market", knoxville, "Fresh produce, and lots of fish.").AddEntity(4);
            var house1 = new TileInfo("House", knoxville, "A nice, spacious home. In fact, as far as you can see, it's the nicest one in this town.").AddEntity(6).AddEntity(7);
            var house2 = new TileInfo("House", knoxville, "A small place, with tools used by craftsmen.").AddEntity(5);
            var beach = new TileInfo("Beach", westKnoxville, "The sound of waves crashing against the shore is so soothing...").AddEntity(1);
            var mineEntrance = new TileInfo("Mine Entrance", westKnoxville, "There's sounds of metal striking against rock coming from inside.");
            var mines1 = new TileInfo("Mines", westKnoxville, "Hot, dirty, and full of rats. Why would anyone choose to work here?");
            var mines2 = mines1.Clone().AddEntity(2); // miners
            var mines3 = mines1.Clone().AddEntity(3); // skeleton
            var mines4 = mines1.Clone().AddMonsterSpawn(2, 1.0, true, true); // giant rat
            mines1.AddMonsterSpawn(1, 0.67);

            AddTileGrid(plains, 1, 3, 3); // plains 1-9
            AddTileGrid(town1, 11, 3, 1); // town 11-13
            AddTileGrid(town1, 15, 1, 4); // town 15-18
            AddTileGrid(docks, 19, 2, 1); // docks 19-20
            AddTileGrid(beach, 24, 5, 2); // beach 24-33
            AddTileGrid(mines1, 35, 1, 4); // mines 35-38
            GetTile(18).ConnectSouth(13);
            GetTile(32).ConnectNorth(7);
            GetTile(33).ConnectNorth(8);

            AddTile(0, landingPad).ConnectSouth(1);
            AddTile(10, gate).ConnectWest(6).ConnectEast(11);
            AddTile(14, town2).ConnectWest(13).ConnectEast(19);
            AddTile(21, market).ConnectNorth(13);
            AddTile(22, house1).ConnectNorth(11);
            AddTile(23, house2).ConnectWest(17);
            AddTile(34, mineEntrance).ConnectSouth(24).ConnectNorth(38);
            AddTile(39, mines2).ConnectEast(35);
            AddTile(40, mines2).ConnectEast(37);
            AddTile(41, mines4);
            AddTile(42, mines1).ConnectNorth(41);
            AddTile(43, mines3).ConnectNorth(42).ConnectWest(35);
        }
    }
}
