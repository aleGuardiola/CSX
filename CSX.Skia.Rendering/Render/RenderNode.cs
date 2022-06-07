using CSX.Skia.Rendering.Graphic;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Skia.Rendering.Render;

public class RenderNode
{
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public RenderNode(RenderElement[] renderElements, RenderNode? parent)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        RenderElements = renderElements;
        Parent = parent;
    }

    public SKRect Rect { get; init; }
    public int ZIndex { get; init; } = 0;
    public Transform? Transform { get; init; }
    public float Opacity { get; init; } = 1f;
    public bool IsAbsolute { get; init; } = false;
    public bool IsScrolling { get; init; } = false;


    public bool IsRoot => Parent?.Parent == null;

    public RenderElement[] RenderElements { get; }

    public RenderNode? Parent { get; }

    public RenderNode[] Children { get; set; }

    /// <summary>
    /// Paint Node
    /// </summary>
    /// <param name="drawingContext"></param>
    public void Paint(GraphicContext drawingContext)
    {
        for(var i = 0; i < RenderElements.Length; i++)
        {
            var renderElement = RenderElements[i];
            renderElement.Paint(drawingContext);
        }
    }

}

