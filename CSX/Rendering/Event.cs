using CSX.Events;

namespace CSX.Rendering
{
    public class Event
    {
        public Event(ulong elementId, NativeEvent eventType, CSXEventArgs payload)
        {
            ElementId = elementId;
            EventType = eventType;
            Payload = payload;
        }

        public ulong ElementId { get; }
        public NativeEvent EventType { get; }
        public CSXEventArgs Payload { get; }
    }
}
