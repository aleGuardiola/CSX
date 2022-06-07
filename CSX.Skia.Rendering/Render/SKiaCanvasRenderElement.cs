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
    public class SKiaCanvasRenderElement : RenderElement
    {
        public SKiaCanvasRenderElement(SKRect rect) : base(rect)
        {
        }

        public override void Paint(GraphicContext context)
        {            
            throw new NotImplementedException();
        }
    }
}
