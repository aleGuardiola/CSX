using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Events
{
    public class TextChangeEventArgs : EventArgs
    {
        public TextChangeEventArgs(string text)
        {
            Text = text;
        }

        public string Text { get; }
    }
}
