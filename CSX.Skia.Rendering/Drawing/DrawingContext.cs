using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Skia.Rendering.Drawing
{
    public class DrawingContext
    {
        public DrawingContext(SKCanvas canvas)
        {
            Canvas = canvas;
        }

        SKCanvas Canvas { get; }
    }
}
