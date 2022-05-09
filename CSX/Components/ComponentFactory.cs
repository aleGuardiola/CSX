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

        public static void AppendToDomIfNotAppended(this IDOM dom, IComponent component)
        {
            if (!dom.HasChild(dom.GetRootElement(), component.DOMElement))
            {
                dom.AppendChild(dom.GetRootElement(), component.DOMElement);
            }            
        }

        public static void SetAttributeIfDifferent(this IDOM dom, Guid element, string name, string? value)
        {
            var domValue = dom.GetAttribute(element, name);
            if (domValue != value)
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

        public static Element CreateElement<T, TProps>(TProps props, IEnumerable<Element> children) where T : IComponent<TProps>
                                                                                                                      where TProps : Props
            => CreateElement(typeof(T), props, children);
        
        public static Element CreateElement(Type type, Props props, IEnumerable<Element> children)
        {
            return new Element(type, props, children.ToArray());
        }

        public static Element UpdateTree(Element? current, Element @new, IServiceProvider serviceProvider, IDOM dom, Action? onRender)
        {
            if (current == null)
            {
                CreateComponent(@new, serviceProvider, dom, onRender);
                return @new;
            }

            if (current.Type != @new.Type)
            {
                DestroyComponent(current, dom);
                CreateComponent(@new, serviceProvider, dom, onRender);
                return @new;
            }           

            var currentNoKeys = current.Children.Where(c => string.IsNullOrEmpty(c.Props.Key)).ToArray();
            var newNoKeys = @new.Children.Where(c => string.IsNullOrEmpty(c.Props.Key)).ToArray();

            for (int i = 0; i < newNoKeys.Length; i++)
            {
                var currentChild = currentNoKeys.Length > i ? currentNoKeys[i] : null;
                var newChild = newNoKeys[i];
                UpdateTree(currentChild, newChild, serviceProvider, dom, onRender);
            }
            
            var currentWithKeys = current.Children.Where(x => !string.IsNullOrEmpty(x.Props.Key)).ToDictionary(x => x.Props.Key ?? throw new Exception("Key cannot be null"), x => x);
            var newWithKeys = @new.Children.Where(x => !string.IsNullOrEmpty(x.Props.Key)).ToDictionary(x => x.Props.Key ?? throw new Exception("Key cannot be null"), x => x);

            foreach (var key in newWithKeys.Keys)
            {
                var currentChild = currentWithKeys.GetValueOrDefault(key);
                var newChild = newWithKeys[key];
                UpdateTree(currentChild, newChild, serviceProvider, dom, onRender);
            }

            // Destroy removed children
            var newChildrenComponents = @new.Children.Select(x => x.Component ?? throw new InvalidOperationException("Could not update component")).ToArray();
            var removedChildren = current.Children.Where(x => !newChildrenComponents.Contains(x.Component));
            foreach (var child in removedChildren)
            {
                DestroyComponent(child, dom);
            }

            var component = current.Component ?? throw new InvalidOperationException("Could not update component");            
            component.SetProps(@new.Props);
            component.SetChildren(newChildrenComponents);
            @new.Component = component;

            return @new;
        }

        public static void CreateComponent(Element virtualComponent, IServiceProvider serviceProvider, IDOM dom, Action? onRender)
        {
            foreach (var child in virtualComponent.Children)
            {
                CreateComponent(child, serviceProvider, dom, onRender);
            }

            // construct component
            var component = (IComponent)(serviceProvider.GetService(virtualComponent.Type) ?? throw new Exception("Could not create component"));

            component.SetServiceProvider(serviceProvider);
            component.SetProps(virtualComponent.Props);
            component.SetChildren(virtualComponent.Children.Select(x => x.Component ?? throw new Exception("Could not create component")));
            component.Initialize(dom);
            component.OnRender(onRender);
            virtualComponent.Component = component;
        }

        public static void DestroyComponent(Element component, IDOM dom)
        {
            foreach (var child in component.Children)
            {
                DestroyComponent(child, dom);                
            }           

            component.Component?.Destroy(dom);            
        }
        
    }
}
