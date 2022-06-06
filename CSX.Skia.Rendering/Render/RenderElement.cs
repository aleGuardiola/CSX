using CSX.Skia.Rendering.Drawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Skia.Rendering.Render
{
    public abstract class RenderElement
    {
        public float X { get; init; }
        public float Y { get; init; }
        public int ZIndex { get; init; }

        public Transform Transform { get; init; }

        public RenderElement? Parent { get; init; }

        public RenderElement[] Children { get; init; }

        public RenderElement(float x, float y, int zIndex, Transform transform, RenderElement[] children, RenderElement? parent)
        {
            X = x;
            Y = y;
            ZIndex = zIndex;
            Children = children;
            Transform = transform;
            Parent = parent;
        }

        public abstract void Draw(DrawingContext context);
    }
}
