using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Skia.Events
{
    public class KeyUpEvent : WindowEvent
    {
        public KeyUpEvent(ulong id, CSXSkiaKey key) : base(id)
        {
            Key = key;
        }

        public CSXSkiaKey Key { get; }
    }
}
