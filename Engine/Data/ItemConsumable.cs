using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Data
{
    public class ItemConsumable : Item
    {
        public int HitPoints;

        public ItemConsumable(int id, string name, string description, int price,
            int hitPoints, ItemFlags flags = 0)
            : base(id, name, description, price, flags)
        {
            HitPoints = hitPoints;
        }
    }
}
