using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Components
{
    public class VirtualComponent
    {
        public Type Type { get; }
        public Props Props { get; }
        public VirtualComponent[] Children { get; }
        public IComponent? Component { get; set; }
        public VirtualComponent(Type type, Props props, VirtualComponent[] children)
        {
            Type = type;
            Props = props;
            Children = children;
        }
    }
        
}
