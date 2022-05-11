using BlazorApp1;
using CSX;
using CSX.Web;
using System.Reflection;

var dom = new CSXWebDom();// new CSXDOM(interpo as IJSInProcessRuntime);

await CSXHostBuilder.Create(args, dom)
    .ConfigureServices((context, services) =>
    {
        services.AddAssemblyComponents(Assembly.GetExecutingAssembly());
    })
    .Build()
    .StartAsync<ComponentTest, TestProps>(new() { Name = "Alejandro", LastName = "Guardiola" });
