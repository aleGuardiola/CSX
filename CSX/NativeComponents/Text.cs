using CSX.Components;
using CSX.Rendering;
using System.Drawing;

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
        public Color? Color { get; init; }
        public string? FontFamily { get; init; }
        public float? FontSize { get; init; }
        public FontStyle? FontStyle { get; init; }
        public FontWeight? FontWeight { get; init; }
        public FontVariant[]? FontVariant { get; init; }
        public float? LetterSpacing { get; init; }
        public float? LineHeight { get; init; }
        public TextAlign? TextAlign { get; init; }
        public TextDecorationLine? TextDecorationLine { get; init; }
        public Color? TextShadowColor { get; init; }
        public float? TextShadowOffsetX { get; init; }
        public float? TextShadowOffsetY { get; init; }
        public float? TextShadowRadius { get; init; }
        public TextTransform? TextTransform { get; init; }
    }
    public record TextProps : ViewProps<TextStyleProps>
    {
        public string Text { get; init; } = "";
    }
    public class Text : Text<TextProps> { }
    public class Text<TProps> : View<TProps, TextStyleProps> where TProps : TextProps
    {
        protected override NativeElement Element => NativeElement.Text;

        protected override ulong OnInitialize(IDOM dom)
        {
            return base.OnInitialize(dom);            
        }

        protected override void Render(IDOM dom)
        {
            base.Render(dom);

            dom.SetAttributesIfDifferent(DOMElement, GetPropertiesWithValues().Select(x => new KeyValuePair<NativeAttribute, object?>(x.Name, x.Value)));
            dom.SetElementText(DOMElement, Props.Text);
        }

        IEnumerable<(NativeAttribute Name, object? Value)> GetPropertiesWithValues()
        {        
            // text styles
            yield return (NativeAttribute.Color,  Props.Style?.Color);
            yield return (NativeAttribute.FontFamily,  Props.Style?.FontFamily);
            yield return (NativeAttribute.FontSize,  Props.Style?.FontSize);
            yield return (NativeAttribute.FontStyle,  Props.Style?.FontStyle);
            yield return (NativeAttribute.FontWeight,  Props.Style?.FontWeight);
            yield return (NativeAttribute.FontVariant,  Props.Style?.FontVariant);
            yield return (NativeAttribute.LetterSpacing,  Props.Style?.LetterSpacing);
            yield return (NativeAttribute.LineHeight,  Props.Style?.LineHeight);
            yield return (NativeAttribute.TextAlign,  Props.Style?.TextAlign);
            yield return (NativeAttribute.TextDecorationLine,  Props.Style?.TextDecorationLine);
            yield return (NativeAttribute.TextShadowColor,  Props.Style?.TextShadowColor);
            yield return (NativeAttribute.TextShadowOffsetX,  Props.Style?.TextShadowOffsetX);
            yield return (NativeAttribute.TextShadowOffsetY,  Props.Style?.TextShadowOffsetY);
            yield return (NativeAttribute.TextShadowRadius,  Props.Style?.TextShadowRadius);
            yield return (NativeAttribute.TextTransform,  Props.Style?.TextTransform);
        }

    }
}
