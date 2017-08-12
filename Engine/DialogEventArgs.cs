using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine
{
    public enum DialogEventType { Update, Close }

    public class DialogEventArgs : EventArgs
    {
        public DialogEventType Type;
        public string Dialog { get; private set; }
        public string[] Options { get; private set; }

        public DialogEventArgs(DialogEventType type, string dialog, string[] options)
        {
            Type = type;
            Dialog = dialog;
            Options = options;
        }
    }
}
