using CSX.Skia.Rendering.Graphic;
using SkiaSharp;
using System.Drawing;

namespace CSX.Skia.Rendering.Render;

public class BackgroundBorderRenderElement : RenderElement
{    

    public SKColor BackgroundColor { get; init; }
    public SKColor BorderRightColor { get; init; }
    public SKColor BorderLeftColor { get; init; }
    public SKColor BorderTopColor { get; init; }
    public SKColor BorderBottomColor { get; init; }
    public float BorderTopWidth { get; init; }
    public float BorderRightWidth { get; init; }
    public float BorderLeftWidth { get; init; }
    public float BorderBottomWidth { get; init; }
    
    public BackgroundBorderRenderElement(SKRect rect) : base(rect)
    {
    }

    public override void Paint(GraphicContext context)
    {
        throw new NotImplementedException();
    }
}

