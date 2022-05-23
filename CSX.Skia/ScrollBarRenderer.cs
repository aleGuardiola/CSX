using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Skia
{
    public abstract class ScrollBarRenderer
    {
        public float ScrollBarWidth = 17f;

        public abstract void Render(SKCanvas canvas, float x, float y, float height, float totalContentLenght, float scrollPosition);
    }
}
