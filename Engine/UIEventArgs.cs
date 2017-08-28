using System;

namespace Engine
{
    public enum UIEventType { ShowVendor, ShowGathering };

    public class UIEventArgs : EventArgs
    {
        public UIEventType Type { get; private set; }
        public int Data { get; private set; }

        public UIEventArgs(UIEventType type, int data)
        {
            Type = type;
            Data = data;
        }
    }
}