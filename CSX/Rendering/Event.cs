using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CSX.Rendering
{
    public class Event
    {
        public Event(ulong elementId, string eventName, JsonElement payload)
        {
            ElementId = elementId;
            EventName = eventName;
            Payload = payload;
        }

        public ulong ElementId { get; }
        public string EventName { get; }
        public JsonElement Payload { get; }

    }
}
