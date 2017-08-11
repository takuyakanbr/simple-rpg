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
        private int _experiencePoints;
        private int _level;
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
        public int ExperiencePoints
        {
            get { return _experiencePoints; }
            private set
            {
                _experiencePoints = value;
                OnPropertyChanged("ExperiencePoints");
            }
        }
        public int Level
        {
            get { return _level; }
            private set
            {
                _level = value;
                OnPropertyChanged("Level");
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
        public BindingList<InventoryItem> Inventory { get; set; }
        public BindingList<PlayerQuest> Quests { get; set; }
        public BindingList<ItemEquipment> Equipment { get; set; }

        // calculated values
        public int MinDamage { get; private set; }
        public int MaxDamage { get; private set; }
        public int Defence { get; private set; }
        public List<ItemConsumable> Consumables
        {
            get
            {
                return Inventory.Where(
                    x => x.Data is ItemConsumable).Select(
                    x => x.Data as ItemConsumable).ToList();
            }
        }

        private Player(int currentHitPoints, int maximumHitPoints,
            int gold, int experiencePoints, int level)
        {
            CurrentHitPoints = currentHitPoints;
            MaximumHitPoints = maximumHitPoints;
            Gold = gold;
            ExperiencePoints = experiencePoints;
            Level = level;
            Inventory = new BindingList<InventoryItem>();
            Quests = new BindingList<PlayerQuest>();
            Equipment = new BindingList<ItemEquipment>();
        }

        public void AddExperiencePoints(int points)
        {
            ExperiencePoints += points;
            if (Level >= 100)
            {
                return; // Maximum level is 100
            }
            if (ExperiencePoints >= Level * 100 + Math.Pow(Level, 2) * 100)
            {
                Level += 1;
                MaximumHitPoints += 10;
                CurrentHitPoints += 10;
            }
        }
        
        public void AddItemToInventory(Item itemToAdd, int quantity = 1)
        {
            InventoryItem item = Inventory.SingleOrDefault(
                ii => ii.Data.ID == itemToAdd.ID);

            if (item == null)
            {
                Inventory.Add(new InventoryItem(itemToAdd, quantity));
            }
            else
            {
                item.Quantity += quantity;
            }

            RaiseInventoryChangedEvent(itemToAdd);
        }

        public int GetQuestState(Quest quest)
        {
            foreach (PlayerQuest playerQuest in Quests)
            {
                if (playerQuest.Data.ID == quest.ID)
                {
                    return playerQuest.State;
                }
            }
            return -1;
        }

        public bool HasQuest(Quest quest)
        {
            return Quests.Any(pq => pq.Data.ID == quest.ID);
        }

        public bool IsQuestComplete(Quest quest)
        {
            foreach (PlayerQuest playerQuest in Quests)
            {
                if (playerQuest.Data.ID == quest.ID)
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

        public void RecalculateStats()
        {
            int minDamage = 0, maxDamage = 2, defence = 0;
            foreach (var equipment in Equipment)
            {
                minDamage += equipment.MinDamage;
                maxDamage += equipment.MaxDamage;
                defence += equipment.Defence;
            }
            MinDamage = minDamage;
            MaxDamage = maxDamage;
            Defence = defence;
        }

        public void RemoveItemFromInventory(Item itemToRemove, int quantity = 1)
        {
            InventoryItem item = Inventory.SingleOrDefault(
                ii => ii.Data.ID == itemToRemove.ID);
            if (item != null)
            {
                item.Quantity -= quantity;

                if (item.Quantity <= 0)
                {
                    Inventory.Remove(item);
                }

                RaiseInventoryChangedEvent(itemToRemove);
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

            XmlNode experiencePoints = saveData.CreateElement("ExperiencePoints");
            experiencePoints.AppendChild(saveData.CreateTextNode(ExperiencePoints.ToString()));
            stats.AppendChild(experiencePoints);

            XmlNode level = saveData.CreateElement("Level");
            level.AppendChild(saveData.CreateTextNode(Level.ToString()));
            stats.AppendChild(level);

            XmlNode homeTile = saveData.CreateElement("HomeTile");
            homeTile.AppendChild(saveData.CreateTextNode(HomeTileID.ToString()));
            stats.AppendChild(homeTile);

            XmlNode currentTile = saveData.CreateElement("CurrentTile");
            currentTile.AppendChild(saveData.CreateTextNode(CurrentTile.ID.ToString()));
            stats.AppendChild(currentTile);

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

                inventoryItems.AppendChild(itemEquipment);
            }

            return saveData.InnerXml;
        }

        public void UpdateQuest(Quest quest, int state, bool isComplete = false)
        {
            PlayerQuest playerQuest = Quests.SingleOrDefault(
                pq => pq.Data.ID == quest.ID);

            if (playerQuest != null)
            {
                playerQuest.State = state;
                playerQuest.IsComplete = isComplete;
            }
        }

        public static Player CreateDefaultPlayer()
        {
            Player player = new Player(100, 100, 20, 0, 1);
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
                int experiencePoints = Convert.ToInt32(saveData.SelectSingleNode("/Player/Stats/ExperiencePoints").InnerText);
                int level = Convert.ToInt32(saveData.SelectSingleNode("/Player/Stats/Level").InnerText);

                Player player = new Player(currentHitPoints, maximumHitPoints, gold, experiencePoints, level);
                player.HomeTileID = Convert.ToInt32(saveData.SelectSingleNode("/Player/Stats/HomeTile").InnerText);

                int currentTileID = Convert.ToInt32(saveData.SelectSingleNode("/Player/Stats/CurrentTile").InnerText);
                player.CurrentTile = World.GetTile(currentTileID);
                
                foreach (XmlNode node in saveData.SelectNodes("/Player/InventoryItems/InventoryItem"))
                {
                    int id = Convert.ToInt32(node.Attributes["ID"].Value);
                    int quantity = Convert.ToInt32(node.Attributes["Quantity"].Value);

                    if (quantity > 0)
                        player.AddItemToInventory(World.GetItem(id), quantity);
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
