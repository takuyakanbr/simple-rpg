using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

using Engine.Data;

namespace Engine
{
    public class GameState : INotifyPropertyChanged
    {
        private const string SAVE_DATA_FILE_NAME = "SaveData.xml";

        private Player _player;
        private Monster _currentMonster;
        private int _monsterHitPoints;

        public Player Player { get; private set; }
        public BindingList<Entity> EntitiesOnTile { get; private set; }
        public Monster CurrentMonster
        {
            get { return _currentMonster; }
            private set
            {
                _currentMonster = value;
                OnPropertyChanged("CurrentMonster");
            }
        }
        public Tile CurrentTile { get { return Player.CurrentTile; } }
        public Vendor CurrentVendor { get; private set; }

        public Entity CurrentEntity { get; private set; }
        public int InteractState = 0;
        public int InteractChoice = 0;

        public event EventHandler<MessageEventArgs> OnMessage;

        public GameState()
        {
            EntitiesOnTile = new BindingList<Entity>();
        }

        public void Attack()
        {
            if (CurrentMonster == null) return;
            
            int damage = RandomGenerator.Next(Player.MinDamage, Player.MaxDamage);
            damage -= CurrentMonster.Defence;
            if (damage < 0) damage = 0;
            
            _monsterHitPoints -= damage;
            RaiseMessage("You hit the " + CurrentMonster.Name + " for " + damage + " points.");
            
            if (_monsterHitPoints <= 0)
            {
                // Monster is dead
                RaiseMessage("");
                RaiseMessage("You defeated the " + CurrentMonster.Name);

                // Give player experience points for killing the monster
                Player.AddExperiencePoints(CurrentMonster.RewardXP);
                RaiseMessage("You receive " + CurrentMonster.RewardXP + " experience points");

                // Give player gold for killing the monster
                var gold = RandomGenerator.Next(CurrentMonster.MinGold, CurrentMonster.MaxGold);
                Player.Gold += gold;
                RaiseMessage("You receive " + gold + " gold");

                // Get random loot from the monster
                foreach (var lootItem in CurrentMonster.LootTable)
                {
                    if (RandomGenerator.NextDouble() <= lootItem.DropChance)
                    {
                        int quantity = RandomGenerator.Next(lootItem.MinQuantity, lootItem.MaxQuantity);
                        Player.AddItemToInventory(lootItem.Data, quantity);
                        RaiseMessage("You loot " + quantity + " " + lootItem.Data.Name);
                    }
                }

                RaiseMessage("");
            }
            else
            {
                // Monster is still alive
                DoMonsterMove();
            }
        }

        public void DoMonsterMove()
        {
            if (CurrentMonster == null) return;

            int damage = RandomGenerator.Next(CurrentMonster.MinDamage, CurrentMonster.MaxDamage);
            damage -= Player.Defence;
            if (damage < 0) damage = 0;

            Player.CurrentHitPoints -= damage;
            RaiseMessage("The " + _currentMonster.Name + " did " + damage + " points of damage.");

            if (Player.CurrentHitPoints <= 0)
            {
                RaiseMessage("The " + _currentMonster.Name + " killed you.");
                MoveHome();
            }
        }

        public void Interact(Entity entity)
        {

        }

        public void LoadProfile()
        {
            if (File.Exists(SAVE_DATA_FILE_NAME))
            {
                Player = Player.CreatePlayerFromXmlString(
                    File.ReadAllText(SAVE_DATA_FILE_NAME));
            }
            else
            {
                Player = Player.CreateDefaultPlayer();
            }
        }

        public void MoveTo(Tile tile)
        {
            if (tile.OnEnter != null && !tile.OnEnter(this, Player))
            {
                // Cannot enter this tile
                return;
            }

            CurrentVendor = null;
            CurrentEntity = null;
            InteractState = 0;
            InteractChoice = 0;

            // Update entities
            EntitiesOnTile.Clear();
            foreach (var entity in tile.Entities)
            {
                if (entity.OnCreate == null || entity.OnCreate(this, Player))
                    EntitiesOnTile.Add(entity);
            }

            // Create a new monster encounter
            if (tile.Monsters.Count() > 0)
            {
                CurrentMonster = tile.Monsters.ElementAt(RandomGenerator.Next(0, tile.Monsters.Count() - 1));
                _monsterHitPoints = CurrentMonster.HitPoints;
            }
            else
            {
                CurrentMonster = null;
            }

            Player.CurrentHitPoints += 1;
            Player.CurrentTile = tile;
        }

        private void MoveHome()
        {
            MoveTo(World.GetTile(Player.HomeTileID));
        }

        public void MoveNorth()
        {
            if (CurrentTile.North != null)
            {
                MoveTo(CurrentTile.North);
            }
        }

        public void MoveEast()
        {
            if (CurrentTile.East != null)
            {
                MoveTo(CurrentTile.East);
            }
        }

        public void MoveSouth()
        {
            if (CurrentTile.South != null)
            {
                MoveTo(CurrentTile.South);
            }
        }

        public void MoveWest()
        {
            if (CurrentTile.West != null)
            {
                MoveTo(CurrentTile.West);
            }
        }
        
        public void OpenEquipment()
        {

        }

        public void RaiseMessage(string message, bool addExtraNewLine = false)
        {
            OnMessage?.Invoke(this, new MessageEventArgs(message, addExtraNewLine));
        }

        public void SaveProfile()
        {
            File.WriteAllText(SAVE_DATA_FILE_NAME, _player.ToXmlString());
        }

        public void UseConsumable(ItemConsumable consumable)
        {
            Player.CurrentHitPoints += consumable.HitPoints;
            Player.RemoveItemFromInventory(consumable, 1);
            RaiseMessage("You consume a " + consumable.Name);
            
            DoMonsterMove();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
