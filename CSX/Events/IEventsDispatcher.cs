using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Events
{
    public interface IEventsDispatcher
    {
        public void DispatchEvent<T>(string type, T data);
        protected internal void DispatchEvent<T>(string type, Event<T> @event);
    }
}
