using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Engine.Data
{
    public class InventoryItem : INotifyPropertyChanged
    {
        private Item _data;
        private int _quantity;

        public Item Data
        {
            get { return _data; }
            set
            {
                _data = value;
                OnPropertyChanged("Data");
            }
        }
        public int Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                OnPropertyChanged("Quantity");
            }
        }

        public int ID { get { return Data.ID; } }
        public string Name { get { return Data.Name; } }
        public int Price { get { return Data.Price; } }
        public int BuyPrice { get { return Data.BuyPrice; } }

        public InventoryItem(Item data, int quantity)
        {
            Data = data;
            Quantity = quantity;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
