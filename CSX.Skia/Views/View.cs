using CSX.NativeComponents;
using CSX.Rendering;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Skia.Views
{
    public class View : BaseView
    {
        private List<BaseView> _children = new List<BaseView>();

        public View(ulong id) : base(id)
        {
        }

        public IEnumerable<BaseView> Children => _children;

        public void AppendView(BaseView view)
        {
            _children.Add(view);
            YogaNode.Insert(YogaNode.Count, view.YogaNode);
        }

        public void RemoveAt(int index)
        {
            YogaNode.RemoveAt(index);
            _children.RemoveAt(index);
        }

        public void Clear()
        {
            YogaNode.Clear();
            _children.Clear();
        }

        public SKColor GetBackgroundColor()
        {
            if(Attributes.TryGetValue(NativeAttribute.BackgroundColor, out object? value))
            {
                var color = (System.Drawing.Color)value;
                return new SKColor(color.R, color.G, color.B, color.A);
            }

            return new SKColor(255,255,255,0);
        }

        public SKColor GetBorderColor()
        {
            if (Attributes.TryGetValue(NativeAttribute.BorderColor, out object? value))
            {
                var color = (System.Drawing.Color)value;
                return new SKColor(color.R, color.G, color.B, color.A);
            }

            return new SKColor(255, 255, 255, 255);
        }

        public SKColor GetBorderTopColor()
        {
            if (Attributes.TryGetValue(NativeAttribute.BorderTopColor, out object? value))
            {
                var color = (System.Drawing.Color)value;
                return new SKColor(color.R, color.G, color.B, color.A);
            }

            return GetBorderColor();
        }

        public SKColor GetBorderBottomColor()
        {
            if (Attributes.TryGetValue(NativeAttribute.BorderBottomColor, out object? value))
            {
                var color = (System.Drawing.Color)value;
                return new SKColor(color.R, color.G, color.B, color.A);
            }

            return GetBorderColor();
        }

        public SKColor GetBorderLeftColor()
        {
            if (Attributes.TryGetValue(NativeAttribute.BorderLeftColor, out object? value))
            {
                var color = (System.Drawing.Color)value;
                return new SKColor(color.R, color.G, color.B, color.A);
            }

            return GetBorderColor();
        }

        public SKColor GetBorderRightColor()
        {
            if (Attributes.TryGetValue(NativeAttribute.BorderRightColor, out object? value))
            {
                var color = (System.Drawing.Color)value;
                return new SKColor(color.R, color.G, color.B, color.A);
            }

            return GetBorderColor();
        }

        public float GetBorderTopWidth()
        {
            if(!float.IsNaN(YogaNode.BorderTopWidth))
            {
                return YogaNode.BorderTopWidth;
            }

            if(!float.IsNaN(YogaNode.BorderWidth))
            {
                return YogaNode.BorderWidth;
            }

            return 0f;
        }

        public float GetBorderBottomWidth()
        {
            if (!float.IsNaN(YogaNode.BorderBottomWidth))
            {
                return YogaNode.BorderTopWidth;
            }

            if (!float.IsNaN(YogaNode.BorderWidth))
            {
                return YogaNode.BorderWidth;
            }

            return 0f;
        }

        public float GetBorderLeftWidth()
        {
            if (!float.IsNaN(YogaNode.BorderLeftWidth))
            {
                return YogaNode.BorderTopWidth;
            }

            if (!float.IsNaN(YogaNode.BorderWidth))
            {
                return YogaNode.BorderWidth;
            }

            return 0f;
        }

        public float GetBorderRightWidth()
        {
            if (!float.IsNaN(YogaNode.BorderRightWidth))
            {
                return YogaNode.BorderTopWidth;
            }

            if (!float.IsNaN(YogaNode.BorderWidth))
            {
                return YogaNode.BorderWidth;
            }

            return 0f;
        }

        public Overflow GetOverFlow()
        {
            if(Attributes.TryGetValue(NativeAttribute.Overflow, out var value))
            {
                return (Overflow)value;
            }

            return Overflow.Hidden;
        }
        public float GetScrollPosition()
        {
            if (Attributes.TryGetValue(NativeAttribute.ScrollPosition, out var value))
            {
                return (float)value;
            }

            return 0f;
        }

        public float GetContentHeight()
        {
            return YogaNode.Sum(x => x.LayoutHeight);
        }

        public override void Draw(SKCanvas canvas)
        {
            var totalWidth = YogaNode.LayoutWidth;
            var totalHeight = YogaNode.LayoutHeight;

            var x = YogaNode.LayoutX;
            var y = YogaNode.LayoutY;                       

            {
                var paint = new SKPaint()
                {
                    Color = GetBackgroundColor(),
                    Style = SKPaintStyle.Fill
                };

                canvas.DrawRect(new SKRect(
                    x,
                    y,
                    x + totalWidth,
                    y + totalHeight
                    ), paint);
            }

            var borderTop = GetBorderTopWidth();
            var borderBottom = GetBorderBottomWidth();
            var borderLeft = GetBorderLeftWidth();
            var borderRight = GetBorderRightWidth();

            var overflow = GetOverFlow();
            if(overflow == Overflow.Hidden || overflow == Overflow.Scroll)
            {
                var info = new SKImageInfo((int)totalWidth, (int)(totalHeight - YogaNode.LayoutPaddingBottom - borderBottom));
                using (var surface = SKSurface.Create(info))
                {
                    if(overflow == Overflow.Scroll)
                    {                        
                        surface.Canvas.Translate(0f, GetScrollPosition() * -1);
                    }

                    // draw children
                    foreach (var child in Children)
                    {
                        child.Draw(surface.Canvas);
                    }
                    canvas.DrawSurface(surface, x, y);
                }                
            }
                        

            
            // draw border
            if (borderTop > 0)
            {
                var paint = new SKPaint()
                {
                    Color = GetBorderTopColor(),
                    Style = SKPaintStyle.Fill
                };

                canvas.DrawRect(SKRect.Create(
                    x,
                    y,
                    totalWidth,
                    borderTop
                    ), paint);

            }

            if (borderBottom > 0)
            {
                var paint = new SKPaint()
                {
                    Color = GetBorderBottomColor(),
                    Style = SKPaintStyle.Fill
                };

                canvas.DrawRect(SKRect.Create(
                    x,
                    y + totalHeight - borderBottom,
                    totalWidth,
                    borderBottom
                    ), paint);
            }

            if (borderLeft > 0)
            {
                var paint = new SKPaint()
                {
                    Color = GetBorderLeftColor(),
                    Style = SKPaintStyle.Fill
                };

                canvas.DrawRect(SKRect.Create(
                    x,
                    y,
                    borderLeft,
                    totalHeight
                    ), paint);
            }

            if (borderRight > 0)
            {
                var paint = new SKPaint()
                {
                    Color = GetBorderRightColor(),
                    Style = SKPaintStyle.Fill
                };

                canvas.DrawRect(SKRect.Create(
                    x + totalWidth - borderRight,
                    y,
                    borderRight,
                    totalHeight
                    ), paint);
            }

            if(overflow == Overflow.Visible)
            {
                foreach (var child in Children)
                {
                    child.Draw(canvas);
                }
            }
        }


        public override void CalculateLayout()
        {
            foreach(var child in Children)
            {
                child.Mesure();
            }
                        
            YogaNode.CalculateLayout();
        }

    }
}
