using CSX.Rendering;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Web
{
    public static class CSSHelper
    {

        static Func<object, string> PixelValue = (value) => $"{value}px";
        static Func<object, string> EnumValue = (value) => CammelCaseToCss(value.ToString() ?? "");
        static Func<object, string> StringValue = (value) => value.ToString() ?? "0";
        static Func<object, string> ColorValue = (value) => ConvertToHex((Color)value);

        static Dictionary<NativeAttribute, (string Name, Func<object, string> GetValue)> PropCssNameMap = new Dictionary<NativeAttribute, (string, Func<object, string>)>()
        {
            { 
                NativeAttribute.AlignContent,
                (CammelCaseToCss(NativeAttribute.AlignContent.ToString()), EnumValue)
            },
            {
                NativeAttribute.AlignItems,
                (CammelCaseToCss(NativeAttribute.AlignItems.ToString()), EnumValue)
            },
            {
                NativeAttribute.AlignSelf,
                (CammelCaseToCss(NativeAttribute.AlignSelf.ToString()), EnumValue)
            },
            {
                NativeAttribute.AspectRatio,
                (CammelCaseToCss(NativeAttribute.AspectRatio.ToString()), EnumValue)
            },
            {
                NativeAttribute.BackgroundColor,
                (CammelCaseToCss(NativeAttribute.BackgroundColor.ToString()), ColorValue)
            },
            {
                NativeAttribute.BorderBottomColor,
                (CammelCaseToCss(NativeAttribute.BorderBottomColor.ToString()), ColorValue)
            },
            {
                NativeAttribute.BorderBottomEndRadius,
                (CammelCaseToCss(NativeAttribute.BorderBottomEndRadius.ToString()), PixelValue)
            },
            {
                NativeAttribute.BorderBottomLeftRadius,
                (CammelCaseToCss(NativeAttribute.BorderBottomLeftRadius.ToString()), PixelValue)
            },
            {
                NativeAttribute.BorderBottomRightRadius,
                (CammelCaseToCss(NativeAttribute.BorderBottomRightRadius.ToString()), PixelValue)
            },
            {
                NativeAttribute.BorderBottomStartRadius,
                (CammelCaseToCss(NativeAttribute.BorderBottomStartRadius.ToString()), PixelValue)
            },
            {
                NativeAttribute.BorderBottomWidth,
                (CammelCaseToCss(NativeAttribute.BorderBottomWidth.ToString()), PixelValue)
            },
            {
                NativeAttribute.BorderColor,
                (CammelCaseToCss(NativeAttribute.BorderColor.ToString()), ColorValue)
            },
            {
                NativeAttribute.BorderEndColor,
                (CammelCaseToCss(NativeAttribute.BorderEndColor.ToString()), ColorValue)
            },
            {
                NativeAttribute.BorderLeftColor,
                (CammelCaseToCss(NativeAttribute.BorderLeftColor.ToString()), ColorValue)
            },
            {
                NativeAttribute.BorderLeftWidth,
                (CammelCaseToCss(NativeAttribute.BorderLeftWidth.ToString()), PixelValue)
            },
            {
                NativeAttribute.BorderRadius,
                (CammelCaseToCss(NativeAttribute.BorderRadius.ToString()), PixelValue)
            },
            {
                NativeAttribute.BorderRightColor,
                (CammelCaseToCss(NativeAttribute.BorderRightColor.ToString()), ColorValue)
            },
            {
                NativeAttribute.BorderRightWidth,
                (CammelCaseToCss(NativeAttribute.BorderRightWidth.ToString()), PixelValue)
            },
            {
                NativeAttribute.BorderStartColor,
                (CammelCaseToCss(NativeAttribute.BorderStartColor.ToString()), ColorValue)
            },
            {
                NativeAttribute.BorderStyle,
                (CammelCaseToCss(NativeAttribute.BorderStyle.ToString()), EnumValue)
            },
            {
                NativeAttribute.BorderTopColor,
                (CammelCaseToCss(NativeAttribute.BorderTopColor.ToString()), ColorValue)
            },
            {
                NativeAttribute.BorderTopEndRadius,
                (CammelCaseToCss(NativeAttribute.BorderTopEndRadius.ToString()), PixelValue)
            },
            {
                NativeAttribute.BorderTopLeftRadius,
                (CammelCaseToCss(NativeAttribute.BorderTopLeftRadius.ToString()), PixelValue)
            },
            {
                NativeAttribute.BorderTopRightRadius,
                (CammelCaseToCss(NativeAttribute.BorderTopRightRadius.ToString()), PixelValue)
            },
            {
                NativeAttribute.BorderTopStartRadius,
                (CammelCaseToCss(NativeAttribute.BorderTopStartRadius.ToString()), PixelValue)
            },
            {
                NativeAttribute.BorderTopWidth,
                (CammelCaseToCss(NativeAttribute.BorderTopWidth.ToString()), PixelValue)
            },
            {
                NativeAttribute.BorderWidth,
                (CammelCaseToCss(NativeAttribute.BorderWidth.ToString()), PixelValue)
            },
            {
                NativeAttribute.Bottom,
                (CammelCaseToCss(NativeAttribute.Bottom.ToString()), PixelValue)
            },
            {
                NativeAttribute.Color,
                (CammelCaseToCss(NativeAttribute.Color.ToString()), ColorValue)
            },
            {
                NativeAttribute.Direction,
                (CammelCaseToCss(NativeAttribute.Direction.ToString()), EnumValue)
            },
            {
                NativeAttribute.Display,
                (CammelCaseToCss(NativeAttribute.Display.ToString()), EnumValue)
            },
            {
                NativeAttribute.End,
                (CammelCaseToCss(NativeAttribute.End.ToString()), PixelValue)
            },
            {
                NativeAttribute.Flex,
                (CammelCaseToCss(NativeAttribute.Flex.ToString()), StringValue)
            },
            {
                NativeAttribute.FlexBasis,
                (CammelCaseToCss(NativeAttribute.FlexBasis.ToString()), StringValue)
            },
            {
                NativeAttribute.FlexDirection,
                (CammelCaseToCss(NativeAttribute.FlexDirection.ToString()), EnumValue)
            },
            {
                NativeAttribute.FlexGrow,
                (CammelCaseToCss(NativeAttribute.FlexGrow.ToString()), StringValue)
            },
            {
                NativeAttribute.FlexShrink,
                (CammelCaseToCss(NativeAttribute.FlexShrink.ToString()), StringValue)
            },
            {
                NativeAttribute.FlexWrap,
                (CammelCaseToCss(NativeAttribute.FlexWrap.ToString()), StringValue)
            },
            {
                NativeAttribute.FontFamily,
                (CammelCaseToCss(NativeAttribute.FontFamily.ToString()), StringValue)
            },
            {
                NativeAttribute.FontSize,
                (CammelCaseToCss(NativeAttribute.FontSize.ToString()), PixelValue)
            },
            {
                NativeAttribute.FontStyle,
                (CammelCaseToCss(NativeAttribute.FontStyle.ToString()), EnumValue)
            },
            {
                NativeAttribute.FontVariant,
                (CammelCaseToCss(NativeAttribute.FontVariant.ToString()), EnumValue)
            },
            {
                NativeAttribute.FontWeight,
                (CammelCaseToCss(NativeAttribute.FontWeight.ToString()), (value) =>
                {
                    var result = value.ToString()?.ToLower() ?? "";
                    if(result.First() == 'v')
                    {
                        return result.Substring(1);
                    }
                    return result;
                })
            },
            {
                NativeAttribute.Height,
                (CammelCaseToCss(NativeAttribute.Height.ToString()), PixelValue)
            },
            {
                NativeAttribute.JustifyContent,
                (CammelCaseToCss(NativeAttribute.JustifyContent.ToString()), EnumValue)
            },
            {
                NativeAttribute.Left,
                (CammelCaseToCss(NativeAttribute.Left.ToString()), PixelValue)
            },
            {
                NativeAttribute.LetterSpacing,
                (CammelCaseToCss(NativeAttribute.LetterSpacing.ToString()), PixelValue)
            },
            {
                NativeAttribute.LineHeight,
                (CammelCaseToCss(NativeAttribute.LineHeight.ToString()), PixelValue)
            },
            {
                NativeAttribute.Margin,
                (CammelCaseToCss(NativeAttribute.Margin.ToString()), PixelValue)
            },
            {
                NativeAttribute.MarginBottom,
                (CammelCaseToCss(NativeAttribute.MarginBottom.ToString()), PixelValue)
            },
            {
                NativeAttribute.MarginEnd,
                (CammelCaseToCss(NativeAttribute.MarginEnd.ToString()), PixelValue)
            },
            {
                NativeAttribute.MarginHorizontal,
                (CammelCaseToCss(NativeAttribute.MarginHorizontal.ToString()), PixelValue)
            },
            {
                NativeAttribute.MarginLeft,
                (CammelCaseToCss(NativeAttribute.MarginLeft.ToString()), PixelValue)
            },
            {
                NativeAttribute.MarginRight,
                (CammelCaseToCss(NativeAttribute.MarginRight.ToString()), PixelValue)
            },
            {
                NativeAttribute.MarginStart,
                (CammelCaseToCss(NativeAttribute.MarginStart.ToString()), PixelValue)
            },
            {
                NativeAttribute.MarginTop,
                (CammelCaseToCss(NativeAttribute.MarginTop.ToString()), PixelValue)
            },
            {
                NativeAttribute.MarginVertical,
                (CammelCaseToCss(NativeAttribute.MarginVertical.ToString()), PixelValue)
            },
            {
                NativeAttribute.MaxHeight,
                (CammelCaseToCss(NativeAttribute.MaxHeight.ToString()), PixelValue)
            },
            {
                NativeAttribute.MaxWidth,
                (CammelCaseToCss(NativeAttribute.MaxWidth.ToString()), PixelValue)
            },
            {
                NativeAttribute.MinHeight,
                (CammelCaseToCss(NativeAttribute.MinHeight.ToString()), PixelValue)
            },
            {
                NativeAttribute.MinWidth,
                (CammelCaseToCss(NativeAttribute.MinWidth.ToString()), PixelValue)
            },
            {
                NativeAttribute.Opacity,
                (CammelCaseToCss(NativeAttribute.Opacity.ToString()), StringValue)
            },
            {
                NativeAttribute.Overflow,
                (CammelCaseToCss(NativeAttribute.Overflow.ToString()), EnumValue)
            },
            {
                NativeAttribute.Padding,
                (CammelCaseToCss(NativeAttribute.Padding.ToString()), PixelValue)
            },
            {
                NativeAttribute.PaddingBottom,
                (CammelCaseToCss(NativeAttribute.PaddingBottom.ToString()), PixelValue)
            },
            {
                NativeAttribute.PaddingEnd,
                (CammelCaseToCss(NativeAttribute.PaddingEnd.ToString()), PixelValue)
            },
            {
                NativeAttribute.PaddingHorizontal,
                (CammelCaseToCss(NativeAttribute.PaddingHorizontal.ToString()), PixelValue)
            },
            {
                NativeAttribute.PaddingLeft,
                (CammelCaseToCss(NativeAttribute.PaddingLeft.ToString()), PixelValue)
            },
            {
                NativeAttribute.PaddingRight,
                (CammelCaseToCss(NativeAttribute.PaddingRight.ToString()), PixelValue)
            },
            {
                NativeAttribute.PaddingStart,
                (CammelCaseToCss(NativeAttribute.PaddingStart.ToString()), PixelValue)
            },
            {
                NativeAttribute.PaddingTop,
                (CammelCaseToCss(NativeAttribute.PaddingTop.ToString()), PixelValue)
            },
            {
                NativeAttribute.PaddingVertical,
                (CammelCaseToCss(NativeAttribute.PaddingVertical.ToString()), PixelValue)
            },
            {
                NativeAttribute.Position,
                (CammelCaseToCss(NativeAttribute.Position.ToString()), EnumValue)
            },
            {
                NativeAttribute.ResizeMode,
                (CammelCaseToCss(NativeAttribute.ResizeMode.ToString()), EnumValue)
            },
            {
                NativeAttribute.Right,
                (CammelCaseToCss(NativeAttribute.Right.ToString()), PixelValue)
            },
            //{ TODO image source
            //    NativeAttribute.Source,
            //    (CammelCaseToCss(NativeAttribute.AlignContent.ToString()), PixelValue)
            //},
            {
                NativeAttribute.Start,
                (CammelCaseToCss(NativeAttribute.Start.ToString()), PixelValue)
            },
            {
                NativeAttribute.TextAlign,
                (CammelCaseToCss(NativeAttribute.TextAlign.ToString()), EnumValue)
            },
            {
                NativeAttribute.TextDecorationLine,
                (CammelCaseToCss(NativeAttribute.TextDecorationLine.ToString()), EnumValue)
            },
            {
                NativeAttribute.TextShadowColor,
                (CammelCaseToCss(NativeAttribute.TextShadowColor.ToString()), ColorValue)
            },
            {
                NativeAttribute.TextShadowOffsetX,
                (CammelCaseToCss(NativeAttribute.TextShadowOffsetX.ToString()), PixelValue)
            },
            {
                NativeAttribute.TextShadowOffsetY,
                (CammelCaseToCss(NativeAttribute.TextShadowOffsetY.ToString()), PixelValue)
            },
            {
                NativeAttribute.TextShadowRadius,
                (CammelCaseToCss(NativeAttribute.TextShadowRadius.ToString()), PixelValue)
            },
            {
                NativeAttribute.TextTransform,
                (CammelCaseToCss(NativeAttribute.TextTransform.ToString()), EnumValue)
            },
            {
                NativeAttribute.Top,
                (CammelCaseToCss(NativeAttribute.Top.ToString()), PixelValue)
            },
            {
                NativeAttribute.Width,
                (CammelCaseToCss(NativeAttribute.Width.ToString()), PixelValue)
            },
            {
                NativeAttribute.ZIndex,
                (CammelCaseToCss(NativeAttribute.ZIndex.ToString()), StringValue)
            },
        };



        public static string GetCss(Dictionary<string, string> cssDict)
        {
            return string.Join("", cssDict.Select(s => $"{s.Key}: {s.Value};"));
        }

        public static string GetCssPropertyName(NativeAttribute prop)
        {
            return PropCssNameMap[prop].Name;
        }

        public static string GetCssValue(NativeAttribute prop, object value)
        {
            return PropCssNameMap[prop].GetValue(value);
        }

        static string CammelCaseToCss(string camelCase)
        {
            // convert from cammel case to css whatever case
            // inspired by https://stackoverflow.com/questions/63055621/how-to-convert-camel-case-to-snake-case-with-two-capitals-next-to-each-other

            if (camelCase.Length < 2)
            {
                return camelCase.ToLower();
            }
            var sb = new StringBuilder();
            sb.Append(char.ToLowerInvariant(camelCase[0]));
            for (int i = 1; i < camelCase.Length; ++i)
            {
                char c = camelCase[i];
                if (char.IsUpper(c))
                {
                    sb.Append('-');
                    sb.Append(char.ToLowerInvariant(c));
                }
                else
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        // https://stackoverflow.com/questions/2395438/convert-system-drawing-color-to-rgb-and-hex-value
        static string ConvertToHex(Color c)
        {
            return "#" + c.R.ToString("X2") + c.G.ToString("X2") + c.B.ToString("X2");
        }

    }
}
