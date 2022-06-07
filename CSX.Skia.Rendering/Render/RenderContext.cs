using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Skia.Rendering.Render;

public class RenderContext
{    
    public float ParentX { get; private set; }
    public float ParentY { get; private set; }

    public List<RenderElement> _renderElements = new List<RenderElement>();

    public RenderContext(float parentX, float parentY)
    {
        ParentX = parentX;
        ParentY = parentY;        
    }

    public IReadOnlyCollection<RenderElement> Elements => _renderElements.AsReadOnly();

    public void Add(RenderElement renderElement)
    {
        _renderElements.Add(renderElement);
    }
}