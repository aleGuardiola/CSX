using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Skia.Events
{
    public class OnMouseMoveEvent : WindowEvent
    {
        public OnMouseMoveEvent(ulong id, float x, float y) : base(id)
        {
            X = x;
            Y = y;
        }

        public float X { get; }
        public float Y { get; }
    }
}
