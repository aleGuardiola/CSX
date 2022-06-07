using CSX.Skia.Rendering.DOM;
using Facebook.Yoga;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Skia.Rendering.Render;

public static class RenderTree
{
    public static RenderNode Create(Element element)
    {
        var result = new RenderNode(new RenderElement[0], null);

        if (element.YogaNode.Display == YogaDisplay.None)
        {
            result.Children = new RenderNode[0];
            return result;
        }

        result.Children = new RenderNode[] { Create(element, result) };
        

        return result;
    }

    static RenderNode Create(Element element, RenderNode? parent)
    {
        var context = new RenderContext(0f, 0f);

        element.TryRender(context);
        var renderElements = context.Elements.ToArray();

        var result = new RenderNode(renderElements, parent)
        {
            IsAbsolute = element.YogaNode.PositionType == YogaPositionType.Absolute,
            IsScrolling = element.YogaNode.Overflow == YogaOverflow.Scroll,
            Opacity = element.Opacity,
            Transform = element.Transform,
            Rect = SKRect.Create(element.YogaNode.LayoutX + parent?.Rect.Left ?? 0, element.YogaNode.LayoutY + parent?.Rect.Top ?? 0, element.YogaNode.LayoutWidth, element.YogaNode.LayoutHeight)
        };

        if (element.Count == 0)
        {
            result.Children = new RenderNode[0];
            return result;
        }

        var children = new RenderNode[element.Count];
        var finalLenght = 0;

        for (int i = 0; i < element.Count; i++)
        {
            var child = element[i];

            // dont add to the render tree children with display none
            if (child.YogaNode.Display == YogaDisplay.None)
            {
                continue;
            }

            children[finalLenght] = Create(child, result);

            finalLenght++;
        }

        var finalChildren = new RenderNode[finalLenght];
        Array.Copy(children, finalChildren, finalLenght);

        result.Children = finalChildren;

        return result;
    }
}

