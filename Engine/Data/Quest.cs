using System;
using System.Collections.Generic;
using System.Linq;

using Engine.Models;

namespace Engine.Data
{
    public class Quest
    {
        public int ID;
        public string Name;
        public string Description;
        public int LevelRequirement;
        public List<int> QuestRequirements;
        public int RewardXP;
        public int RewardGold;
        public List<RewardItem> RewardItems;

        public Quest(int id, string name, string description, int level, int xp, int gold)
        {
            ID = id;
            Name = name;
            Description = description;
            LevelRequirement = level;
            RewardXP = xp;
            RewardGold = gold;
            QuestRequirements = new List<int>();
            RewardItems = new List<RewardItem>();
        }
    }
}
