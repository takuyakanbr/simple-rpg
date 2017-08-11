using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Data
{
    public enum EquipmentType { MainHand, Offhand, Head, Body, Legs, Hands, Feet }

    public class ItemEquipment : Item
    {
        public EquipmentType Type;
        public int LevelRequirement;
        public int HitPoints;
        public int MinDamage;
        public int MaxDamage;
        public int Defence;

        public ItemEquipment(int id, string name, string description, int price,
            EquipmentType type, int levelRequirement, int hitPoints,
            int minDamage, int maxDamage, int defence, ItemFlags flags = 0)
            : base(id, name, description, price, flags | ItemFlags.Unstackable)
        {
            Type = type;
            LevelRequirement = levelRequirement;
            HitPoints = hitPoints;
            MinDamage = minDamage;
            MaxDamage = maxDamage;
            Defence = defence;
        }
    }
}
