using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Data
{
    public class LootItem
    {
        public Item Data;
        public int MinQuantity;
        public int MaxQuantity;
        public double DropChance; // possible values: 0.0 - 1.0; 1.0 = 100% chance

        public LootItem(Item data, int minQuantity, int maxQuantity, double dropChance)
        {
            Data = data;
            MinQuantity = minQuantity;
            MaxQuantity = maxQuantity;
            DropChance = dropChance;
        }
    }
}
