// See https://aka.ms/new-console-template for more information
using CSX.OpenTK.Test;
using CSX.Rendering;
using CSX.Skia.Views;
using System.Drawing;
using OpenTK.Windowing.Desktop;
using CSX.Skia.Views.ScrollBars;
using CSX;
using System.Reflection;
using CSX.Skia;
using CSX.NativeComponents;
using OpenTK.Windowing.GraphicsLibraryFramework;

//View root;

//View ScrollView;
//View ScrollView2;
//View ScrollView3;

//root = new View(2020);//new ScrollView(2020, new DefaultScrollBarView(0));
////root.SetAttribute(NativeAttribute.Width, width);
////root.SetAttribute(NativeAttribute.Height, height);
//// root.SetAttribute(NativeAttribute.BackgroundColor, Color.Red);

////ScrollView = GetScrollView();
////ScrollView2 = GetScrollView();
////ScrollView3 = GetScrollView();

//var text = new TextInput(0);

//text.TextContent = "Hello ";

//root.AppendView(text);

////for (var i = 0; i < 3; i++)
////{
////    var sscrollView = GetScrollView((ulong)i + 1);
////    root.AppendView(sscrollView);
////}


////root.AppendView(ScrollView2);
////root.AppendView(ScrollView3);

//View GetScrollView(ulong id)
//{

//    var totalItems = 100;
//    var view = new ScrollView(id, new DefaultScrollBarView(0));
//    // view.SetAttribute(NativeAttribute.Width, 512f);
//    view.SetAttribute(NativeAttribute.Flex, 1f);
//    // view.SetAttribute(NativeAttribute.Height, 200f);
//    //view.SetAttribute(NativeAttribute.MarginRight, 20f);
//    // view.SetAttribute(NativeAttribute.BackgroundColor, Color.AliceBlue);
//    view.SetAttribute(NativeAttribute.BorderColor, Color.Black);
//    // view.SetAttribute(NativeAttribute.BorderWidth, 1f);
//    view.SetAttribute(NativeAttribute.MarginTop, 10f);
//    // view.SetAttribute(NativeAttribute.ScrollPosition, totalScroll/*RandomFloat(0, totalScroll)*/);

//    Random rnd = new Random();

//    for (int i = 0; i < totalItems; i++)
//    {
//        var text = new TextInput(1);

//        text.SetAttribute(NativeAttribute.BorderColor, Color.Black);
//        text.SetAttribute(NativeAttribute.MarginTop, 10f);
//        text.SetAttribute(NativeAttribute.FontWeight, CSX.NativeComponents.FontWeight.Bold);
//        // text.SetAttribute(NativeAttribute.BorderWidth, 1f);
//        text.SetAttribute(NativeAttribute.Width, 200f);
//        text.SetAttribute(NativeAttribute.Height, 13f);

//        text.TextContent = "Hello " + i;

//        view.AppendView(text);
//    }

//    return view;
//}

//// root.CalculateLayout();

//var scrollView = root.Children.First() as View;
//var totalScroll = scrollView.GetContentHeight() - (scrollView.YogaNode.LayoutHeight - scrollView.GetBorderTopWidth() - scrollView.GetBorderBottomWidth());

//window.Root = root;

var dom = new SkiaDom();

// Doing some ugly reflection cuz GLFWProvider is internal 😡
//var type = typeof(NativeWindow).Assembly.GetTypes().First(x => x.Name == "GLFWProvider");
//var initializeMethod = type.GetMethod("EnsureInitialized");
//initializeMethod?.Invoke(null, null);
//GLFW.WindowHint(WindowHintBool.TransparentFramebuffer, true);
//GLFW.WindowHint(WindowHintBool.Visible, true);

var window = new CSXWindow(dom,
    new GameWindowSettings()
    {
        RenderFrequency = 250,
        UpdateFrequency = 60.0,
        IsMultiThreaded = false
    },
    new NativeWindowSettings()
    {
        Title = "CSX",
        Size = new OpenTK.Mathematics.Vector2i(800, 600),          
    }, false, true);


CSXHostBuilder.Create(new string[0], dom)
    .ConfigureServices((context, services) =>
    {
        services.AddAssemblyComponents(Assembly.GetExecutingAssembly());
    })
    .Build()
    .StartAsync<ComponentTest, ViewProps>(new() {  });

window.Run();
