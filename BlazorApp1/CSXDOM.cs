using CSX.Components;
using CSX.NativeComponents;
using CSX.Rendering;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Microsoft.JSInterop.Infrastructure;
using Microsoft.JSInterop.WebAssembly;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;

namespace BlazorApp1
{
    public class CSXDOM : IDOM
    {
        IJSInProcessRuntime _jsRuntime;

        public CSXDOM(IJSInProcessRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public void AppendChild(Guid parent, Guid child)
        {
            _jsRuntime.InvokeVoid("AppendChild", parent.ToString().ToLower(), child.ToString().ToLower());
        }

        public Guid CreateElement(string name)
        {
            return Guid.Parse(_jsRuntime.Invoke<string>("CreateElement", name));
        }

        public void DestroyElement(Guid id)
        {
            _jsRuntime.InvokeVoid("DestroyElement", id.ToString().ToLower());
        }

        public string? GetAttribute(Guid id, string name)
        {
            return _jsRuntime.Invoke<string>("GetAttribute", id.ToString().ToLower(), name);
        }

        public Guid GetRootElement()
        {
            return Guid.Parse(_jsRuntime.Invoke<string>("GetRootElement"));
        }

        public bool HasChild(Guid parent, Guid child)
        {
            return _jsRuntime.Invoke<bool>("HasChild", parent.ToString().ToLower(), child.ToString().ToLower());
        }

        public void Remove(Guid id)
        {
            _jsRuntime.InvokeVoid("Remove", id.ToString().ToLower());
        }

        public void SetAttribute(Guid id, string name, string? value)
        {
            _jsRuntime.InvokeVoid("SetAttribute", id.ToString().ToLower(), name, value);
        }
    }


    public static class ComponentFunctions
    {
        public static Element View(ViewProps? props = null, IEnumerable<Element>? children = null)
            => ComponentFactory.CreateElement<View, ViewProps>(props ?? new ViewProps(), children ?? new Element[0]);

        public static Element Text(TextProps? props = null, IEnumerable<Element>? children = null)
            => ComponentFactory.CreateElement<Text, TextProps>(props ?? new TextProps(), children ?? new Element[0]);

        public static Element String(string value)
            => ComponentFactory.CreateElement<StringComponent, StringProps>(new StringProps(value), new Element[0]);
    }


}
