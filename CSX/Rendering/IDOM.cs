using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Rendering
{
    public interface IDOM
    {
        Guid GetRootElement();
        Guid CreateElement(string name);
        void Remove(Guid id);
        void SetAttribute(Guid id, string name, string? value);
        string? GetAttribute(Guid id, string name);
        void AppendChild(Guid parent, Guid child);        
        bool HasChild(Guid parent, Guid child);
        void DestroyElement(Guid id);
    }
}
