﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Data
{
    // contains data about a monster
    public class Monster
    {
        public int ID;
        public string Name;
        public int HitPoints;
        public int MinDamage;
        public int MaxDamage;
        public int Defence;
        public int RewardXP;
        public int MinGold;
        public int MaxGold;
        public List<LootItem> LootTable;

        // run when the monster is killed
        public Action<GameState, Player> OnKill;

        public Monster(int id, string name, int hitPoints, int minDamage, int maxDamage, 
            int defence, int xp, int minGold, int maxGold)
        {
            ID = id;
            Name = name;
            HitPoints = hitPoints;
            MinDamage = minDamage;
            MaxDamage = maxDamage;
            Defence = defence;
            RewardXP = xp;
            MinGold = minGold;
            MaxGold = maxGold;
            LootTable = new List<LootItem>();
        }

        public Monster AddLoot(int itemID, int minQuantity, int maxQuantity, double dropChance)
        {
            LootTable.Add(new LootItem(World.GetItem(itemID), minQuantity, maxQuantity, dropChance));
            return this;
        }

        public Monster AddLoot(Item item, int minQuantity, int maxQuantity, double dropChance)
        {
            LootTable.Add(new LootItem(item, minQuantity, maxQuantity, dropChance));
            return this;
        }
    }
}
