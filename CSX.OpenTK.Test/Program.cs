// See https://aka.ms/new-console-template for more information
using CSX.OpenTK.Test;
using CSX.Rendering;
using CSX.Skia.Views;
using System.Drawing;
using OpenTK.Windowing.Desktop;

var window = new CSXWindow(new NativeWindowSettings()
{
    Title = "CSX",
    Size = new OpenTK.Mathematics.Vector2i(800, 600)
});

View root;

View ScrollView;
View ScrollView2;
View ScrollView3;

root = new ScrollView(0);
//root.SetAttribute(NativeAttribute.Width, width);
//root.SetAttribute(NativeAttribute.Height, height);
root.SetAttribute(NativeAttribute.BackgroundColor, Color.Red);

ScrollView = GetScrollView();
ScrollView2 = GetScrollView();
ScrollView3 = GetScrollView();

root.AppendView(ScrollView);
root.AppendView(ScrollView2);
root.AppendView(ScrollView3);

View GetScrollView()
{

    var totalItems = 100f;
    var view = new View(0);
    // view.SetAttribute(NativeAttribute.Width, 512f);
    //view.SetAttribute(NativeAttribute.Flex, 1f);
    //view.SetAttribute(NativeAttribute.Height, 500f);
    view.SetAttribute(NativeAttribute.MarginRight, 20f);
    //view.SetAttribute(NativeAttribute.BackgroundColor, Color.Blue);
    view.SetAttribute(NativeAttribute.BorderColor, Color.Black);
    view.SetAttribute(NativeAttribute.BorderWidth, 1f);
    view.SetAttribute(NativeAttribute.MarginTop, 10f);
    // view.SetAttribute(NativeAttribute.ScrollPosition, totalScroll/*RandomFloat(0, totalScroll)*/);

    Random rnd = new Random();

    for (int i = 0; i < totalItems; i++)
    {
        var text = new Text(1);

        text.SetAttribute(NativeAttribute.BorderColor, Color.Black);
        text.SetAttribute(NativeAttribute.MarginTop, 10f);
        text.SetAttribute(NativeAttribute.FontWeight, CSX.NativeComponents.FontWeight.Bold);
        // text.SetAttribute(NativeAttribute.BorderWidth, 1f);
        text.SetAttribute(NativeAttribute.Width, 200f);
        text.SetAttribute(NativeAttribute.Height, 13f);

        text.TextContent = "Hello " + i;

        view.AppendView(text);
    }

    return view;
}

// root.CalculateLayout();

var scrollView = root.Children.First() as View;
var totalScroll = scrollView.GetContentHeight() - (scrollView.YogaNode.LayoutHeight - scrollView.GetBorderTopWidth() - scrollView.GetBorderBottomWidth());

window.Root = root;

window.Run();