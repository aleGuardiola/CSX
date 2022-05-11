using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Components
{
    public class Element
    {
        public Type Type { get; }
        public Props Props { get; }
        public Element[] Children { get; set; }
        public IComponent? Component { get; set; } = null;
        public Element(Type type, Props props, Element[] children)
        {
            Type = type;
            Props = props;
            Children = children;
        }
    }
        
}
