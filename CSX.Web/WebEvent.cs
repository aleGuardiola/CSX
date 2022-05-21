using CSX.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CSX.Web
{
    public class WebEvent
    {
        public ulong ElementId { get; }
        public NativeEvent EventType { get; }
        public JsonElement Payload { get; }
    }
}
