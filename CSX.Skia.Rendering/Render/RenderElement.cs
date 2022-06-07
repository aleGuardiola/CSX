using CSX.Skia.Rendering.Graphic;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Skia.Rendering.Render
{
    public abstract class RenderElement
    {
        public SKRect Rect { get; init; }
        
        // TODO filter
        // TODO reflection

        public RenderElement(SKRect rect)
        {
            Rect = rect;
        }

        public abstract void Paint(GraphicContext context);
    }
}
