using CSX.Components;
using CSX.Rendering;

namespace CSX.NativeComponents
{
    public enum BorderStyle
    {
        Solid,
        Dotted,
        Dashed
    }
    public record ViewStyleProps : LayoutStyleProps
    {
        public string? BackgroundColor { get; init; }
        public string? BorderBottomColor { get; init; }
        public double? BorderBottomEndRadius { get; init; }
        public double? BorderBottomLeftRadius { get; init; }
        public double? BorderBottomRightRadius { get; init; }
        public double? BorderBottomStartRadius { get; init; }
        public double? BorderBottomWidth { get; init; }
        public string? BorderColor { get; init; }
        public string? BorderEndColor { get; init; }
        public string? BorderLeftColor { get; init; }
        public double? BorderLeftWidth { get; init; }
        public double? BorderRadius { get; init; }
        public string? BorderRightColor { get; init; }
        public double? BorderRightWidth { get; init; }
        public string? BorderStartColor { get; init; }
        public BorderStyle? BorderStyle { get; init; }
        public string? BorderTopColor { get; init; }
        public double? BorderTopEndRadius { get; init; }
        public double? BorderTopLeftRadius { get; init; }
        public double? BorderTopRightRadius { get; init; }
        public double? BorderTopStartRadius { get; init; }
        public double? BorderTopWidth { get; init; }
        public double? BorderWidth { get; init; }
        public double? Opacity { get; init; }
    }
    public record ViewProps<TStyle> : Props where TStyle : ViewStyleProps
    {
        public TStyle? Style { get; init; }
    }
    public record ViewProps : ViewProps<ViewStyleProps>;    
    public class View : DOMComponent<ViewProps>
    {
        const string name = "View";
        protected override Guid OnInitialize(IDOM dom)
        {
            return dom.CreateElement(name);
        }

        protected override void Render(IDOM dom)
        {
            foreach(var propValue in GetPropertiesWithValues())
            {
                dom.SetAttributeIfDifferent(DOMElement, propValue.Name, propValue.Value);
            }

            foreach (var child in Children)
            {
                dom.AppendChildIfNotAppended(DOMElement, child.DOMElement);
            }
        }

        IEnumerable<(string Name, string? Value)> GetPropertiesWithValues()
        {
            // styles
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
        }

        protected override void OnDestroy(IDOM dom)
        {
            dom.Remove(DOMElement);
            dom.DestroyElement(DOMElement);
        }

    }
}
