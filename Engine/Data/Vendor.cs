using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Engine.Data
{
    public class Vendor : INotifyPropertyChanged
    {
        public int ID;
        public string Name;
        public BindingList<InventoryItem> Inventory;

        public Vendor(int id, string name)
        {
            ID = id;
            Name = name;
        }
        
        // add item without checking for merges
        public Vendor AddItem(int itemID, int quantity = 1)
        {
            Inventory.Add(new InventoryItem(World.GetItem(itemID), quantity));
            return this;
        }

        public void AddItemToInventory(Item itemToAdd, int quantity = 1)
        {
            InventoryItem invItem = Inventory.SingleOrDefault(ii => ii.Data.ID == itemToAdd.ID);
            if (invItem == null)
            {
                Inventory.Add(new InventoryItem(itemToAdd, quantity));
            }
            else
            {
                invItem.Quantity += quantity;
            }
            OnPropertyChanged("Inventory");
        }

        public void RemoveItemFromInventory(int itemID, int quantity = 1)
        {
            InventoryItem invItem = Inventory.SingleOrDefault(ii => ii.Data.ID == itemID);

            if (invItem != null)
            {
                invItem.Quantity -= quantity;
                
                if (invItem.Quantity <= 0)
                {
                    Inventory.Remove(invItem);
                }
                
                OnPropertyChanged("Inventory");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
