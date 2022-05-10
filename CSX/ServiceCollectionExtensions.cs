using CSX.Components;
using CSX.NativeComponents;
using CSX.Rendering;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace CSX
{
    public static class ServiceCollectionExtensions
    {
        internal static IServiceCollection AddCSX(this IServiceCollection services, IDOM dom)
        {
            // Add the dom
            services.AddSingleton(dom);

            // Add core components
            services.AddComponent<Image>();
            services.AddComponent<View>();
            services.AddComponent<Text>();
            services.AddComponent<StringComponent>();

            return services;            
        }

        public static void AddAssemblyComponents(this IServiceCollection services, Assembly assembly)
        {
            var components = assembly.GetTypes().Where(type => typeof(IComponent).IsAssignableFrom(type));
            foreach (var component in components)
            {
                services.AddComponent(component);
            }
        }

        public static void AddComponent<T>(this IServiceCollection services)
            where T : class, IComponent
        {
            services.AddTransient<T>();
        }

        public static void AddComponent(this IServiceCollection services, Type type)
        {
            if (!typeof(IComponent).IsAssignableFrom(type))
            {
                throw new InvalidOperationException("The type is not a component");
            }
            services.AddTransient(type);
        }

    }
}
