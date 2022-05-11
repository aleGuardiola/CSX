using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Rendering
{
    public interface IDOM
    {
        IObservable<Event> Events { get; }
        ulong GetRootElement();
        ulong CreateElement(string name);
        void Remove(ulong id);
        void SetChildren(ulong id, ulong[] children);
        void SetAttribute(ulong id, string name, string? value);
        void SetAttributes(ulong id, KeyValuePair<string, string?>[] attributes);
        void SetElementText(ulong id, string text);
        string? GetAttribute(ulong id, string name);
        void AppendChild(ulong parent, ulong child); 
        bool HasChild(ulong parent, ulong child);
        void DestroyElement(ulong id);
    }   

}
