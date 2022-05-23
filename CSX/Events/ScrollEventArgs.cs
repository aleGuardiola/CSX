using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Events
{
    public class ScrollEventArgs : CSXEventArgs
    {
        public ScrollEventArgs(int x, int y)
        {
            X = x;
            Y = y;
        }

        public int X { get; }
        public int Y { get; }
    }
}
