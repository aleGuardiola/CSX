using CSX.Native;
using CSX.Rendering;

namespace CSX.Skia
{
    public class SkiaDom : IDOM
    {

        

        public SkiaDom()
        {

        }

        public IObservable<Event> Events => throw new NotImplementedException();

        public void AppendChild(ulong parent, ulong child)
        {
            throw new NotImplementedException();
        }

        public void AppendDom(IDOM dom)
        {
            throw new NotImplementedException();
        }

        public ulong CreateElement(NativeElement element)
        {
            throw new NotImplementedException();
        }

        public IDOM CreateNewMemoryDom()
        {
            throw new NotImplementedException();
        }

        public void DestroyElement(ulong id)
        {
            throw new NotImplementedException();
        }

        public object? GetAttribute(ulong id, NativeAttribute name)
        {
            throw new NotImplementedException();
        }

        public ulong[] GetChildren(ulong parent)
        {
            throw new NotImplementedException();
        }

        public string GetElementText(ulong id)
        {
            throw new NotImplementedException();
        }

        public ulong GetRootElement()
        {
            throw new NotImplementedException();
        }

        public bool HasChild(ulong parent, ulong child)
        {
            throw new NotImplementedException();
        }

        public void Remove(ulong id)
        {
            throw new NotImplementedException();
        }

        public void SetAttribute(ulong id, NativeAttribute name, object? value)
        {
            throw new NotImplementedException();
        }

        public void SetAttributes(ulong id, KeyValuePair<NativeAttribute, object?>[] attributes)
        {
            throw new NotImplementedException();
        }

        public void SetChildren(ulong id, ulong[] children)
        {
            throw new NotImplementedException();
        }

        public void SetElementText(ulong id, string text)
        {
            throw new NotImplementedException();
        }

        public bool SupportAppendingDom()
        {
            throw new NotImplementedException();
        }
    }
}