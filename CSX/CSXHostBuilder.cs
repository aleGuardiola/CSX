using CSX.Components;
using CSX.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX
{
    public class CSXHostBuilder
    {
        readonly IHostBuilder _builder;
        readonly IDOM _dom;

        internal CSXHostBuilder(IHostBuilder builder, IDOM dom)
        {
            _builder = builder;
            _dom = dom;
        }

        public IDictionary<object, object> Properties => _builder.Properties;

        public CSXHost Build()
        {
            var host = _builder.Build();
            return new CSXHost(host);
        }

        public CSXHostBuilder ConfigureAppConfiguration(Action<HostBuilderContext, IConfigurationBuilder> configureDelegate)
        {
            _builder.ConfigureAppConfiguration(configureDelegate);
            return this;
        }

        public CSXHostBuilder ConfigureContainer<TContainerBuilder>(Action<HostBuilderContext, TContainerBuilder> configureDelegate)
        {
            _builder.ConfigureContainer(configureDelegate);
            return this;
        }

        public CSXHostBuilder ConfigureHostConfiguration(Action<IConfigurationBuilder> configureDelegate)
        {
            _builder.ConfigureHostConfiguration(configureDelegate);
            return this;
        }

        public CSXHostBuilder ConfigureServices(Action<HostBuilderContext, IServiceCollection> configureDelegate)
        {
            _builder.ConfigureServices((hostContext, services) =>
            {
                // CSX services
                services.AddCSX(_dom);

                configureDelegate(hostContext, services);
            });

            return this; ;
        }

        public CSXHostBuilder UseServiceProviderFactory<TContainerBuilder>(IServiceProviderFactory<TContainerBuilder> factory)
        {
            _builder.UseServiceProviderFactory(factory);
            return this;
        }

        public CSXHostBuilder UseServiceProviderFactory<TContainerBuilder>(Func<HostBuilderContext, IServiceProviderFactory<TContainerBuilder>> factory)
        {
            _builder.UseServiceProviderFactory(factory);
            return this;
        }



        public static CSXHostBuilder Create(string[] args, IDOM dom)
        {
            var builder = Host.CreateDefaultBuilder(args);

            return new CSXHostBuilder(builder, dom);
        }

    }

    public class CSXHost
    {
        IHost _host;

        Type? RootComponentType;
        Props? RootComponentProps;
        IComponent? RootComponent = null;
        Element? RootComponentElement = null;

        internal CSXHost(IHost host)
        {
            _host = host;
            var appLifetime = Services.GetRequiredService<IHostApplicationLifetime>();
            appLifetime.ApplicationStarted.Register(OnStarted);
            appLifetime.ApplicationStopping.Register(OnStopping);
        }

        private void OnStarted()
        {
            try
            {
                var dom = Services.GetRequiredService<IDOM>();

                var RootComponentElement = ComponentFactory.CreateElement(RootComponentType ?? throw new InvalidOperationException("Root component not setted"), RootComponentProps ?? throw new InvalidOperationException("Root component props not setted"), new List<Element>());

                var onRender = () =>
                {
                    if(RootComponent != null)
                    {
                        Stopwatch sw = Stopwatch.StartNew();
                        sw.Start();

                        RootComponent.RenderView(dom);
                        dom.AppendToDomIfNotAppended(RootComponent);

                        sw.Stop();
                        Console.WriteLine("Render View time {0}", sw.ElapsedMilliseconds);
                    }                    
                };

                Stopwatch sw = Stopwatch.StartNew();
                sw.Start();

                // create root component and append it to the dom
                ComponentFactory.CreateComponent(RootComponentElement, Services, dom, onRender, appendToDom: false);

                // first render
                RootComponentElement.Component?.RenderView(dom);
                dom.AppendToDom(RootComponentElement.Component);

                sw.Stop();
                Console.WriteLine("First Render Time {0}ms", sw.ElapsedMilliseconds);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex);
                throw;
            }
            
        }


        private void OnStopping()
        {
            var dom = Services.GetRequiredService<IDOM>();
            ComponentFactory.DestroyComponent(RootComponentElement ?? throw new InvalidOperationException("Root is null"), dom);
        }

        public IServiceProvider Services => _host.Services;

        public void Dispose()
        {
            _host.Dispose();
        }

        public Task StartAsync<TRootComponent, TProps>(TProps props, CancellationToken cancellationToken = default) where TRootComponent : class, IComponent<TProps>
                                                                                                                    where TProps : Props
        {
            RootComponentType = typeof(TRootComponent);
            RootComponentProps = props;

            return _host.StartAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken = default)
        {
            return _host.StopAsync(cancellationToken);
        }
    }

}
