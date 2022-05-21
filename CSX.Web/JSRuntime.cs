using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;
using Microsoft.JSInterop.WebAssembly;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Web
{
    public static class CSXJSRuntime
    {

        static IJSRuntime _current;
        static CSXJSRuntime()
        {
            _current = WebAssemblyHostBuilder.CreateDefault().Services.BuildServiceProvider().GetRequiredService<IJSRuntime>();

        }

        public static IJSRuntime GetCurrent()
        {
            return _current;

            //var assembly = typeof(Microsoft.AspNetCore.Components.BindConverter).Assembly;
            //var name = assembly.FullName;
            // Microsoft.AspNetCore.Components.WebAssembly


            //var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(x => x.FullName?.Contains("Microsoft.AspNetCore.Components.WebAssembly") ?? false);

            //var defaultRuntime = assembly.GetTypes().FirstOrDefault(x => x.Name.Contains("DefaultWebAssemblyJSRuntime"));
            //var name = defaultRuntime?.Name;

            // WebAssemblyHostBuilder.CreateDefault();

            //var assembly = typeof(WebAssemblyHostBuilder).Assembly;
            //var defaultRuntime = assembly.GetTypes().FirstOrDefault(x => x.Name.Contains("DefaultWebAssemblyJSRuntime"));

            //var properties = defaultRuntime.GetProperties(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Static);
            //var propName = properties.First().Name;

            //var instance = defaultRuntime.GetProperty("Instance", BindingFlags.Static | BindingFlags.NonPublic);

        }
            
    }
}
