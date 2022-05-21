using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Rendering
{
    public class NativeAttributeAttribute : Attribute
    {
        NativeAttribute Attribute { get; }

        public NativeAttributeAttribute(NativeAttribute attribute)
        {

        }
    }
}
