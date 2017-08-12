using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Data
{
    [Flags]
    public enum ItemFlags
    {
        None = 0x00,
        Untradable = 0x01,
        Unique = 0x02,
        Unstackable = 0x04
    }

    public class Item
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description;
        public int Price;
        public ItemFlags Flags;

        public int BuyPrice
        {
            get
            {
                return Price * 2;
            }
        }
        public bool Untradable { get { return (Flags & ItemFlags.Untradable) > 0; } }
        public bool Unique { get { return (Flags & ItemFlags.Unique) > 0; } }
        public bool Unstackable { get { return (Flags & ItemFlags.Unstackable) > 0; } }

        public Item(int id, string name, string description, int price, ItemFlags flags = 0)
        {
            ID = id;
            Name = name;
            Description = description;
            Price = price;
            Flags = flags;
        }
    }
}
