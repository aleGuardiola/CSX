using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Skia
{
    public record DrawContext
    {
        public ScrollBarRenderer ScrollBarRenderer = new DefaultScrollBarRenderer();

        public SKImageInfo ImageInfo { get; set; }

        private List<SKSurface> _surfaces = new List<SKSurface>();
        public IReadOnlyCollection<SKSurface> Surfaces => _surfaces.AsReadOnly();

        public SKCanvas GetCanvas(int level)
        {
            if(level == _surfaces.Count)
            {
                var surface = SKSurface.Create(ImageInfo);
                _surfaces.Add(surface);
                return surface.Canvas;
            }
            else
            {
                return _surfaces[level].Canvas;
            }
        }

        public float RelativeToX = 0f;
        public float RelativeToY = 0f;
    }
}
