using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Skia.Events
{
    public class MouseDownEvent : WindowEvent
    {
        public MouseDownEvent(ulong id, CSXSkiaMouseButton mouseButton) : base(id)
        {
            MouseButton = mouseButton;
        }

        public CSXSkiaMouseButton MouseButton { get; }
    }
}
