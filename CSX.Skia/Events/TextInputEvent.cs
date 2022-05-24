using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Skia.Events
{
    public class TextInputEvent : WindowEvent
    {
        public TextInputEvent(int unicode)
        {
            Unicode = unicode;
        }

        public int Unicode { get; }
    }
}
