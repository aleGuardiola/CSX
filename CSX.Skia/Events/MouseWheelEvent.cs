using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Skia.Events
{
    public class MouseWheelEvent : WindowEvent
    {
        public MouseWheelEvent(ulong id, float offsetX, float offsetY) : base(id)
        {
            OffsetX = offsetX;
            OffsetY = offsetY;
        }

        public float OffsetX { get; }
        public float OffsetY { get; }
    }
}
