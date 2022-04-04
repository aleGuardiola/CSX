using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Events
{
    public class EventDispatcherHandler : IEventsDispatcher, IEventsHandler
    {
        Dictionary<string, List<object>> handlers = new Dictionary<string, List<object>>();
        
        public void AddEventHandler<T>(string type, Action<Event<T>> handler)
        {
            if (handlers.TryGetValue(type, out var list))
            {
                list.Add(handler);
            }
            else
            {
                list = new List<object>();
                list.Add(handler);
                handlers.Add(type, list);
            }
        }

        public void DispatchEvent<T>(string type, T data)
        {
            var @event = new Event<T>(data);

            if (handlers.TryGetValue(type, out var handlersList))
            {
                foreach (var handler in handlersList)
                {
                    ((Action<Event<T>>)handler)(@event);
                }
            }
        }

        void IEventsDispatcher.DispatchEvent<T>(string type, Event<T> @event)
        {
            throw new NotImplementedException();
        }
    }
}
