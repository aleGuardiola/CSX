using CSX.Skia.Input;
using CSX.Skia.Views.ScrollBars;
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
        public SKImageInfo ImageInfo { get; set; }

        private List<SKSurface?> _surfaces = new List<SKSurface>();
        public IReadOnlyCollection<SKSurface> Surfaces => _surfaces.AsReadOnly();
        public Func<SKSurface> SurfaceFactory { get; set; }

        public CSXSkiaCursor Cursor { get; private set; }

        public Action<CSXSkiaCursor>? CursorFactory { get; set; }

        public bool ShowScreenDraws = false;
        public SKSurface? ScreenDrawsSurface { get; private set; }

        public DrawContext()
        {
            SurfaceFactory = DefaultSurfaceFactory;
        }

        public void SetDeep(uint deep)
        {
            if(_surfaces.Count == deep)
            {
                return;
            }

            if(_surfaces.Count > deep)
            {
                foreach(var surface in _surfaces.Skip((int)deep).Where(x => x != null))
                {
                    surface?.Flush();
                    surface?.Dispose();
                }
                _surfaces.RemoveRange((int)deep, _surfaces.Count - (int)deep);
            }
            else
            {
                var toAddCount = deep - _surfaces.Count;
                for(int i = 0; i < toAddCount; i++)
                {
                    _surfaces.Add(SurfaceFactory());
                }
            }
        }

        public SKCanvas GetCanvas(int level)
        {
            if(level == _surfaces.Count)
            {
                var surface = SurfaceFactory();
                _surfaces.Add(surface);
                return surface.Canvas;
            }
            else if(level < _surfaces.Count)
            {
                var surf = _surfaces[level];
                if(surf == null)
                {
                    surf = SurfaceFactory();
                    _surfaces[level] = surf;
                }
                return surf.Canvas;
            }
            else
            {
                for(int i = _surfaces.Count; i < level - 1; i++)
                {
                    _surfaces.Add(null);
                }
                var surf = SurfaceFactory();
                _surfaces.Add(surf);
                return surf.Canvas;
            }
        }

        public SKCanvas GetScreenDrawCanvas()
        {
            if(ScreenDrawsSurface == null)
            {                
                ScreenDrawsSurface = SurfaceFactory();                
            }
            return ScreenDrawsSurface.Canvas;
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
            foreach(var surface in _surfaces.Where(x => x != null))
            {
                surface.Flush();
                surface.Dispose();
            }
            ScreenDrawsSurface?.Dispose();
        }

        public float RelativeToX = 0f;
        public float RelativeToY = 0f;
    }
}
