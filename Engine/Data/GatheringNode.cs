using System;
using System.Collections.Generic;
using System.Linq;

namespace Engine.Data
{
    // an entity that the player can interact with to gather resources using a gathering skill
    public class GatheringNode : Entity
    {
        // the skill this gathering node uses / requires
        public SkillType Skill;

        // minimum level in the skill that is needed to use this node
        public int Level;

        // experience received from the use of this node
        public int Experience;

        // number of times the player will attempt using this node before interaction is required
        public int GatherCounts;

        // the type of equipment needed to use this node
        public EquipmentType Equipment;

        // list of LootItem that can be acquired from this node
        public List<LootItem> LootTable;

        public GatheringNode(int id, string name, string description, SkillType skill, int level, 
            int experience, int gatherCounts, EquipmentType equipment) : base(id, name, description)
        {
            Skill = skill;
            Level = level;
            Experience = experience;
            GatherCounts = gatherCounts;
            LootTable = new List<LootItem>();
        }

        public GatheringNode AddLoot(int itemID, int minQuantity, int maxQuantity, double dropChance)
        {
            LootTable.Add(new LootItem(World.GetItem(itemID), minQuantity, maxQuantity, dropChance));
            return this;
        }

        public GatheringNode AddLoot(Item item, int minQuantity, int maxQuantity, double dropChance)
        {
            LootTable.Add(new LootItem(item, minQuantity, maxQuantity, dropChance));
            return this;
        }
    }
}
