using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Data
{
    public enum EquipmentType { MainHand, Offhand, Head, Body, Legs, Hands, Feet, Rod, Pickaxe, Axe }

    public class ItemEquipment : Item
    {
        private static string[] TypeNames = { "Main-hand", "Off-hand", "Head", "Body", "Legs", "Hands", "Feet",
            "Rod", "Pickaxe", "Axe" };

        public EquipmentType Type;
        public int LevelRequirement { get; private set; }
        public int HitPoints;
        public int MinDamage;
        public int MaxDamage;
        public int Defence;
        public string TypeName { get { return TypeNames[(int)Type]; } }

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
