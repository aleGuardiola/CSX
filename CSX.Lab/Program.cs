// See https://aka.ms/new-console-template for more information


//using BlazorApp1;
//using Castle.DynamicProxy;
//using CSX;
//using CSX.Components;
//using CSX.Lab;
//using CSX.Rendering;
//using System.Reactive.Subjects;
//using System.Reflection;


using CSX.Skia.Rendering.DOM;
using CSX.Skia.Rendering.Render;
using CSX.Skia.Rendering.RenderLayer;
using Facebook.Yoga;
using SkiaSharp;

var element = new Element()
{
    YogaNode = new YogaNode()
    {
        Width = 800f,
        Height = 600f,
    },

};

element.Add(new Element()
{
    BackgroundColor = SKColors.Red,
    Opacity = .5f,
    YogaNode = new YogaNode()
    {
        Flex = 1f,
        Height = 200f
    }
});


element.Add(new Element()
{
    BackgroundColor = SKColors.Red,
    Opacity = .5f,
    YogaNode = new YogaNode()
    {
        Flex = 1f,
        Height = 200f
    }
});


element.YogaNode.CalculateLayout();

var renderTree = RenderTree.Create(element);

var renderLayerTree = RenderLayerTree.Create(renderTree);

Console.Write(element.YogaNode.Print());

Console.Read();

//ulong id = 1;
//var realDom = new MemoryDom(new Subject<Event>(), () => id++);

//var averages = new Dictionary<string, Average>();

//var proxy = new ProxyGenerator()
//   .CreateInterfaceProxyWithTarget<IDOM>(realDom, new StopwatchInterceptor(averages));

////var dom = new WebDom();

//await CSXHostBuilder.Create(args, proxy)
//    .ConfigureServices((context, services) =>
//    {
//        services.AddAssemblyComponents(Assembly.GetExecutingAssembly());
//    })
//    .Build()
//    .StartAsync<ComponentTest, TestProps>(new() { Name = "Alejandro", LastName = "Guardiola" });

//Console.WriteLine($"Total elements: {realDom.Nodes.Count - 1}");
//Console.WriteLine($"Total Dom time: {averages.Sum(x => x.Value.Sum()) / TimeSpan.TicksPerMillisecond}ms");
//Console.WriteLine("-----------------------------------------------");
//Console.WriteLine("-----------------------------------------------");


//foreach (var average in averages)
//{
//    var methodName = average.Key;
//    var thicks = average.Value.GetAverageTicks();
//    var milliseconds = average.Value.GetAverageMilliseconds();

//    Console.WriteLine($"Method {methodName} time:");

//    Console.WriteLine($"Average {thicks}thicks");
//    Console.WriteLine($"Average {milliseconds}ms");

//    Console.WriteLine();

//    Console.WriteLine($"Total {average.Value.GetTotalThicks()}thicks");
//    Console.WriteLine($"Total {average.Value.GetTotalMilliseconds()}ms");

//    Console.WriteLine();

//    Console.WriteLine($"Total calls {average.Value.Count}");

//    Console.WriteLine("-----------------------------------------------");
//}

//Console.ReadKey();