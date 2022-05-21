using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Rendering
{
    public class MemoryNode
    {
        public MemoryNode(ulong id, NativeElement element)
        {
            Id = id;
            Element = element;
        }

        public ulong Id { get; }
        public NativeElement Element { get; }
        public string Text { get; set; } = "";
        public MemoryNode? Parent { get; set; }
        public Dictionary<NativeAttribute, object> Attributes { get; } = new Dictionary<NativeAttribute, object>();
        public List<MemoryNode> Children { get; } = new List<MemoryNode>();
    }
}
