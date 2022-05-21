using BlazorApp1;
using CSX;
using CSX.Rendering;
using CSX.Web;
using CSX.Web.Skia;
using SkiaSharp;
using System.Reactive.Subjects;
using System.Reflection;

ulong id = 1;
//var dom = new MemoryDom(new Subject<Event>(), () => id++);

var dom = new WebDom();

await CSXHostBuilder.Create(args, dom)
    .ConfigureServices((context, services) =>
    {
        services.AddAssemblyComponents(Assembly.GetExecutingAssembly());
    })
    .Build()
    .StartAsync<ComponentTest, TestProps>(new() { Name = "Alejandro", LastName = "Guardiola" });

//var skiaController = new SkiaCanvasController("skia_canvas");

//skiaController.OnPaintSurface = OnPaintSurface;
//skiaController.IgnorePixelScaling = true;
//skiaController.EnableRenderLoop = true;

//skiaController.OnAfterFirstRender();

//void OnPaintSurface(SKPaintSurfaceEventArgs obj)
//{
//    var canvas = obj.Surface.Canvas;
//    canvas.Clear(SKColors.Red);
//}