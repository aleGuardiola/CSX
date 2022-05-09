using BlazorApp1;
using CSX.Components;
using CSX.NativeComponents;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.JSInterop;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
//builder.RootComponents.Add<App>("#app");
//builder.RootComponents.Add<HeadOutlet>("head::after");

var descriptor = builder.Services.First(x => typeof(IJSInProcessRuntime).IsAssignableFrom(x.ImplementationInstance?.GetType()));
var interpo = descriptor.ImplementationInstance;

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

//await builder.Build().RunAsync();


var serviceCollection = new ServiceCollection();

serviceCollection.AddTransient<ComponentTest>();
serviceCollection.AddTransient<Text>();
serviceCollection.AddTransient<StringComponent>();
serviceCollection.AddTransient<View>();

var provider = serviceCollection.BuildServiceProvider();

#pragma warning disable CS8604 // Possible null reference argument.
var dom = new CSXDOM(interpo as IJSInProcessRuntime);
#pragma warning restore CS8604 // Possible null reference argument.

var virtualThing = ComponentFactory.CreateElement<ComponentTest, TestProps>(new() { Name = "Alejandr", LastName = "Guardiola" }, new List<CSX.Components.Element>());

IComponent? component = null;

var onRender = () =>
{
    component?.RenderView(dom);
    dom.AppendToDomIfNotAppended(component);
};

ComponentFactory.CreateComponent(virtualThing, provider, dom, onRender);

component = virtualThing.Component ?? throw new Exception("Component is null");

dom.AppendToDom(component);

component.RenderView(dom);

Console.WriteLine("Hello");
