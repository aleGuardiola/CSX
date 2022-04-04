using CSX.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Components
{
    public static class ComponentFactory
    {
        public static void AppendToDom(this IDOM dom, IComponent component)
        {
            dom.AppendChild(dom.GetRootElement(), component.DOMElement);
        }

        public static void SetAttributeIfDifferent(this IDOM dom, Guid element, string name, string? value)
        {
            if (dom.GetAttribute(element, name) != value)
            {
                dom.SetAttribute(element, name, value);
            }
        }

        public static void AppendChildIfNotAppended(this IDOM dom, Guid parent, Guid child)
        {
            if (!dom.HasChild(parent, child))
            {
                dom.AppendChild(parent, child);
            }
        }

        public static VirtualComponent CreateElement<T, TProps>(TProps props, IEnumerable<VirtualComponent> children) where T : IComponent<TProps>
                                                                                                                      where TProps : Props
            => CreateElement(typeof(T), props, children);
        
        public static VirtualComponent CreateElement(Type type, Props props, IEnumerable<VirtualComponent> children)
        {
            return new VirtualComponent(type, props, children.ToArray());
        }

        public static VirtualComponent UpdateTree(VirtualComponent? current, VirtualComponent @new, IServiceProvider serviceProvider, IDOM dom)
        {
            if (current == null)
            {
                CreateComponent(@new, serviceProvider, dom);
                return @new;
            }

            if (current.Type != @new.Type)
            {
                DestroyComponent(current, dom);
                CreateComponent(@new, serviceProvider, dom);
                return @new;
            }           

            var currentNoKeys = current.Children.Where(c => string.IsNullOrEmpty(c.Props.Key)).ToArray();
            var newNoKeys = @new.Children.Where(c => string.IsNullOrEmpty(c.Props.Key)).ToArray();

            for (int i = 0; i < @new.Children.Length; i++)
            {
                var currentChild = currentNoKeys[i];
                var newChild = newNoKeys[i];
                UpdateTree(currentChild, newChild, serviceProvider, dom);
            }
            
            var currentWithKeys = current.Children.Where(x => !string.IsNullOrEmpty(x.Props.Key)).ToDictionary(x => x.Props.Key ?? throw new Exception("Key cannot be null"), x => x);
            var newWithKeys = @new.Children.Where(x => !string.IsNullOrEmpty(x.Props.Key)).ToDictionary(x => x.Props.Key ?? throw new Exception("Key cannot be null"), x => x);

            foreach (var key in newWithKeys.Keys)
            {
                var currentChild = currentWithKeys.GetValueOrDefault(key);
                var newChild = newWithKeys[key];
                UpdateTree(currentChild, newChild, serviceProvider, dom);
            }

            var component = current.Component ?? throw new InvalidOperationException("Could not update component");            
            component.SetProps(@new.Props);
            component.SetChildren(@new.Children.Select(x => x.Component ?? throw new InvalidOperationException("Could not update component")));
            @new.Component = component;

            return @new;
        }

        public static void CreateComponent(VirtualComponent virtualComponent, IServiceProvider serviceProvider, IDOM dom)
        {
            foreach (var child in virtualComponent.Children)
            {
                CreateComponent(child, serviceProvider, dom);
            }

            // construct component
            var component = (IComponent)(serviceProvider.GetService(virtualComponent.Type) ?? throw new Exception("Could not create component"));

            component.SetServiceProvider(serviceProvider);
            component.SetProps(virtualComponent.Props);
            component.SetChildren(virtualComponent.Children.Select(x => x.Component ?? throw new Exception("Could not create component")));
            component.Initialize(dom);
            virtualComponent.Component = component;
        }

        public static void DestroyComponent(VirtualComponent component, IDOM dom)
        {
            foreach (var child in component.Children)
            {
                DestroyComponent(child, dom);                
            }           

            component.Component?.Destroy(dom);            
        }
        
    }
}
