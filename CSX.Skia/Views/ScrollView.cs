﻿using CSX.NativeComponents;
using CSX.Rendering;
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

            var x = YogaNode.LayoutX + width - ScrollBarWidth;
            var y = YogaNode.LayoutY;

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
            DrawUpButton(buttonX, y + 3f, buttonH, scrollBarPostion == 0f ? ScrollBarButtonDisabledColor : ScrollBarButtonColor, canvas);
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
