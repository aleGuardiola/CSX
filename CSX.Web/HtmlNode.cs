using CSX.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Web
{
    internal class HtmlNode
    {
        public HtmlNode(ulong id, NativeElement element)
        {
            Id = id;
            Element = element;
        }

        public ulong Id { get; }
        public string Text { get; set; } = "";
        public NativeElement Element { get; }
        public Dictionary<string, string> HtmlStyle { get; set; } = new Dictionary<string, string>();
        public HtmlNode? Parent { get; set; }
        public Dictionary<NativeAttribute, object> Attributes { get; } = new Dictionary<NativeAttribute, object>();
        public List<HtmlNode> Children { get; } = new List<HtmlNode>();
    }
}
