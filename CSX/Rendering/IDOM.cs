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
        ulong CreateElement(NativeElement element);
        void Remove(ulong id);
        void SetChildren(ulong id, ulong[] children);
        void SetAttribute(ulong id, NativeAttribute name, object? value);
        void SetAttributes(ulong id, KeyValuePair<NativeAttribute, object?>[] attributes);
        void SetElementText(ulong id, string text);
        string GetElementText(ulong id);
        object? GetAttribute(ulong id, NativeAttribute name);
        void AppendChild(ulong parent, ulong child); 
        bool HasChild(ulong parent, ulong child);
        ulong[] GetChildren(ulong parent);
        void DestroyElement(ulong id);        
        
        bool SupportAppendingDom();
        void AppendDom(IDOM dom);
        IDOM CreateNewMemoryDom();   
        void RunOnUIThread(Action<double> action, bool forNextFrame = false)
        {
            double deltaTime = 33;
            Task.Delay((int)deltaTime).ContinueWith((t) => action(deltaTime));            
        }
    }   

}
