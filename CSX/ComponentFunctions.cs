using CSX.Components;
using CSX.NativeComponents;
using CSX.CoreComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX;


public static class ComponentFunctions
{
    public static Element View(ViewProps? props = null, Content? children = null)
        => ComponentFactory.CreateElement<View, ViewProps>(props ?? new ViewProps(), children ?? new Content());

    public static Element ScrollView(ScrollViewProps? props = null, Content? children = null)
        => ComponentFactory.CreateElement<ScrollView, ScrollViewProps>(props ?? new ScrollViewProps(), children ?? new Content());

    public static Element ListView<TData>(ListViewProps<TData>? props = null, Content? children = null)
        => ComponentFactory.CreateElement<ListView<TData>, ListViewProps<TData>>(props ?? new ListViewProps<TData>(), children ?? new Content());

    public static Element Text(TextProps? props = null, Content? children = null)
        => ComponentFactory.CreateElement<Text, TextProps>(props ?? new TextProps(), children ?? new Content());

    public static Element Image(ImageProps props, Content? children = null)
        => ComponentFactory.CreateElement<Image, ImageProps>(props, children ?? new Content());

    public static Element TextInput(TextInputProps? props = null, Content? children = null)
        => ComponentFactory.CreateElement<TextInput, TextInputProps>(props ?? new TextInputProps(), children ?? new Content());

    public static Element Button(ButtonProps? props = null, Content? children = null)
        => ComponentFactory.CreateElement<Button, ButtonProps>(props ?? new ButtonProps(), children ?? new Content());
}


