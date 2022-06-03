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
        protected List<BaseView> _children = new List<BaseView>();

        public View(ulong id) : base(id)
        {
            
        }
                
        public IEnumerable<BaseView> Children => _children;

        public virtual void RemoveWithId(ulong id)
        {
            RemoveAt(_children.IndexOf(_children.First(x => x.Id == id)));
        }

        public virtual void SetChildren(BaseView[] children)
        {
            Clear();
            foreach(var child in children)
            {
                AppendView(child);
            }
        }

        public virtual void AppendView(BaseView view)
        {
            if(view.Parent != null)
            {
                throw new InvalidOperationException("View to append already have a parent");
            }

            view.Parent = this;
            _children.Add(view);
            YogaNode.Insert(YogaNode.Count, view.YogaNode);
            SetDeep(view.Deep);
            IsDirty = true;
        }

        public virtual void RemoveAt(int index)
        {
            var child = _children[index];
            child.Parent = null;
            YogaNode.RemoveAt(index);
            _children.RemoveAt(index);
            IsDirty = true;
            ComputeDeep();
        }

        public virtual void Clear()
        {
            foreach(var child in _children)
            {
                child.Parent = null;
            }
            YogaNode.Clear();
            _children.Clear();
            IsDirty = true;
            Deep = 1;
        }

        void ComputeDeep()
        {
            uint result = 1;
            foreach(var child in Children)
            {
                if(child.Deep > result)
                {
                    result = child.Deep;
                }
            }
            Deep = result;
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
                var totalContentLenght = GetContentHeight();
                var maxScroll = totalContentLenght - (YogaNode.LayoutHeight - YogaNode.LayoutPaddingTop - YogaNode.LayoutPaddingBottom - GetBorderTopWidth() - GetBorderBottomWidth());

                return Math.Min((float)value, maxScroll);
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

        public int? GetZIndex()
        {
            if (Attributes.TryGetValue(NativeAttribute.ZIndex, out var value))
            {
                if(value == null)
                {
                    return null;
                }
                return (int)value;
            }

            return null;
        }

        SKPaint _backgroundPaint = new SKPaint()
        {
            Style = SKPaintStyle.Fill,            
           // BlendMode = SKBlendMode.Src,            
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

            LastDrawedWidth = totalWidth;
            LastDrawedHeight = totalHeight;

            RelativeX = YogaNode.LayoutX;
            RelativeY = YogaNode.LayoutY;

            var x = AbsoulteX = RelativeX + context.RelativeToX;
            var y = AbsoluteY = RelativeY + context.RelativeToY;

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

            bool childrenChangedRect = false;
            if(YogaNode.HasNewLayout)
            {
                childrenChangedRect = Children.Any(x => x.RectChanged());
            }

            // only draw background when the layout changed
            if (forceDraw || IsDirty || (YogaNode.HasNewLayout && childrenChangedRect))
            {
                result = true;
                _backgroundPaint.Color = GetBackgroundColor();
                canvas.DrawRect(contentRect, _backgroundPaint);

                if (context.ShowScreenDraws)
                {
                    var screnDrawCanvas = context.GetScreenDrawCanvas();
                    screnDrawCanvas.DrawRect(SKRect.Create(X, Y, YogaNode.LayoutWidth, YogaNode.LayoutHeight), new SKPaint()
                    {
                        Color = SKColors.Purple.WithAlpha(125),
                        Style = SKPaintStyle.Fill
                    });
                }

            }

            var overflow = GetOverFlow();
            if (overflow == Overflow.Hidden || overflow == Overflow.Scroll)
            {
                
                bool scrollPositionChanged = false;
                if (lastScrollPosition != scrollPosition)
                {
                    scrollPositionChanged = true;
                    lastScrollPosition = scrollPosition;
                }


                // offset the content canvas by what this view is currently translated           
                contentRect.Offset(canvas.TotalMatrix.TransX, canvas.TotalMatrix.TransY);                

                // if scroll position changed or the layout changed always draw children
                if (scrollPositionChanged || forceDraw || (YogaNode.HasNewLayout && childrenChangedRect))
                {
                    result = true;
                    if(childrenCanvas != null)
                    {
                        // clear all layers on top
                        foreach (var surface in context.Surfaces.Skip(childrenLevel).Where(x => x != null))
                        {
                            surface.Canvas.Save();
                            surface.Canvas.ClipRect(contentRect);
                            surface.Canvas.DrawRect(contentRect, new SKPaint() { BlendMode = SKBlendMode.Clear });
                            // surface.Canvas.DrawRect(contentRect, new SKPaint() { Color = SKColors.Yellow, StrokeWidth = 2f, Style = SKPaintStyle.Stroke });
                        }

                        //childrenCanvas.Save();
                        //childrenCanvas.ClipRect(contentRect);
                        // childrenCanvas.DrawRect(contentRect, new SKPaint() { Color = SKColors.Black, Style = SKPaintStyle.Stroke, StrokeWidth = 5f });
                        childrenCanvas.Translate(canvas.TotalMatrix.TransX, (scrollPosition * -1) + canvas.TotalMatrix.TransY);
                        
                        result = true;
                        foreach (var child in Children)
                        {
                            if(child is View view && view.GetZIndex() != null)
                            {
                                var zindex = view.GetZIndex();
#pragma warning disable CS8629 // Nullable value type may be null.
                                var childCanvas = context.GetCanvas(zindex.Value);
#pragma warning restore CS8629 // Nullable value type may be null.
                                childCanvas.Translate(canvas.TotalMatrix.TransX, (scrollPosition * -1) + canvas.TotalMatrix.TransY);
                                child.Draw(childCanvas, true, zindex.Value, contentRect, scrollPosition, newContext);
                                child.MarkAsSeen();
                            }
                            else
                            {
                                child.Draw(childrenCanvas, true, childrenLevel, contentRect, scrollPosition, newContext);
                                child.MarkAsSeen();
                            }                            
                        }

                        foreach (var surface in context.Surfaces.Skip(childrenLevel).Where(x => x != null))
                        {
                            surface.Canvas.Restore();
                        }
                            
                    }
                }
                else
                {
                    if(childrenCanvas != null)
                    {
                        var childrenToDraw = Children.Where(x => x.NeedsToReDraw()).ToArray();

                        childrenCanvas.Save();
                        childrenCanvas.ClipRect(contentRect);
                        // childrenCanvas.DrawRect(contentRect, new SKPaint() { Color = SKColors.Black, Style = SKPaintStyle.Stroke, StrokeWidth = 5f });
                        childrenCanvas.Translate(canvas.TotalMatrix.TransX, (scrollPosition * -1) + canvas.TotalMatrix.TransY);

                        // draw children
                        foreach (var child in childrenToDraw)
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

        public override void Mesure()
        {
            foreach(var child in Children)
            {
                child.Mesure();
            }
            base.Mesure();
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
