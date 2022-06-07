using CSX.Skia.Rendering.Graphic;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Skia.Rendering.Render
{
    public class ImageRenderElement : RenderElement
    {        
        public SKImage? Image { get; init; }

        public ImageRenderElement(SKRect rect) : base(rect)
        {
        }

        public override void Paint(GraphicContext context)
        {
            throw new NotImplementedException();
        }
    }
}
