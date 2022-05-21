using CSX.Components;
using CSX.Rendering;
using System.Text.Json;

namespace CSX.NativeComponents
{
    public enum ResizeMode
    {
        Cover,
        Contain,
        Stretch,
        Center,
        Repeat
    }
    public record ImageStyleProps : ViewStyleProps
    {
        [NativeAttribute(NativeAttribute.ResizeMode)]
        public ResizeMode? ResizeMode { get; init; } = NativeComponents.ResizeMode.Cover;
    }
    public record ImageSource(string Uri, double? Width = null, double? Height = null, double? Scale = null, string? Method = null, Dictionary<string, string>? Headers = null, string? Body = null);
    public record ImageProps : ViewProps<ImageStyleProps>
    {
        [NativeAttribute(NativeAttribute.Source)]
        public ImageSource? Source { get; init; }
    }
    public class Image : View<ImageProps, ImageStyleProps>
    {
        protected override NativeElement Element => NativeElement.Image;

        protected override ulong OnInitialize(IDOM dom)
        {
            return base.OnInitialize(dom);
        }

        protected override void Render(IDOM dom)
        {
            base.Render(dom);
            dom.SetAttributesIfDifferent(DOMElement, GetPropertiesWithValues().Select(x => new KeyValuePair<NativeAttribute, object?>(x.Name, x.Value)));
            dom.SetAttributeIfDifferent(DOMElement, NativeAttribute.Source, Props.Source);
        }

        IEnumerable<(NativeAttribute Name, object? Value)> GetPropertiesWithValues()
        {
            // image styles
            yield return (NativeAttribute.ResizeMode, Props.Style?.ResizeMode);
        }
    }
}
