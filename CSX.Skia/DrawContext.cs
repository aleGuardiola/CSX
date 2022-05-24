using CSX.Skia.Input;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Skia
{
    public record DrawContext : IDisposable
    {
        public ScrollBarRenderer ScrollBarRenderer = new DefaultScrollBarRenderer();

        public SKImageInfo ImageInfo { get; set; }

        private List<SKSurface> _surfaces = new List<SKSurface>();
        public IReadOnlyCollection<SKSurface> Surfaces => _surfaces.AsReadOnly();
        public Func<SKSurface> SurfaceFactory { get; set; }

        public CSXSkiaCursor Cursor { get; private set; }

        public Action<CSXSkiaCursor>? CursorFactory { get; set; }

        public DrawContext()
        {
            SurfaceFactory = DefaultSurfaceFactory;
        }

        public SKCanvas GetCanvas(int level)
        {
            if(level == _surfaces.Count)
            {
                var surface = SurfaceFactory();
                _surfaces.Add(surface);
                return surface.Canvas;
            }
            else
            {
                return _surfaces[level].Canvas;
            }
        }

        public void SetCursor(CSXSkiaCursor cursor)
        {
            CursorFactory?.Invoke(cursor);
        }

        SKSurface DefaultSurfaceFactory()
        {
            return SKSurface.Create(ImageInfo);
        }

        public void Dispose()
        {
            foreach(var surface in _surfaces)
            {
                surface.Flush();
                surface.Dispose();
            }
        }

        public float RelativeToX = 0f;
        public float RelativeToY = 0f;
    }
}
