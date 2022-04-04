using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Events
{
    public interface IEventsHandler
    {
        void AddEventHandler<T>(string type, Action<Event<T>> handler);
    }
}
