using CSX.Events;

namespace CSX.NativeComponents
{
    public record SkiaCanvasProps : ViewProps
    {
        public Action<SKPaintSurfaceEventArgs>? OnPaintSurface { get; init; }
    }
    public class SkiaCanvas
    {
        
    }
}
