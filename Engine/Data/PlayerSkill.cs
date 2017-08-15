using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Engine.Data
{
    public enum SkillType { Combat, Crafting, Cooking, Fishing, Mining, Smithing, Woodcutting };

    public class PlayerSkill : INotifyPropertyChanged
    {
        public static readonly string[] SKILL_NAMES = { "Combat", "Crafting", "Cooking", "Fishing", "Mining", "Smithing", "Woodcutting" };
        public static readonly SkillType[] SKILL_TYPES = { SkillType.Combat, SkillType.Crafting, SkillType.Cooking,
            SkillType.Fishing, SkillType.Mining, SkillType.Smithing, SkillType.Woodcutting };
        public static readonly int NUM_SKILL_TYPES = SKILL_TYPES.Length;
        public static readonly int MAX_LEVEL = 100;

        private int _level;
        private int _experience;

        public SkillType Type;
        public int Level
        {
            get { return _level; }
            private set
            {
                _level = value;
                OnPropertyChanged("Level");
            }
        }
        public int Experience
        {
            get { return _experience; }
            private set
            {
                _experience = value;
                OnPropertyChanged("Experience");
            }
        }

        public int ID { get { return (int)Type; } }
        public string Name { get { return SKILL_NAMES[(int)Type]; } }
        public int ExperienceToNextLevel { get { return Level * 100 + (int)Math.Pow(Level, 2) * 100; } }

        public PlayerSkill(SkillType type, int level = 1, int experience = 0)
        {
            Type = type;
            Level = level;
            Experience = experience;
        }

        public void AddExperience(int points)
        {
            Experience += points;
            if (Level >= MAX_LEVEL)
                return;
            if (Experience >= ExperienceToNextLevel)
                Level += 1;
        }

        public void Update(int level, int points)
        {
            Level = level;
            Experience = points;
        }

        public static BindingList<PlayerSkill> GetDefaultList()
        {
            BindingList<PlayerSkill> skills = new BindingList<PlayerSkill>();
            foreach (var type in SKILL_TYPES)
            {
                skills.Add(new PlayerSkill(type));
            }
            return skills;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
