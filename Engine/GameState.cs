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
        public int InteractCounter = 0;
        public int InteractChoice = 0;

        public event EventHandler<MessageEventArgs> OnMessage;
        public event EventHandler<DialogEventArgs> OnDialogEvent;
        public event EventHandler<UIEventArgs> OnUIEvent;

        public GameState()
        {
            EntitiesOnTile = new BindingList<Entity>();
        }

        // attempt to attack the current monster
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
                Player.AddCombatExperience(CurrentMonster.RewardXP);
                RaiseMessage("You receive " + CurrentMonster.RewardXP + " experience points");

                // Give player gold for killing the monster
                var gold = RandomGenerator.Next(CurrentMonster.MinGold, CurrentMonster.MaxGold);
                if (gold > 0)
                {
                    Player.Gold += gold;
                    RaiseMessage("You receive " + gold + " gold");
                }

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

                CurrentMonster.OnKill?.Invoke(this, Player);
                CurrentMonster = null;

                RaiseMessage("");
            }
            else
            {
                // Monster is still alive
                DoMonsterMove();
            }
        }

        // starts a new interaction with the given entity
        public void BeginInteraction(Entity entity)
        {
            CurrentEntity = entity;
            InteractState = 0;
            InteractCounter = 0;
            Interact(0);
        }

        // (internal) make the monster take a turn
        private void DoMonsterMove()
        {
            if (CurrentMonster == null) return;

            int damage = RandomGenerator.Next(CurrentMonster.MinDamage, CurrentMonster.MaxDamage);
            damage -= Player.Defence;
            if (damage < 0) damage = 0;

            TakeDamage(damage, CurrentMonster.Name);
        }

        // closes the dialog screen and end interaction
        public void EndDialog()
        {
            OnDialogEvent?.Invoke(this, new DialogEventArgs(DialogEventType.Close, null, null));
            CurrentEntity = null;
        }

        // continue dialog with the current entity
        public void Interact(int option)
        {
            if (CurrentEntity == null) return;
            InteractChoice = option;
            CurrentEntity.OnInteract(this, Player);
            InteractCounter++;
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
            CurrentVendor = null;
            CurrentEntity = null;
            Player.CurrentHitPoints += 1;

            // Update entities
            EntitiesOnTile.Clear();
            foreach (var entity in tile.Entities)
            {
                if (entity.OnCreate == null || entity.OnCreate(this, Player))
                    EntitiesOnTile.Add(entity);
            }

            // Create a new monster encounter
            bool newMonster = false;
            foreach (var monsterSpawn in tile.MonsterSpawns)
            {
                if (RandomGenerator.NextDouble() <= monsterSpawn.SpawnChance)
                {
                    CurrentMonster = monsterSpawn.Data;
                    _monsterHitPoints = CurrentMonster.HitPoints;
                    RaiseMessage("You see a " + CurrentMonster.Name);
                    newMonster = true;
                    if (monsterSpawn.HasInitiative)
                        DoMonsterMove();
                    break;
                }
            }
            if (!newMonster) CurrentMonster = null;

            Player.CurrentTile = tile;
        }

        private void MoveHome()
        {
            MoveTo(World.GetTile(Player.HomeTileID));
        }

        private void MoveDirection(Tile tile, string direction)
        {
            if (tile == null) return;
            if (tile.OnEnter != null && !tile.OnEnter(this, Player))
            {
                // Cannot enter this tile
                return;
            }

            RaiseMessage("You walk " + direction);
            MoveTo(tile);
        }

        public void MoveNorth()
        {
            MoveDirection(CurrentTile.North, "north");
        }

        public void MoveEast()
        {
            MoveDirection(CurrentTile.East, "east");
        }

        public void MoveSouth()
        {
            MoveDirection(CurrentTile.South, "south");
        }

        public void MoveWest()
        {
            MoveDirection(CurrentTile.West, "west");
        }
        
        // append a message to the message textbox
        public void RaiseMessage(string message, bool addExtraNewLine = false)
        {
            OnMessage?.Invoke(this, new MessageEventArgs(message, addExtraNewLine));
        }

        public void SaveProfile()
        {
            File.WriteAllText(SAVE_DATA_FILE_NAME, Player.ToXmlString());
        }

        // show/update the dialog screen
        public void ShowDialog(string text, string[] options = null)
        {
            OnDialogEvent?.Invoke(this, 
                new DialogEventArgs(DialogEventType.Update, CurrentEntity.Name + ": " + text, options));
        }

        public void ShowVendor(int vendorID)
        {
            CurrentVendor = World.GetVendor(vendorID);
            OnUIEvent?.Invoke(this, new UIEventArgs(UIEventType.ShowVendor, vendorID));
        }

        public void TakeDamage(int damage, string source = null)
        {
            Player.CurrentHitPoints -= damage;
            if (source != null)
                RaiseMessage("The " + source + " did " + damage + " points of damage.");

            if (Player.CurrentHitPoints <= 0)
            {
                if (source != null)
                    RaiseMessage("The " + source + " killed you.");
                else
                    RaiseMessage("You died.");
                MoveHome();
            }
        }

        // consume the specified consumable - this lets the monster take a turn
        public void UseConsumable(ItemConsumable consumable)
        {
            Player.CurrentHitPoints += consumable.HitPoints;
            Player.RemoveItemFromInventory(consumable.ID, 1);
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
