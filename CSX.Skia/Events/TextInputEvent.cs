using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Skia.Events
{
    public class TextInputEvent : WindowEvent
    {
        public TextInputEvent(ulong id, int unicode) : base(id)
        {
            Unicode = unicode;
        }

        public int Unicode { get; }
    }
}
