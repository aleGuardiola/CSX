using CSX.Components;
using CSX.Events;
using CSX.Rendering;
using System.Drawing;
using System.Reactive.Linq;
using System.Text.Json;

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
        public Color? BackgroundColor { get; init; }

        public Color? BorderBottomColor { get; init; }

        public float? BorderBottomEndRadius { get; init; }

        public float? BorderBottomLeftRadius { get; init; }

        public float? BorderBottomRightRadius { get; init; }

        public float? BorderBottomStartRadius { get; init; }

        public float? BorderBottomWidth { get; init; }

        public Color? BorderColor { get; init; }

        public Color? BorderEndColor { get; init; }

        public Color? BorderLeftColor { get; init; }

        public float? BorderLeftWidth { get; init; }

        public float? BorderRadius { get; init; }

        public Color? BorderRightColor { get; init; }

        public float? BorderRightWidth { get; init; }

        public Color? BorderStartColor { get; init; }

        public BorderStyle? BorderStyle { get; init; }

        public Color? BorderTopColor { get; init; }

        public float? BorderTopEndRadius { get; init; }

        public float? BorderTopLeftRadius { get; init; }

        public float? BorderTopRightRadius { get; init; }

        public float? BorderTopStartRadius { get; init; }

        public float? BorderTopWidth { get; init; }

        public float? BorderWidth { get; init; }

        public float? Opacity { get; init; }
    }
    public record ViewProps<TStyle> : Props where TStyle : ViewStyleProps
    {
        public Action<CursorEventArgs>? OnPress { get; init; }
        public Action<CursorEventArgs>? OnMouseOver { get; init; }
        public Action<CursorEventArgs>? OnMouseOut { get; init; }
        public TStyle? Style { get; init; } = Activator.CreateInstance<TStyle>();
    }
    public record ViewProps : ViewProps<ViewStyleProps>;
    public class View : View<ViewProps, ViewStyleProps> { }
    public class View<TProps, TStyles> : DOMComponent<TProps> where TProps : ViewProps<TStyles> where TStyles : ViewStyleProps
    {
        protected virtual NativeElement Element => NativeElement.View;

        protected override ulong OnInitialize(IDOM dom)
        {
            var elementId = dom.CreateElement(Element);

            dom.Events.RedirectToCallback(this, NativeEvent.Click, (p) => p.OnPress);
            dom.Events.RedirectToCallback(this, NativeEvent.MouseOver, (p) => p.OnMouseOver);
            dom.Events.RedirectToCallback(this, NativeEvent.MouseOut, (p) => p.OnMouseOut);
            
            return elementId;
        }

        protected override void Render(IDOM dom)
        {            
            dom.SetAttributesIfDifferent(DOMElement, GetPropertiesWithValues().Select(x => new KeyValuePair<NativeAttribute, object?>(x.Name, x.Value)));
            RenderChildren(dom);
        }

        IEnumerable<(NativeAttribute Name, object? Value)> GetPropertiesWithValues()
        {
            // styles
            yield return (NativeAttribute.BackgroundColor,   Props.Style?.BackgroundColor);
            yield return (NativeAttribute.BorderBottomColor,   Props.Style?.BorderBottomColor);
            yield return (NativeAttribute.BorderBottomEndRadius,   Props.Style?.BorderBottomEndRadius);
            yield return (NativeAttribute.BorderBottomLeftRadius,   Props.Style?.BorderBottomLeftRadius);
            yield return (NativeAttribute.BorderBottomRightRadius,   Props.Style?.BorderBottomRightRadius);
            yield return (NativeAttribute.BorderBottomStartRadius,   Props.Style?.BorderBottomStartRadius);
            yield return (NativeAttribute.BorderBottomWidth,   Props.Style?.BorderBottomWidth);
            yield return (NativeAttribute.BorderColor,   Props.Style?.BorderColor);
            yield return (NativeAttribute.BorderEndColor,   Props.Style?.BorderEndColor);
            yield return (NativeAttribute.BorderLeftColor,   Props.Style?.BorderLeftColor);
            yield return (NativeAttribute.BorderLeftWidth,   Props.Style?.BorderLeftWidth);
            yield return (NativeAttribute.BorderRadius,   Props.Style?.BorderRadius);
            yield return (NativeAttribute.BorderRightColor,   Props.Style?.BorderRightColor);
            yield return (NativeAttribute.BorderRightWidth,   Props.Style?.BorderRightWidth);
            yield return (NativeAttribute.BorderStartColor,   Props.Style?.BorderStartColor);
            yield return (NativeAttribute.BorderStyle,   Props.Style?.BorderStyle);
            yield return (NativeAttribute.BorderTopColor,   Props.Style?.BorderTopColor);
            yield return (NativeAttribute.BorderTopEndRadius,   Props.Style?.BorderTopEndRadius);
            yield return (NativeAttribute.BorderTopLeftRadius,   Props.Style?.BorderTopLeftRadius);
            yield return (NativeAttribute.BorderTopRightRadius,   Props.Style?.BorderTopRightRadius);
            yield return (NativeAttribute.BorderTopStartRadius,   Props.Style?.BorderTopStartRadius);
            yield return (NativeAttribute.BorderTopWidth,   Props.Style?.BorderTopWidth);
            yield return (NativeAttribute.BorderWidth,   Props.Style?.BorderWidth);
            yield return (NativeAttribute.Opacity,   Props.Style?.Opacity);
            yield return (NativeAttribute.AlignContent,   Props.Style?.AlignContent);
            yield return (NativeAttribute.AlignItems,   Props.Style?.AlignItems);
            yield return (NativeAttribute.AlignSelf,   Props.Style?.AlignSelf);
            yield return (NativeAttribute.AspectRatio,   Props.Style?.AspectRatio);
            yield return (NativeAttribute.Bottom,   Props.Style?.Bottom);
            yield return (NativeAttribute.Direction,   Props.Style?.Direction);
            yield return (NativeAttribute.Display,   Props.Style?.Display);
            yield return (NativeAttribute.End,   Props.Style?.End);
            yield return (NativeAttribute.Flex,   Props.Style?.Flex);
            yield return (NativeAttribute.FlexBasis,   Props.Style?.FlexBasis);
            yield return (NativeAttribute.FlexDirection,   Props.Style?.FlexDirection);
            yield return (NativeAttribute.FlexGrow,   Props.Style?.FlexGrow);
            yield return (NativeAttribute.FlexShrink,   Props.Style?.FlexShrink);
            yield return (NativeAttribute.FlexWrap,   Props.Style?.FlexWrap);
            yield return (NativeAttribute.Height,   Props.Style?.Height);
            yield return (NativeAttribute.JustifyContent,   Props.Style?.JustifyContent);
            yield return (NativeAttribute.Left,   Props.Style?.Left);
            yield return (NativeAttribute.Margin,   Props.Style?.Margin);
            yield return (NativeAttribute.MarginBottom,   Props.Style?.MarginBottom);
            yield return (NativeAttribute.MarginEnd,   Props.Style?.MarginEnd);
            yield return (NativeAttribute.MarginHorizontal,   Props.Style?.MarginHorizontal);
            yield return (NativeAttribute.MarginLeft,   Props.Style?.MarginLeft);
            yield return (NativeAttribute.MarginRight,   Props.Style?.MarginRight);
            yield return (NativeAttribute.MarginStart,   Props.Style?.MarginStart);
            yield return (NativeAttribute.MarginTop,   Props.Style?.MarginTop);
            yield return (NativeAttribute.MarginVertical,   Props.Style?.MarginVertical);
            yield return (NativeAttribute.MaxHeight,   Props.Style?.MaxHeight);
            yield return (NativeAttribute.MaxWidth,   Props.Style?.MaxWidth);
            yield return (NativeAttribute.MinHeight,   Props.Style?.MinHeight);
            yield return (NativeAttribute.MinWidth,   Props.Style?.MinWidth);
            yield return (NativeAttribute.Overflow,   Props.Style?.Overflow);
            yield return (NativeAttribute.Padding,   Props.Style?.Padding);
            yield return (NativeAttribute.PaddingBottom,   Props.Style?.PaddingBottom);
            yield return (NativeAttribute.PaddingEnd,   Props.Style?.PaddingEnd);
            yield return (NativeAttribute.PaddingHorizontal,   Props.Style?.PaddingHorizontal);
            yield return (NativeAttribute.PaddingLeft,   Props.Style?.PaddingLeft);
            yield return (NativeAttribute.PaddingRight,   Props.Style?.PaddingRight);
            yield return (NativeAttribute.PaddingStart,   Props.Style?.PaddingStart);
            yield return (NativeAttribute.PaddingTop,   Props.Style?.PaddingTop);
            yield return (NativeAttribute.PaddingVertical,   Props.Style?.PaddingVertical);
            yield return (NativeAttribute.Position,   Props.Style?.Position);
            yield return (NativeAttribute.Right,   Props.Style?.Right);
            yield return (NativeAttribute.Start,   Props.Style?.Start);
            yield return (NativeAttribute.Top,   Props.Style?.Top);
            yield return (NativeAttribute.Width,   Props.Style?.Width);
            yield return (NativeAttribute.ZIndex,   Props.Style?.ZIndex);
        }

        protected override void OnDestroy(IDOM dom)
        {            
            dom.Remove(DOMElement);
            dom.DestroyElement(DOMElement);
        }

    }
}
