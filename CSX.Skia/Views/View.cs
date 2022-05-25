using CSX.NativeComponents;
using CSX.Rendering;
using CSX.Skia.Events;
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
            if(view.Parent != null)
            {
                throw new InvalidOperationException("View to append already have a parent");
            }

            view.Parent = this;
            _children.Add(view);
            YogaNode.Insert(YogaNode.Count, view.YogaNode);
            IsDirty = true;
        }

        public void RemoveAt(int index)
        {
            var child = _children[index];
            child.Parent = null;
            YogaNode.RemoveAt(index);
            _children.RemoveAt(index);
            IsDirty = true;
        }

        public void Clear()
        {
            foreach(var child in _children)
            {
                child.Parent = null;
            }
            YogaNode.Clear();
            _children.Clear();
            IsDirty = true;
        }

        public SKColor GetBackgroundColor()
        {
            if (Attributes.TryGetValue(NativeAttribute.BackgroundColor, out object? value))
            {
                var color = (System.Drawing.Color)value;
                return new SKColor(color.R, color.G, color.B, color.A);
            }

            return SKColors.Transparent;
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
            if (!float.IsNaN(YogaNode.BorderTopWidth))
            {
                return YogaNode.BorderTopWidth;
            }

            if (!float.IsNaN(YogaNode.BorderWidth))
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
            if (Attributes.TryGetValue(NativeAttribute.Overflow, out var value))
            {
                return (Overflow)value;
            }

            return Overflow.Hidden;
        }
        public float GetScrollPosition()
        {
            if(GetOverFlow() != Overflow.Scroll)
            {
                return 0f;
            }

            if (Attributes.TryGetValue(NativeAttribute.ScrollPosition, out var value))
            {
                if(value == null)
                {
                    return 0f;
                }
                return (float)value;
            }

            return 0f;
        }

        public float GetContentHeight()
        {
            if(_children.Count == 0)
            {
                return 0f;
            }

            var firstChild = YogaNode.First();
            var lastChild = YogaNode.Last();
            return (lastChild.LayoutY - firstChild.LayoutY) + lastChild.LayoutHeight + firstChild.LayoutMarginTop + lastChild.LayoutMarginBottom;
        }

        SKPaint _backgroundPaint = new SKPaint()
        {
            Style = SKPaintStyle.Fill,
            BlendMode = SKBlendMode.Src
        };

        SKPaint _borderTopPaint = new SKPaint()
        {
            Style = SKPaintStyle.Fill
        };

        SKPaint _borderBottomPaint = new SKPaint()
        {
            Style = SKPaintStyle.Fill
        };

        SKPaint _borderRightPaint = new SKPaint()
        {
            Style = SKPaintStyle.Fill
        };

        SKPaint _borderLeftPaint = new SKPaint()
        {
            Style = SKPaintStyle.Fill
        };

        float lastScrollPosition = 0f;
        float lastBorderTop = 0f;
        float lastBorderBottom = 0f;
        float lastBorderLeft = 0f;
        float lastBorderRight = 0f;

        public override void OnEvent(WindowEvent ev)
        {
            if (_children.Count == 0)
            {
                base.OnEvent(ev);
                return;
            }
                        
            foreach(var child in _children)
            {
                child.OnEvent(ev);
            }

            if(ev.Propagate)
            {
                base.OnEvent(ev);
            }
        }

        public override bool Draw(SKCanvas canvas, bool forceDraw, int level, SKRect? clipRect, float translateY, DrawContext context)
        {
            TranslatedX = canvas.TotalMatrix.TransX;
            TranslatedY = canvas.TotalMatrix.TransY;
            DrawContext = context;

            var childrenLevel = level + 1;

            SKCanvas? childrenCanvas = null;
            if (_children.Count > 0)
            {
                childrenCanvas = context.GetCanvas(childrenLevel);
            }            

            bool result = false;

            var totalWidth = YogaNode.LayoutWidth;
            var totalHeight = YogaNode.LayoutHeight;                       

            var x = AbsoulteX = YogaNode.LayoutX + context.RelativeToX;
            var y = AbsoluteY = YogaNode.LayoutY + context.RelativeToY;

            var newContext = context with { RelativeToX = x, RelativeToY = y };

            var borderTop = GetBorderTopWidth();
            var borderBottom = GetBorderBottomWidth();
            var borderLeft = GetBorderLeftWidth();
            var borderRight = GetBorderRightWidth();

            var contentWidth = totalWidth - (YogaNode.LayoutPaddingRight + borderRight);
            var contentHeight = totalHeight - (YogaNode.LayoutPaddingBottom + borderBottom);

            var scrollPosition = GetScrollPosition();

            var contentRect = new SKRect(
                x + borderLeft,
                y + borderTop,
                x + (totalWidth - borderRight),
                y + (totalHeight - borderBottom));

            // only draw background when the layout changed
            if(forceDraw || IsDirty || YogaNode.HasNewLayout)
            {
                result = true;
                _backgroundPaint.Color = GetBackgroundColor();
                canvas.DrawRect(contentRect, _backgroundPaint);
            }            

            var overflow = GetOverFlow();
            if (overflow == Overflow.Hidden || overflow == Overflow.Scroll)
            {
                //if (_contentSurface == null)
                //{
                //    _contentInfo = new SKImageInfo((int)contentWidth, (int)contentHeight);
                //    _contentSurface = SKSurface.Create(_contentInfo);
                //}
                //else
                //{
                //    if (_contentInfo.Width != (int)contentWidth || _contentInfo.Height != (int)contentHeight)
                //    {
                //        _contentInfo = new SKImageInfo((int)contentWidth, (int)contentHeight);
                //        _contentSurface = SKSurface.Create(_contentInfo);
                //    }
                //}                

                bool scrollPositionChanged = false;
                if (lastScrollPosition != scrollPosition)
                {
                    scrollPositionChanged = true;
                    lastScrollPosition = scrollPosition;
                }

                //if (overflow == Overflow.Scroll)
                //{                    


                //    //if (scrollPosition * -1 != _contentSurface.Canvas.TotalMatrix.TransY)
                //    //{
                //    //    var translation = GetScrollPosition() + _contentSurface.Canvas.TotalMatrix.TransY;
                //    //    _contentSurface.Canvas.Translate(0f, translation * -1);
                //    //    scrollPositionChanged = true;
                //    //}
                //}

                // offset the content canvas by what this view is currently translated           
                contentRect.Offset(canvas.TotalMatrix.TransX, canvas.TotalMatrix.TransY);

                // if scroll position changed or the layout changed always draw children
                if (scrollPositionChanged || YogaNode.HasNewLayout || forceDraw)
                {
                    result = true;
                    if(childrenCanvas != null)
                    {                                     
                        // clear all layers on top
                        foreach(var surface in context.Surfaces.Skip(childrenLevel))
                        {
                            surface.Canvas.DrawRect(contentRect, new SKPaint() { BlendMode = SKBlendMode.Clear });
                        }

                        childrenCanvas.Save();
                        childrenCanvas.ClipRect(contentRect);
                        childrenCanvas.Translate(0f, (scrollPosition * -1) + canvas.TotalMatrix.TransY);
                        
                        result = true;
                        foreach (var child in Children)
                        {
                            child.Draw(childrenCanvas, true, childrenLevel, contentRect, scrollPosition, newContext);
                            child.MarkAsSeen();
                        }

                        childrenCanvas.Restore();
                    }
                }
                else
                {
                    if(childrenCanvas != null)
                    {
                        var childrenToDraw = Children.Where(x => x.NeedsToReDraw()).ToArray();

                        childrenCanvas.Save();
                        childrenCanvas.ClipRect(contentRect);
                        childrenCanvas.Translate(0f, scrollPosition * -1);

                        // draw children
                        foreach (var child in Children)
                        {
                            if (child.Draw(childrenCanvas, false, childrenLevel, contentRect, scrollPosition, newContext))
                            {
                                result = true;
                            }
                            child.MarkAsSeen();
                        }

                        childrenCanvas.Restore();
                    }                    
                }

                

                //if (scrollPositionChanged || YogaNode.HasNewLayout)
                //{
                //    _backgroundPaint.Color = GetBackgroundColor();

                //    canvas.DrawRect(new SKRect(
                //        x + borderLeft,
                //        y + borderTop,
                //        x + (totalWidth - borderRight),
                //        y + (totalHeight - borderBottom)
                //        ), _backgroundPaint);

                //    result = true;

                //    // canvas.Clear(GetBackgroundColor());

                //    // draw children
                //    foreach (var child in Children)
                //    {
                //        child.Draw(childrenCanvas, true, childrenLevel, newContext);
                //        child.MarkAsSeen();
                //    }
                //}
                //else
                //{
                //    if (childrenToDraw.Length > 0)
                //    {
                //        _backgroundPaint.Color = GetBackgroundColor();

                //        canvas.DrawRect(new SKRect(
                //            x + borderLeft,
                //            y + borderTop,
                //            x + (totalWidth - borderRight),
                //            y + (totalHeight - borderBottom)
                //            ), _backgroundPaint);                        
                //    }

                //    // If the layout has not changed and the scroll position is the same lets try and reduce the number of draws,
                //    // also clearing the content is not necessary because the children are in the same position
                //    foreach (var child in childrenToDraw)
                //    {                                                
                //        if(child.Draw(childrenCanvas, false, childrenLevel, newContext))
                //        {
                //            result = true;
                //        }
                //        child.MarkAsSeen();                        
                //    }
                //}
            }
            else
            {
                result = true;
                foreach (var child in Children)
                {
                    child.Draw(childrenCanvas, true, childrenLevel, null, 0f, newContext);
                }
            }

            // draw border
            if (borderTop > 0 && borderTop != lastBorderTop || forceDraw)
            {
                _borderTopPaint.Color = GetBorderTopColor();

                canvas.DrawRect(SKRect.Create(
                    x,
                    y,
                    totalWidth,
                    borderTop
                    ), _borderBottomPaint);

            }

            if (borderBottom > 0 && borderBottom != lastBorderBottom || forceDraw)
            {
                _borderBottomPaint.Color = GetBorderTopColor();

                canvas.DrawRect(SKRect.Create(
                    x,
                    y + totalHeight - borderBottom,
                    totalWidth,
                    borderBottom
                    ), _borderBottomPaint);
            }

            if (borderLeft > 0 && borderLeft != lastBorderLeft || forceDraw)
            {
                _borderLeftPaint.Color = GetBorderTopColor();

                canvas.DrawRect(SKRect.Create(
                    x,
                    y,
                    borderLeft,
                    totalHeight
                    ), _borderLeftPaint);
            }

            if (borderRight > 0 && borderRight != lastBorderRight || forceDraw)
            {
                _borderRightPaint.Color = GetBorderTopColor();

                canvas.DrawRect(SKRect.Create(
                    x + totalWidth - borderRight,
                    y,
                    borderRight,
                    totalHeight
                    ), _borderRightPaint);
            }

            lastBorderTop = borderTop;
            lastBorderLeft = borderLeft;
            lastBorderRight = borderRight;
            lastBorderBottom = borderBottom;

            YogaNode.MarkLayoutSeen();
            //canvas.Translate(0f, translateY);
            
            //if(clipRect != null)
            //{
            //    canvas.Restore();
            //}           

            return result;
        }

        public override bool NeedsToReDraw()
        {
            if (base.NeedsToReDraw())
            {
                return true;
            }

            return Children.Any(x => x.NeedsToReDraw());            
        }

        public override bool IsLayoutDirty()
        {
            if(base.IsLayoutDirty())
            {
                return true;
            }

            return Children.Any(x => x.IsLayoutDirty());
        }

        public override void CalculateLayout()
        {
            foreach (var child in Children)
            {
                child.Mesure();
            }

            YogaNode.CalculateLayout();
        }

    }
}
