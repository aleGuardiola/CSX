using CSX.NativeComponents;
using CSX.Rendering;
using CSX.Skia.Events;
using CSX.Skia.Input;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Skia.Views
{
    public class ScrollView : View
    {
        public float ScrollBarWidth = 17f;
        public float ScrollBarMinimumHeight = 20f;
        public Color ScrollBarBackgroundColor = ColorTranslator.FromHtml("#424242");
        public Color ScrollBarColor = ColorTranslator.FromHtml("#686868");
        public Color ScrollBarButtonColor = Color.White;
        public Color ScrollBarButtonDisabledColor = ColorTranslator.FromHtml("#808080");

        public ScrollView(ulong id) : base(id)
        {
            SetAttribute(NativeAttribute.Overflow, Overflow.Scroll);
        }

        public override bool Draw(SKCanvas canvas, bool forceDraw, int level, SKRect? clipRect, float translateY, DrawContext context)
        {
            if (base.Draw(canvas, forceDraw, level, clipRect, translateY, context))
            {
                RenderScrollBar(canvas);
                return true;
            }            

            return false;
        }

        public float GetMaxScroll()
        {
            var totalContentLenght = GetContentHeight();
            return totalContentLenght - (YogaNode.LayoutHeight - YogaNode.LayoutPaddingTop - YogaNode.LayoutPaddingBottom - GetBorderTopWidth() - GetBorderBottomWidth());
        }

        SKRect UpRect = SKRect.Empty;
        SKRect DownRect = SKRect.Empty;

        protected override void OnMouseWheel(float offsetX, float offsetY)
        {
            var currentScrollPosition = GetScrollPosition();
            var newScroll = currentScrollPosition - (offsetY * 100);
            var maxScroll = GetMaxScroll();

            SetAttribute(NativeAttribute.ScrollPosition, Math.Max(0f, Math.Min(maxScroll, newScroll)));
            base.OnMouseWheel(offsetX, offsetY);
        }

        protected override void OnLeftClick(SKPoint down, SKPoint up)
        {
            if(UpRect.Contains(up) && UpRect.Contains(down))
            {
                // scroll up pressed
                MoveScrollBarPosition(100f);
            }
            else if(DownRect.Contains(up) && DownRect.Contains(down))
            {
                // scroll down pressed
                MoveScrollBarPosition(-100f);
            }
            base.OnLeftClick(down, up);
        }

        bool _isCursorScrolling;
        SKPoint _cursorScrollingZero = SKPoint.Empty;
        SKPoint _mousePosition = SKPoint.Empty;

        protected override void OnFrameDraw(double time)
        {
            if(_isCursorScrolling)
            {
                var difference = _cursorScrollingZero - _mousePosition;
                if(difference.Y == 0)
                {
                    DrawContext.SetCursor(CSXSkiaCursor.Move);
                }
                else
                {
                    if (difference.Y < 0)
                    {
                        DrawContext.SetCursor(CSXSkiaCursor.MoveDown);
                    }
                    else
                    {
                        DrawContext.SetCursor(CSXSkiaCursor.MoveUp);
                    }

                    MoveScrollBarPosition((float)(difference.Y * 50 * time));
                }
            }

            base.OnFrameDraw(time);
        }

        protected override void OnMouseMove(SKPoint position)
        {
            _mousePosition = position;            
            base.OnMouseMove(position);
        }

        protected override void OnMouseButtonUp(CSXSkiaMouseButton button, SKPoint position)
        {
            if (button == CSXSkiaMouseButton.Middle)
            {
                if (_isCursorScrolling)
                {
                    DrawContext.SetCursor(CSXSkiaCursor.Default);
                }
                else
                {
                    DrawContext.SetCursor(CSXSkiaCursor.Move);
                    _cursorScrollingZero = _mousePosition;
                }
                _isCursorScrolling = !_isCursorScrolling;
            }
            base.OnMouseButtonUp(button, position);
        }

        void MoveScrollBarPosition(float offset)
        {
            var currentScrollPosition = GetScrollPosition();
            var newScroll = currentScrollPosition - offset;
            var maxScroll = GetMaxScroll();

            SetAttribute(NativeAttribute.ScrollPosition, Math.Max(0f, Math.Min(maxScroll, newScroll)));
        }



        public void RenderScrollBar(SKCanvas canvas)
        {
            var totalContentLenght = GetContentHeight();
            var maxScroll = totalContentLenght - (YogaNode.LayoutHeight - YogaNode.LayoutPaddingTop - YogaNode.LayoutPaddingBottom - GetBorderTopWidth() - GetBorderBottomWidth());
            
            // Dont render the scroll bar if it is not need it
            if(maxScroll < 0)
            {
                return;
            }

            var scrollBarPostion = GetScrollPosition() / maxScroll;

            var height = YogaNode.LayoutHeight;
            var width = YogaNode.LayoutWidth;

            var x = AbsoulteX + width - ScrollBarWidth;
            var y = AbsoluteY;

            // render background
            using (var paint = new SKPaint())
            {
                paint.Style = SKPaintStyle.Fill;
                paint.Color = new SKColor(ScrollBarBackgroundColor.R, ScrollBarBackgroundColor.G, ScrollBarBackgroundColor.B, ScrollBarBackgroundColor.A);
                canvas.DrawRect(x, y, ScrollBarWidth, height, paint);
            }

            var buttonH = ScrollBarWidth * .4f;
            var buttonX = (x + (ScrollBarWidth / 2f)) - (buttonH / 2);
            
            // render buttons
            UpRect = SKRect.Create(buttonX + TranslatedX, y + 3f + TranslatedY, buttonH, buttonH);
            DrawUpButton(buttonX, y + 3f, buttonH, scrollBarPostion == 0f ? ScrollBarButtonDisabledColor : ScrollBarButtonColor, canvas);
            DownRect = SKRect.Create(buttonX + TranslatedX, (y + height - buttonH - 3f) + TranslatedY, buttonH, buttonH);
            DrawDownButton(buttonX, y + height - buttonH - 3f, buttonH, scrollBarPostion == 1f ? ScrollBarButtonDisabledColor : ScrollBarButtonColor, canvas);

            // render scroll bar           
            var sY = y + buttonH + 6f;
            var stY = y + height - buttonH - 6f;

            var scrollBarHeight = Math.Max((YogaNode.LayoutHeight / totalContentLenght) * (stY - sY), ScrollBarMinimumHeight);

            var scrollBarWidth = ScrollBarWidth - 2f;
                        
            var scrollBarStart = sY + (scrollBarHeight / 2f);
            var scrollBarStop = stY - (scrollBarHeight / 2f);

            var scrollBarTotal = scrollBarStop - scrollBarStart;

            var scrollBarY = sY + scrollBarPostion * scrollBarTotal;

            using (var paint = new SKPaint())
            {
                paint.Style = SKPaintStyle.Fill;
                paint.Color = new SKColor(ScrollBarColor.R, ScrollBarColor.G, ScrollBarColor.B, ScrollBarColor.A);
                canvas.DrawRect(x + 1f, scrollBarY, scrollBarWidth, scrollBarHeight, paint);
            }

        }

        public void DrawUpButton(float x, float y, float h, Color color, SKCanvas canvas)
        {            
            SKPath path = new SKPath();
            path.MoveTo((h / 2) + x, y);

            path.LineTo(x, h * .7f + y);
            path.LineTo(h + x, h * .7f + y);
            path.LineTo((h / 2) + x, y);

            SKPaint paint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = new SKColor(color.R, color.G, color.B, color.A)
            };

            canvas.DrawPath(path, paint);
            path.Dispose();
            paint.Dispose();
        }

        public void DrawDownButton(float x, float y, float h, Color color, SKCanvas canvas)
        {
            SKPath path = new SKPath();
            path.MoveTo(x, y);

            path.LineTo((h / 2) + x, y + h * .7f);
            path.LineTo(h + x, y);
            path.LineTo(x, y);

            SKPaint paint = new SKPaint
            {
                Style = SKPaintStyle.Fill,
                Color = new SKColor(color.R, color.G, color.B, color.A)
            };

            canvas.DrawPath(path, paint);
            path.Dispose();
            paint.Dispose();
        }

    }
}
