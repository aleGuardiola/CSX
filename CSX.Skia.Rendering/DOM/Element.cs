using CSX.Skia.Rendering.Render;
using Facebook.Yoga;
using SkiaSharp;
using System.Collections;

namespace CSX.Skia.Rendering.DOM;

public class Element : IList<Element>
{
    List<Element> _children = new List<Element>();

    public YogaNode YogaNode { get; init; }

    public Transform Transform { get; private set; }

    public int ZIndex { get; private set; } = 0;

    public SKColor BackgroundColor { get; init; }
    public SKColor BorderRightColor { get; init; }
    public SKColor BorderLeftColor { get; init; }
    public SKColor BorderTopColor { get; init; }
    public SKColor BorderBottomColor { get; init; }
    public float BorderTopWidth { get; init; }
    public float BorderRightWidth { get; init; }
    public float BorderLeftWidth { get; init; }
    public float BorderBottomWidth { get; init; }

    public float Opacity { get; init; } = 1f;

    public int Count => _children.Count;

    public bool IsReadOnly => false;

    public Element this[int index] { get => _children[index]; set => _children[index] = value; }

    public Element()
    {
        YogaNode = new YogaNode();
        Transform = new Transform();        
    }   

    public void TryRender(RenderContext renderContext)
    {
        // ignore elements with display none
        if (YogaNode.Display == YogaDisplay.None)
        {
            return;
        }
        Render(renderContext);        
    }

    protected virtual void Render(RenderContext renderContext)
    {
        // add the background of the element and the borders by default
        var x = renderContext.ParentX + YogaNode.LayoutX;
        var y = renderContext.ParentX + YogaNode.LayoutY;
        var rect = SKRect.Create(x, y, YogaNode.LayoutWidth, YogaNode.LayoutHeight);

        var background = new BackgroundBorderRenderElement(rect)
        {
            BackgroundColor = BackgroundColor,
            BorderBottomColor = BorderBottomColor,
            BorderBottomWidth = BorderLeftWidth,
            BorderLeftColor = BorderLeftColor,
            BorderLeftWidth = BorderLeftWidth,
            BorderRightColor = BorderRightColor,
            BorderRightWidth = BorderRightWidth,
            BorderTopColor = BorderTopColor,
            BorderTopWidth = BorderTopWidth           
        };

        renderContext.Add(background);
    }

    public int IndexOf(Element item)
    {
        return _children.IndexOf(item);
    }

    public void Insert(int index, Element item)
    {
        YogaNode.Insert(index, item.YogaNode);
        _children.Insert(index, item);
    }

    public void RemoveAt(int index)
    {
        YogaNode.RemoveAt(index);
        _children.RemoveAt(index);
    }

    public void Add(Element item)
    {
        YogaNode.Insert(YogaNode.Count, item.YogaNode);
        _children.Add(item);
    }

    public void Clear()
    {
        YogaNode.Clear();
        _children.Clear();
    }

    public bool Contains(Element item)
    {
        return _children.Contains(item);
    }

    public void CopyTo(Element[] array, int arrayIndex)
    {
        for(int index = arrayIndex, i = 0; i < array.Length; i++, index++ )
        {
            YogaNode.Insert(index, array[i].YogaNode);
        }
        _children.CopyTo(array, arrayIndex);
    }

    public bool Remove(Element item)
    {
        YogaNode.RemoveAt(_children.IndexOf(item));
        return _children.Remove(item);
    }

    public IEnumerator<Element> GetEnumerator()
    {
        return _children.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _children.GetEnumerator();
    }
}
