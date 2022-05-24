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
    public class Text : View
    {
        public Text(ulong id) : base(id)
        {
        }

        float GetTextSize()
        {
            if (Attributes.TryGetValue(NativeAttribute.FontSize, out var value))
            {
                return (float)value;
            }

            return 17;
        }

        SKColor GetTextColor()
        {
            if (Attributes.TryGetValue(NativeAttribute.Color, out var value))
            {
                var color = (System.Drawing.Color)value;
                return new SKColor(color.R, color.G, color.B, color.A);
            }

            return new SKColor(0,0,0, 255);
        }

        SKFontStyleWeight GetFontWeight()
        {
            if(Attributes.TryGetValue(NativeAttribute.FontWeight, out var value))
            {
                var fontWeight = (FontWeight)value;
                return fontWeight switch
                {
                    FontWeight.Normal => SKFontStyleWeight.Normal,
                    FontWeight.Bold => SKFontStyleWeight.Bold,
                    FontWeight.V100 => SKFontStyleWeight.Thin,
                    FontWeight.V200 => SKFontStyleWeight.ExtraLight,
                    FontWeight.V300 => SKFontStyleWeight.Light,
                    FontWeight.V400 => SKFontStyleWeight.Normal,
                    FontWeight.V500 => SKFontStyleWeight.Medium,
                    FontWeight.V600 => SKFontStyleWeight.SemiBold,
                    FontWeight.V700 => SKFontStyleWeight.Bold,
                    FontWeight.V800 => SKFontStyleWeight.ExtraBold,
                    FontWeight.V900 => SKFontStyleWeight.Black,
                    _ => throw new NotImplementedException()
                };
            }

            return SKFontStyleWeight.Normal;
        }

        SKFontStyle GetFontStyle()
        {
            if (Attributes.TryGetValue(NativeAttribute.FontStyle, out var value))
            {
                var style = (FontStyle)value;
                return style switch
                {
                    FontStyle.Normal => new SKFontStyle(GetFontWeight(), SKFontStyleWidth.Normal, SKFontStyleSlant.Upright),
                    FontStyle.Italic => new SKFontStyle(GetFontWeight(), SKFontStyleWidth.Normal, SKFontStyleSlant.Italic),
                    _ => throw new NotImplementedException()
                };
            }

            return SKFontStyle.Normal;
        }

        SKTypeface GetTypeface()
        {
            SKTypeface result = SKTypeface.Default;
            if (Attributes.TryGetValue(NativeAttribute.FontFamily, out var value))
            {
                SKTypeface.FromFamilyName((string)value, GetFontStyle());
            }

            return result;
        }

        public string TextContent { get; set; } = "";
        string lastDrawText = "";

        public override bool Draw(SKCanvas canvas, bool forceDraw, int level, SKRect? clipRect, float translateY, DrawContext context)
        {
            var x = YogaNode.LayoutX + context.RelativeToX;
            var y = YogaNode.LayoutY + context.RelativeToY;

            if(!forceDraw && !IsDirty && !YogaNode.HasNewLayout && lastDrawText == TextContent)
            {
                return false;
            }

            base.Draw(canvas, true, level, clipRect, translateY, context);

            using (var paint = GetPaint())
            {
                var borderLeftWidth = float.IsNaN(YogaNode.BorderLeftWidth) ? 0 : YogaNode.BorderLeftWidth;
                var borderToptWidth = float.IsNaN(YogaNode.BorderTopWidth) ? 0 : YogaNode.BorderTopWidth;
                canvas.DrawText(TextContent, x + YogaNode.LayoutPaddingLeft + borderLeftWidth, y + YogaNode.LayoutPaddingTop + borderToptWidth + YogaNode.LayoutHeight, paint);
            }

            lastDrawText = TextContent;

            return true;
        }

        public override bool NeedsToReDraw()
        {
            return base.NeedsToReDraw() || lastDrawText != TextContent;
        }

        public override void Mesure()
        {
            using (var paint = GetPaint())
            {
                SKRect textBounds = new SKRect();
                _ = paint.MeasureText(TextContent, ref textBounds);

                if(float.IsNaN(YogaNode.Width.Value))
                {
                    YogaNode.Width = textBounds.Width;
                }

                if (float.IsNaN(YogaNode.Height.Value))
                {
                    YogaNode.Height = textBounds.Height;
                }                
            }                

        }

        SKPaint GetPaint()
        {
            var paint = new SKPaint();
            var size = GetTextSize();

            paint.TextSize = size;
            paint.IsAntialias = true;
            paint.Color = GetTextColor();
            paint.IsStroke = false;
            paint.Typeface = GetTypeface();
            

            return paint;
        }

    }
}
