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

        public void AddItemToInventory(Item itemToAdd, int quantity = 1)
        {
            InventoryItem item = Inventory.SingleOrDefault(ii => ii.Data.ID == itemToAdd.ID);
            if (item == null)
            {
                Inventory.Add(new InventoryItem(itemToAdd, quantity));
            }
            else
            {
                item.Quantity += quantity;
            }
            OnPropertyChanged("Inventory");
        }

        public void RemoveItemFromInventory(Item itemToRemove, int quantity = 1)
        {
            InventoryItem item = Inventory.SingleOrDefault(ii => ii.Data.ID == itemToRemove.ID);

            if (item != null)
            {
                item.Quantity -= quantity;
                
                if (item.Quantity <= 0)
                {
                    Inventory.Remove(item);
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
