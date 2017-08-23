using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml;

using Engine.Data;

namespace Engine
{
    public class Player : INotifyPropertyChanged
    {
        private int _currentHitPoints;
        private int _gold;
        private Tile _currentTile;

        public int CurrentHitPoints
        {
            get { return _currentHitPoints; }
            set
            {
                _currentHitPoints = value;
                if (_currentHitPoints > MaximumHitPoints) _currentHitPoints = MaximumHitPoints;
                if (_currentHitPoints < 0) _currentHitPoints = 0;
                OnPropertyChanged("CurrentHitPoints");
            }
        }
        public int MaximumHitPoints { get; set; }
        public int Gold
        {
            get { return _gold; }
            set
            {
                _gold = value;
                OnPropertyChanged("Gold");
            }
        }
        public Tile CurrentTile {
            get { return _currentTile; }
            set
            {
                _currentTile = value;
                OnPropertyChanged("CurrentTile");
            }
        }
        public int HomeTileID = 0;
        public BindingList<PlayerSkill> Skills { get; set; }
        public BindingList<InventoryItem> Inventory { get; set; }
        public BindingList<PlayerQuest> Quests { get; set; }
        public BindingList<ItemEquipment> Equipment { get; set; }

        // calculated values
        public int MinDamage { get; private set; }
        public int MaxDamage { get; private set; }
        public int Defence { get; private set; }
        public List<ItemEquipment> Equippable
        {
            get
            {
                return Inventory.Where(
                    x => x.Data is ItemEquipment).Select(
                    x => x.Data as ItemEquipment).ToList();
            }
        }
        public List<ItemConsumable> Consumables
        {
            get
            {
                return Inventory.Where(
                    x => x.Data is ItemConsumable).Select(
                    x => x.Data as ItemConsumable).ToList();
            }
        }

        private Player(int currentHitPoints, int maximumHitPoints, int gold)
        {
            MaximumHitPoints = maximumHitPoints;
            CurrentHitPoints = currentHitPoints;
            Gold = gold;
            Skills = PlayerSkill.GetDefaultList();
            Inventory = new BindingList<InventoryItem>();
            Quests = new BindingList<PlayerQuest>();
            Equipment = new BindingList<ItemEquipment>();
        }

        public void AddExperience(SkillType type, int points)
        {
            Skills[(int)type].AddExperience(points);
        }

        public void AddCombatExperience(int points)
        {
            Skills[(int)SkillType.Combat].AddExperience(points);
        }
        
        public void AddItemToInventory(Item item, int quantity = 1)
        {
            InventoryItem invItem = Inventory.SingleOrDefault(ii => ii.ID == item.ID);

            if (invItem == null)
            {
                Inventory.Add(new InventoryItem(item, quantity));
            }
            else
            {
                invItem.Quantity += quantity;
            }

            RaiseInventoryChangedEvent(item);
        }

        public void AddItemToInventory(int itemID, int quantity = 1)
        {
            InventoryItem invItem = Inventory.SingleOrDefault(ii => ii.ID == itemID);

            if (invItem == null)
            {
                invItem = new InventoryItem(World.GetItem(itemID), quantity);
                Inventory.Add(invItem);
            }
            else
            {
                invItem.Quantity += quantity;
            }

            RaiseInventoryChangedEvent(invItem.Data);
        }
        
        // mark the specified quest as completed and give the player the rewards
        public void CompleteQuest(int questID)
        {
            PlayerQuest playerQuest = Quests.SingleOrDefault(
                pq => pq.ID == questID);

            if (playerQuest != null)
            {
                foreach (var item in playerQuest.Data.RewardItems)
                {
                    AddItemToInventory(item.Data, item.Quantity);
                }
                AddCombatExperience(playerQuest.Data.RewardXP);
                Gold += playerQuest.Data.RewardGold;
                playerQuest.IsComplete = true;
            }
        }

        // equips the specified item, returns true if successful
        public bool EquipItem(int itemID)
        {
            InventoryItem item = Inventory.SingleOrDefault(ii => ii.ID == itemID);
            if (item != null && item.Data is ItemEquipment)
            {
                ItemEquipment itemEquipment = (ItemEquipment)item.Data;
                RemoveItemFromInventory(itemID);

                // remove equipped item occupying the same slot
                ItemEquipment itemInSlot = Equipment.SingleOrDefault(ie => ie.Type == itemEquipment.Type);
                if (itemInSlot != null)
                    UnequipItem(itemInSlot.ID);

                Equipment.Add(itemEquipment);
                RecalculateStats();
                return true;
            }
            return false;
        }

        public int GetLevel(SkillType type = SkillType.Combat)
        {
            return Skills[(int)type].Level;
        }

        // gets the state of the specified quest;
        // returns -1 if the player has not received it yet
        public int GetQuestState(int questID)
        {
            foreach (PlayerQuest playerQuest in Quests)
            {
                if (playerQuest.ID == questID)
                {
                    return playerQuest.State;
                }
            }
            return -1;
        }

        public bool HasItem(int itemID, int quantity = 1)
        {
            InventoryItem invItem = Inventory.SingleOrDefault(ii => ii.ID == itemID);
            return invItem != null && invItem.Quantity >= quantity;
        }

        public bool HasQuest(int questID)
        {
            return Quests.Any(pq => pq.ID == questID);
        }
        
        public bool HasQuestPrerequisites(int questID)
        {
            Quest quest = World.GetQuest(questID);
            foreach (int req in quest.QuestRequirements)
            {
                if (!IsQuestComplete(req))
                    return false;
            }
            return GetLevel() >= quest.LevelRequirement;
        }

        public bool HasTypeEquipped(EquipmentType type)
        {
            ItemEquipment item = Equipment.SingleOrDefault(ie => ie.Type == type);
            return item != null;
        }

        public bool IsQuestComplete(int questID)
        {
            foreach (PlayerQuest playerQuest in Quests)
            {
                if (playerQuest.ID == questID)
                {
                    return playerQuest.IsComplete;
                }
            }
            return false;
        }
        
        private void RaiseInventoryChangedEvent(Item item)
        {
            if (item is ItemConsumable)
            {
                OnPropertyChanged("Consumables");
            }
        }

        // recalculate the player's hp, attack and defence data
        public void RecalculateStats()
        {
            int minDamage = 0, maxDamage = 2, defence = 0, maxHP = 90 + GetLevel() * 10;
            foreach (var equipment in Equipment)
            {
                maxHP += equipment.HitPoints;
                minDamage += equipment.MinDamage;
                maxDamage += equipment.MaxDamage;
                defence += equipment.Defence;
            }
            MaximumHitPoints = maxHP;
            MinDamage = minDamage;
            MaxDamage = maxDamage;
            Defence = defence;
        }

        public void RemoveItemFromInventory(int itemID, int quantity = 1)
        {
            InventoryItem invItem = Inventory.SingleOrDefault(ii => ii.ID == itemID);
            if (invItem != null)
            {
                invItem.Quantity -= quantity;

                if (invItem.Quantity <= 0)
                {
                    Inventory.Remove(invItem);
                }

                RaiseInventoryChangedEvent(invItem.Data);
            }
        }

        public string ToXmlString()
        {
            XmlDocument saveData = new XmlDocument();

            // Create the top-level XML node
            XmlNode player = saveData.CreateElement("Player");
            saveData.AppendChild(player);

            // Create the "Stats" child node to hold the other player statistics nodes
            XmlNode stats = saveData.CreateElement("Stats");
            player.AppendChild(stats);

            // Create the child nodes for the "Stats" node
            XmlNode currentHitPoints = saveData.CreateElement("CurrentHitPoints");
            currentHitPoints.AppendChild(saveData.CreateTextNode(CurrentHitPoints.ToString()));
            stats.AppendChild(currentHitPoints);

            XmlNode maximumHitPoints = saveData.CreateElement("MaximumHitPoints");
            maximumHitPoints.AppendChild(saveData.CreateTextNode(MaximumHitPoints.ToString()));
            stats.AppendChild(maximumHitPoints);

            XmlNode gold = saveData.CreateElement("Gold");
            gold.AppendChild(saveData.CreateTextNode(Gold.ToString()));
            stats.AppendChild(gold);

            XmlNode homeTile = saveData.CreateElement("HomeTile");
            homeTile.AppendChild(saveData.CreateTextNode(HomeTileID.ToString()));
            stats.AppendChild(homeTile);

            XmlNode currentTile = saveData.CreateElement("CurrentTile");
            currentTile.AppendChild(saveData.CreateTextNode(CurrentTile.ID.ToString()));
            stats.AppendChild(currentTile);

            // Create the "PlayerSkills" child node to hold each PlayerSkill node
            XmlNode playerSkills = saveData.CreateElement("PlayerSkills");
            player.AppendChild(playerSkills);

            // Create an "PlayerSkill" node for each skill
            foreach (PlayerSkill skill in Skills)
            {
                XmlNode playerSkill = saveData.CreateElement("PlayerSkill");

                XmlAttribute typeAttribute = saveData.CreateAttribute("Type");
                typeAttribute.Value = skill.ID.ToString();
                playerSkill.Attributes.Append(typeAttribute);

                XmlAttribute levelAttribute = saveData.CreateAttribute("Level");
                levelAttribute.Value = skill.Level.ToString();
                playerSkill.Attributes.Append(levelAttribute);

                XmlAttribute experienceAttribute = saveData.CreateAttribute("Experience");
                experienceAttribute.Value = skill.Experience.ToString();
                playerSkill.Attributes.Append(experienceAttribute);

                playerSkills.AppendChild(playerSkill);
            }

            // Create the "InventoryItems" child node to hold each InventoryItem node
            XmlNode inventoryItems = saveData.CreateElement("InventoryItems");
            player.AppendChild(inventoryItems);

            // Create an "InventoryItem" node for each item in the player's inventory
            foreach (InventoryItem item in Inventory)
            {
                XmlNode inventoryItem = saveData.CreateElement("InventoryItem");

                XmlAttribute idAttribute = saveData.CreateAttribute("ID");
                idAttribute.Value = item.Data.ID.ToString();
                inventoryItem.Attributes.Append(idAttribute);

                XmlAttribute quantityAttribute = saveData.CreateAttribute("Quantity");
                quantityAttribute.Value = item.Quantity.ToString();
                inventoryItem.Attributes.Append(quantityAttribute);

                inventoryItems.AppendChild(inventoryItem);
            }

            // Create the "PlayerQuests" child node to hold each PlayerQuest node
            XmlNode playerQuests = saveData.CreateElement("PlayerQuests");
            player.AppendChild(playerQuests);

            // Create a "PlayerQuest" node for each quest the player has acquired
            foreach (PlayerQuest quest in Quests)
            {
                XmlNode playerQuest = saveData.CreateElement("PlayerQuest");

                XmlAttribute idAttribute = saveData.CreateAttribute("ID");
                idAttribute.Value = quest.Data.ID.ToString();
                playerQuest.Attributes.Append(idAttribute);

                XmlAttribute stateAttribute = saveData.CreateAttribute("State");
                stateAttribute.Value = quest.State.ToString();
                playerQuest.Attributes.Append(stateAttribute);

                XmlAttribute isCompletedAttribute = saveData.CreateAttribute("IsComplete");
                isCompletedAttribute.Value = quest.IsComplete.ToString();
                playerQuest.Attributes.Append(isCompletedAttribute);

                playerQuests.AppendChild(playerQuest);
            }

            // Create the "ItemEquipments" child node to hold each ItemEquipment node
            XmlNode itemEquipments = saveData.CreateElement("ItemEquipments");
            player.AppendChild(itemEquipments);

            // Create an "ItemEquipment" node for each equipped item
            foreach (ItemEquipment item in Equipment)
            {
                XmlNode itemEquipment = saveData.CreateElement("ItemEquipment");

                XmlAttribute idAttribute = saveData.CreateAttribute("ID");
                idAttribute.Value = item.ID.ToString();
                itemEquipment.Attributes.Append(idAttribute);

                itemEquipments.AppendChild(itemEquipment);
            }

            return saveData.InnerXml;
        }

        // unequips the specified item, returns true if successful
        public bool UnequipItem(int itemID)
        {
            ItemEquipment item = Equipment.SingleOrDefault(ie => ie.ID == itemID);
            if (item != null)
            {
                Equipment.Remove(item);
                AddItemToInventory(itemID);
                RecalculateStats();
                return true;
            }
            return false;
        }

        public void UpdateQuest(int questID, int state)
        {
            PlayerQuest playerQuest = Quests.SingleOrDefault(
                pq => pq.ID == questID);

            if (playerQuest == null)
            {
                Quests.Add(new PlayerQuest(World.GetQuest(questID), state, false));
            }
            else
            {
                playerQuest.State = state;
            }
        }

        public void UpdateSkill(int type, int level, int points)
        {
            Skills.ElementAt(type).Update(level, points);
        }

        public static Player CreateDefaultPlayer()
        {
            Player player = new Player(100, 100, 20);
            player.CurrentTile = World.GetTile(player.HomeTileID);

            return player;
        }

        public static Player CreatePlayerFromXmlString(string xmlSaveData)
        {
            try
            {
                XmlDocument saveData = new XmlDocument();

                saveData.LoadXml(xmlSaveData);

                int currentHitPoints = Convert.ToInt32(saveData.SelectSingleNode("/Player/Stats/CurrentHitPoints").InnerText);
                int maximumHitPoints = Convert.ToInt32(saveData.SelectSingleNode("/Player/Stats/MaximumHitPoints").InnerText);
                int gold = Convert.ToInt32(saveData.SelectSingleNode("/Player/Stats/Gold").InnerText);

                Player player = new Player(currentHitPoints, maximumHitPoints, gold);
                player.HomeTileID = Convert.ToInt32(saveData.SelectSingleNode("/Player/Stats/HomeTile").InnerText);

                int currentTileID = Convert.ToInt32(saveData.SelectSingleNode("/Player/Stats/CurrentTile").InnerText);
                player.CurrentTile = World.GetTile(currentTileID);
                
                foreach (XmlNode node in saveData.SelectNodes("/Player/PlayerSkills/PlayerSkill"))
                {
                    int type = Convert.ToInt32(node.Attributes["Type"].Value);
                    int level = Convert.ToInt32(node.Attributes["Level"].Value);
                    int experience = Convert.ToInt32(node.Attributes["Experience"].Value);
                    player.UpdateSkill(type, level, experience);
                }

                foreach (XmlNode node in saveData.SelectNodes("/Player/InventoryItems/InventoryItem"))
                {
                    int id = Convert.ToInt32(node.Attributes["ID"].Value);
                    int quantity = Convert.ToInt32(node.Attributes["Quantity"].Value);

                    if (quantity > 0)
                        player.AddItemToInventory(id, quantity);
                }

                foreach (XmlNode node in saveData.SelectNodes("/Player/PlayerQuests/PlayerQuest"))
                {
                    int id = Convert.ToInt32(node.Attributes["ID"].Value);
                    int state = Convert.ToInt32(node.Attributes["State"].Value);
                    bool isComplete = Convert.ToBoolean(node.Attributes["IsComplete"].Value);

                    player.Quests.Add(new PlayerQuest(World.GetQuest(id), state, isComplete));
                }
                
                foreach (XmlNode node in saveData.SelectNodes("/Player/ItemEquipments/ItemEquipment"))
                {
                    int id = Convert.ToInt32(node.Attributes["ID"].Value);

                    player.Equipment.Add((ItemEquipment)World.GetItem(id));
                }

                return player;
            }
            catch
            {
                // If there was an error with the XML data, return a default player object
                return Player.CreateDefaultPlayer();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
