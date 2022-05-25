using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Skia.Events
{
    public class WindowEvent
    {
        public WindowEvent(ulong id)
        {
            Id = id;
        }

        public ulong Id { get; }

        public bool Propagate { get; private set; } = true;

        public bool Handled { get; private set; } = false;

        public void MarkAsHandled()
            => Handled = true;

        public void DontPropagate()
            => Propagate = false;
    }
}
