using CSX.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Components
{
    public record StringProps(string? Value) : Props;
    public class StringComponent : DOMComponent<StringProps>
    {        
        protected override Guid OnInitialize(IDOM dom)
        {
            return Guid.Empty;
        }

        protected override void Render(IDOM dom)
        {
        
        }

        protected override void OnDestroy(IDOM dom)
        {
        
        }
    }
}
