using BlazorApp1;
using CSX;
using CSX.Components;
using CSX.NativeComponents;
using CSX.Web;
//using Microsoft.AspNetCore.Components.Web;
//using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.JSInterop;
using System.Reflection;

//var builder = WebAssemblyHostBuilder.CreateDefault(args);
//builder.RootComponents.Add<App>("#app");
//builder.RootComponents.Add<HeadOutlet>("head::after");

//var descriptor = builder.Services.First(x => typeof(IJSInProcessRuntime).IsAssignableFrom(x.ImplementationInstance?.GetType()));
//var interpo = descriptor.ImplementationInstance;

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

//await builder.Build().RunAsync();

#pragma warning disable CS8604 // Possible null reference argument.
var dom = new CSXWebDom();// new CSXDOM(interpo as IJSInProcessRuntime);
#pragma warning restore CS8604 // Possible null reference argument.

await CSXHostBuilder.Create(args, dom)
    .ConfigureServices((context, services) =>
    {
        services.AddAssemblyComponents(Assembly.GetExecutingAssembly());
    })
    .Build()
    .StartAsync<ComponentTest, TestProps>(new() { Name = "Alejandro", LastName = "Guardiola" });
