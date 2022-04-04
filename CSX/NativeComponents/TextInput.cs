using CSX.Components;
using CSX.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.NativeComponents
{
    
    public record TextInputProps : ViewProps<TextStyleProps>
    {

    }
    public class TextInput : DOMComponent<TextInputProps>
    {
        const string name = "TextInput";
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
        }

        IEnumerable<(string Name, string? Value)> GetPropertiesWithValues()
        {
            // styles
            yield return (nameof(ViewStyleProps.BackgroundColor), Props.Style?.BackgroundColor?.ToString());
            yield return (nameof(ViewStyleProps.BorderBottomColor), Props.Style?.BorderBottomColor?.ToString());
            yield return (nameof(ViewStyleProps.BorderBottomEndRadius), Props.Style?.BorderBottomEndRadius?.ToString());
            yield return (nameof(ViewStyleProps.BorderBottomLeftRadius), Props.Style?.BorderBottomLeftRadius?.ToString());
            yield return (nameof(ViewStyleProps.BorderBottomRightRadius), Props.Style?.BorderBottomRightRadius?.ToString());
            yield return (nameof(ViewStyleProps.BorderBottomStartRadius), Props.Style?.BorderBottomStartRadius?.ToString());
            yield return (nameof(ViewStyleProps.BorderBottomWidth), Props.Style?.BorderBottomWidth?.ToString());
            yield return (nameof(ViewStyleProps.BorderColor), Props.Style?.BorderColor?.ToString());
            yield return (nameof(ViewStyleProps.BorderEndColor), Props.Style?.BorderEndColor?.ToString());
            yield return (nameof(ViewStyleProps.BorderLeftColor), Props.Style?.BorderLeftColor?.ToString());
            yield return (nameof(ViewStyleProps.BorderLeftWidth), Props.Style?.BorderLeftWidth?.ToString());
            yield return (nameof(ViewStyleProps.BorderRadius), Props.Style?.BorderRadius?.ToString());
            yield return (nameof(ViewStyleProps.BorderRightColor), Props.Style?.BorderRightColor?.ToString());
            yield return (nameof(ViewStyleProps.BorderRightWidth), Props.Style?.BorderRightWidth?.ToString());
            yield return (nameof(ViewStyleProps.BorderStartColor), Props.Style?.BorderStartColor?.ToString());
            yield return (nameof(ViewStyleProps.BorderStyle), Props.Style?.BorderStyle?.ToString());
            yield return (nameof(ViewStyleProps.BorderTopColor), Props.Style?.BorderTopColor?.ToString());
            yield return (nameof(ViewStyleProps.BorderTopEndRadius), Props.Style?.BorderTopEndRadius?.ToString());
            yield return (nameof(ViewStyleProps.BorderTopLeftRadius), Props.Style?.BorderTopLeftRadius?.ToString());
            yield return (nameof(ViewStyleProps.BorderTopRightRadius), Props.Style?.BorderTopRightRadius?.ToString());
            yield return (nameof(ViewStyleProps.BorderTopStartRadius), Props.Style?.BorderTopStartRadius?.ToString());
            yield return (nameof(ViewStyleProps.BorderTopWidth), Props.Style?.BorderTopWidth?.ToString());
            yield return (nameof(ViewStyleProps.BorderWidth), Props.Style?.BorderWidth?.ToString());
            yield return (nameof(ViewStyleProps.Opacity), Props.Style?.Opacity?.ToString());
            yield return (nameof(ViewStyleProps.AlignContent), Props.Style?.AlignContent?.ToString());
            yield return (nameof(ViewStyleProps.AlignItems), Props.Style?.AlignItems?.ToString());
            yield return (nameof(ViewStyleProps.AlignSelf), Props.Style?.AlignSelf?.ToString());
            yield return (nameof(ViewStyleProps.AspectRatio), Props.Style?.AspectRatio?.ToString());
            yield return (nameof(ViewStyleProps.Bottom), Props.Style?.Bottom?.ToString());
            yield return (nameof(ViewStyleProps.Direction), Props.Style?.Direction?.ToString());
            yield return (nameof(ViewStyleProps.Display), Props.Style?.Display?.ToString());
            yield return (nameof(ViewStyleProps.End), Props.Style?.End?.ToString());
            yield return (nameof(ViewStyleProps.Flex), Props.Style?.Flex?.ToString());
            yield return (nameof(ViewStyleProps.FlexBasis), Props.Style?.FlexBasis?.ToString());
            yield return (nameof(ViewStyleProps.FlexDirection), Props.Style?.FlexDirection?.ToString());
            yield return (nameof(ViewStyleProps.FlexGrow), Props.Style?.FlexGrow?.ToString());
            yield return (nameof(ViewStyleProps.FlexShrink), Props.Style?.FlexShrink?.ToString());
            yield return (nameof(ViewStyleProps.FlexWrap), Props.Style?.FlexWrap?.ToString());
            yield return (nameof(ViewStyleProps.Height), Props.Style?.Height?.ToString());
            yield return (nameof(ViewStyleProps.JustifyContent), Props.Style?.JustifyContent?.ToString());
            yield return (nameof(ViewStyleProps.Left), Props.Style?.Left?.ToString());
            yield return (nameof(ViewStyleProps.Margin), Props.Style?.Margin?.ToString());
            yield return (nameof(ViewStyleProps.MarginBottom), Props.Style?.MarginBottom?.ToString());
            yield return (nameof(ViewStyleProps.MarginEnd), Props.Style?.MarginEnd?.ToString());
            yield return (nameof(ViewStyleProps.MarginHorizontal), Props.Style?.MarginHorizontal?.ToString());
            yield return (nameof(ViewStyleProps.MarginLeft), Props.Style?.MarginLeft?.ToString());
            yield return (nameof(ViewStyleProps.MarginRight), Props.Style?.MarginRight?.ToString());
            yield return (nameof(ViewStyleProps.MarginStart), Props.Style?.MarginStart?.ToString());
            yield return (nameof(ViewStyleProps.MarginTop), Props.Style?.MarginTop?.ToString());
            yield return (nameof(ViewStyleProps.MarginVertical), Props.Style?.MarginVertical?.ToString());
            yield return (nameof(ViewStyleProps.MaxHeight), Props.Style?.MaxHeight?.ToString());
            yield return (nameof(ViewStyleProps.MaxWidth), Props.Style?.MaxWidth?.ToString());
            yield return (nameof(ViewStyleProps.MinHeight), Props.Style?.MinHeight?.ToString());
            yield return (nameof(ViewStyleProps.MinWidth), Props.Style?.MinWidth?.ToString());
            yield return (nameof(ViewStyleProps.Overflow), Props.Style?.Overflow?.ToString());
            yield return (nameof(ViewStyleProps.Padding), Props.Style?.Padding?.ToString());
            yield return (nameof(ViewStyleProps.PaddingBottom), Props.Style?.PaddingBottom?.ToString());
            yield return (nameof(ViewStyleProps.PaddingEnd), Props.Style?.PaddingEnd?.ToString());
            yield return (nameof(ViewStyleProps.PaddingHorizontal), Props.Style?.PaddingHorizontal?.ToString());
            yield return (nameof(ViewStyleProps.PaddingLeft), Props.Style?.PaddingLeft?.ToString());
            yield return (nameof(ViewStyleProps.PaddingRight), Props.Style?.PaddingRight?.ToString());
            yield return (nameof(ViewStyleProps.PaddingStart), Props.Style?.PaddingStart?.ToString());
            yield return (nameof(ViewStyleProps.PaddingTop), Props.Style?.PaddingTop?.ToString());
            yield return (nameof(ViewStyleProps.PaddingVertical), Props.Style?.PaddingVertical?.ToString());
            yield return (nameof(ViewStyleProps.Position), Props.Style?.Position?.ToString());
            yield return (nameof(ViewStyleProps.Right), Props.Style?.Right?.ToString());
            yield return (nameof(ViewStyleProps.Start), Props.Style?.Start?.ToString());
            yield return (nameof(ViewStyleProps.Top), Props.Style?.Top?.ToString());
            yield return (nameof(ViewStyleProps.Width), Props.Style?.Width?.ToString());
            yield return (nameof(ViewStyleProps.ZIndex), Props.Style?.ZIndex?.ToString());
        }

        protected override void OnDestroy(IDOM dom)
        {
            dom.Remove(DOMElement);
            dom.DestroyElement(DOMElement);
        }

    }
}
