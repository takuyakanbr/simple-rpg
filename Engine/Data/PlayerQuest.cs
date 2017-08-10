using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace Engine.Data
{
    public class PlayerQuest : INotifyPropertyChanged
    {
        private Quest _data;
        private bool _complete;

        public Quest Data
        {
            get { return _data; }
            set
            {
                _data = value;
                OnPropertyChanged("Data");
            }
        }
        public bool IsComplete
        {
            get { return _complete; }
            set
            {
                _complete = value;
                OnPropertyChanged("IsComplete");
            }
        }
        public int State { get; set; }

        public PlayerQuest(Quest data, int state = 0)
        {
            Data = data;
            IsComplete = false;
            State = state;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
