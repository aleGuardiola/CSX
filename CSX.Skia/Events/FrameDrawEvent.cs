using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Skia.Events
{
    public class FrameDrawEvent : WindowEvent
    {
        public FrameDrawEvent(double time)
        {
            Time = time;
        }

        public double Time { get; }
    }
}
