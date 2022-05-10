using CSX.Components;
using CSX.NativeComponents;

namespace BlazorApp1;

public static class ComponentFunctions
{
    public static Element View(ViewProps? props = null, IEnumerable<Element>? children = null)
        => ComponentFactory.CreateElement<View, ViewProps>(props ?? new ViewProps(), children ?? new Element[0]);

    public static Element Text(TextProps? props = null, IEnumerable<Element>? children = null)
        => ComponentFactory.CreateElement<Text, TextProps>(props ?? new TextProps(), children ?? new Element[0]);

    public static Element String(string? value)
        => ComponentFactory.CreateElement<StringComponent, StringProps>(new StringProps(value), new Element[0]);
}
