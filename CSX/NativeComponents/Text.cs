using CSX.Components;
using CSX.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CSX.NativeComponents
{
    public enum FontStyle
    {
        Normal,
        Italic
    }
    public enum FontWeight
    {
        Normal,
        Bold,
        V100,
        V200,
        V300,
        V400,
        V500,
        V600,
        V700,
        V800,
        V900
    }
    public enum FontVariant
    {
        SmallCaps,
        OldstyleNums,
        LiningNums,
        TabularNums,
        ProportionalNums,
    }
    public enum TextAlign
    {
        Left,
        Center,
        Right,
        Justify,
        Auto
    }
    public enum TextDecorationLine
    {
        Solid,
        Double,
        Dotted,
        Dashed
    }
    public enum TextTransform
    {
        None,
        Capitalize,
        Uppercase,
        Lowercase
    }
    public record TextStyleProps : ViewStyleProps
    {
        public string? Color { get; init; }
        public string? FontFamily { get; init; }
        public double? FontSize { get; init; }
        public FontStyle? FontStyle { get; init; }
        public FontWeight? FontWeight { get; init; }
        public FontVariant[]? FontVariant { get; init; }
        public double? LetterSpacing { get; init; }
        public double? LineHeight { get; init; }
        public TextAlign? TextAlign { get; init; }
        public TextDecorationLine? TextDecorationLine { get; init; }
        public string? TextShadowColor { get; init; }
        public double? TextShadowOffsetX { get; init; }
        public double? TextShadowOffsetY { get; init; }
        public double? TextShadowRadius { get; init; }
        public TextTransform? TextTransform { get; init; }

    }
    public record TextProps : ViewProps<TextStyleProps>
    {
        
    }
    public class Text : DOMComponent<TextProps>
    {
        const string name = "Text";
        protected override Guid OnInitialize(IDOM dom)
        {
            return dom.CreateElement(name);
        }

        protected override void Render(IDOM dom)
        {            
            foreach (var propValue in GetPropertiesWithValues())
            {
                dom.SetAttributeIfDifferent(DOMElement, propValue.Name, propValue.Value);                
            }
            
            // text
            dom.SetAttribute(DOMElement, "TextContent", (Children.FirstOrDefault() as StringComponent)?.Props.Value ?? "");
        }

        IEnumerable<(string Name, string? Value)> GetPropertiesWithValues()
        {
            // view styles
            yield return ($"Style.{nameof(ViewStyleProps.BackgroundColor)}", Props.Style?.BackgroundColor?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.BorderBottomColor)}", Props.Style?.BorderBottomColor?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.BorderBottomEndRadius)}", Props.Style?.BorderBottomEndRadius?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.BorderBottomLeftRadius)}", Props.Style?.BorderBottomLeftRadius?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.BorderBottomRightRadius)}", Props.Style?.BorderBottomRightRadius?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.BorderBottomStartRadius)}", Props.Style?.BorderBottomStartRadius?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.BorderBottomWidth)}", Props.Style?.BorderBottomWidth?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.BorderColor)}", Props.Style?.BorderColor?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.BorderEndColor)}", Props.Style?.BorderEndColor?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.BorderLeftColor)}", Props.Style?.BorderLeftColor?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.BorderLeftWidth)}", Props.Style?.BorderLeftWidth?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.BorderRadius)}", Props.Style?.BorderRadius?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.BorderRightColor)}", Props.Style?.BorderRightColor?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.BorderRightWidth)}", Props.Style?.BorderRightWidth?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.BorderStartColor)}", Props.Style?.BorderStartColor?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.BorderStyle)}", Props.Style?.BorderStyle?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.BorderTopColor)}", Props.Style?.BorderTopColor?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.BorderTopEndRadius)}", Props.Style?.BorderTopEndRadius?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.BorderTopLeftRadius)}", Props.Style?.BorderTopLeftRadius?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.BorderTopRightRadius)}", Props.Style?.BorderTopRightRadius?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.BorderTopStartRadius)}", Props.Style?.BorderTopStartRadius?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.BorderTopWidth)}", Props.Style?.BorderTopWidth?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.BorderWidth)}", Props.Style?.BorderWidth?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.Opacity)}", Props.Style?.Opacity?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.AlignContent)}", Props.Style?.AlignContent?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.AlignItems)}", Props.Style?.AlignItems?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.AlignSelf)}", Props.Style?.AlignSelf?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.AspectRatio)}", Props.Style?.AspectRatio?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.Bottom)}", Props.Style?.Bottom?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.Direction)}", Props.Style?.Direction?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.Display)}", Props.Style?.Display?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.End)}", Props.Style?.End?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.Flex)}", Props.Style?.Flex?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.FlexBasis)}", Props.Style?.FlexBasis?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.FlexDirection)}", Props.Style?.FlexDirection?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.FlexGrow)}", Props.Style?.FlexGrow?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.FlexShrink)}", Props.Style?.FlexShrink?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.FlexWrap)}", Props.Style?.FlexWrap?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.Height)}", Props.Style?.Height?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.JustifyContent)}", Props.Style?.JustifyContent?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.Left)}", Props.Style?.Left?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.Margin)}", Props.Style?.Margin?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.MarginBottom)}", Props.Style?.MarginBottom?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.MarginEnd)}", Props.Style?.MarginEnd?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.MarginHorizontal)}", Props.Style?.MarginHorizontal?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.MarginLeft)}", Props.Style?.MarginLeft?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.MarginRight)}", Props.Style?.MarginRight?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.MarginStart)}", Props.Style?.MarginStart?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.MarginTop)}", Props.Style?.MarginTop?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.MarginVertical)}", Props.Style?.MarginVertical?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.MaxHeight)}", Props.Style?.MaxHeight?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.MaxWidth)}", Props.Style?.MaxWidth?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.MinHeight)}", Props.Style?.MinHeight?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.MinWidth)}", Props.Style?.MinWidth?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.Overflow)}", Props.Style?.Overflow?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.Padding)}", Props.Style?.Padding?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.PaddingBottom)}", Props.Style?.PaddingBottom?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.PaddingEnd)}", Props.Style?.PaddingEnd?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.PaddingHorizontal)}", Props.Style?.PaddingHorizontal?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.PaddingLeft)}", Props.Style?.PaddingLeft?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.PaddingRight)}", Props.Style?.PaddingRight?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.PaddingStart)}", Props.Style?.PaddingStart?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.PaddingTop)}", Props.Style?.PaddingTop?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.PaddingVertical)}", Props.Style?.PaddingVertical?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.Position)}", Props.Style?.Position?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.Right)}", Props.Style?.Right?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.Start)}", Props.Style?.Start?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.Top)}", Props.Style?.Top?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.Width)}", Props.Style?.Width?.ToString());
            yield return ($"Style.{nameof(ViewStyleProps.ZIndex)}", Props.Style?.ZIndex?.ToString());


            // text styles
            yield return ($"Style.{nameof(TextStyleProps.Color)}", Props.Style?.Color?.ToString());
            yield return ($"Style.{nameof(TextStyleProps.FontFamily)}", Props.Style?.FontFamily?.ToString());
            yield return ($"Style.{nameof(TextStyleProps.FontSize)}", Props.Style?.FontSize?.ToString());
            yield return ($"Style.{nameof(TextStyleProps.FontStyle)}", Props.Style?.FontStyle?.ToString());
            yield return ($"Style.{nameof(TextStyleProps.FontWeight)}", Props.Style?.FontWeight?.ToString());
            yield return ($"Style.{nameof(TextStyleProps.FontVariant)}", Props.Style?.FontVariant?.ToString());
            yield return ($"Style.{nameof(TextStyleProps.LetterSpacing)}", Props.Style?.LetterSpacing?.ToString());
            yield return ($"Style.{nameof(TextStyleProps.LineHeight)}", Props.Style?.LineHeight?.ToString());
            yield return ($"Style.{nameof(TextStyleProps.TextAlign)}", Props.Style?.TextAlign?.ToString());
            yield return ($"Style.{nameof(TextStyleProps.TextDecorationLine)}", Props.Style?.TextDecorationLine?.ToString());
            yield return ($"Style.{nameof(TextStyleProps.TextShadowColor)}", Props.Style?.TextShadowColor?.ToString());
            yield return ($"Style.{nameof(TextStyleProps.TextShadowOffsetX)}", Props.Style?.TextShadowOffsetX?.ToString());
            yield return ($"Style.{nameof(TextStyleProps.TextShadowOffsetY)}", Props.Style?.TextShadowOffsetY?.ToString());
            yield return ($"Style.{nameof(TextStyleProps.TextShadowRadius)}", Props.Style?.TextShadowRadius?.ToString());
            yield return ($"Style.{nameof(TextStyleProps.TextTransform)}", Props.Style?.TextTransform?.ToString());
        }

        protected override void OnDestroy(IDOM dom)
        {
            dom.Remove(DOMElement);
            dom.DestroyElement(DOMElement);
        }

    }
}
