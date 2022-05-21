using CSX.Rendering;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public static void SetAttributeIfDifferent(this IDOM dom, ulong element, NativeAttribute name, object? value)
        {
            var domValue = dom.GetAttribute(element, name);
            if (domValue != value)
            {
                dom.SetAttribute(element, name, value);
            }
        }

        public static void SetAttributesIfDifferent(this IDOM dom, ulong element, IEnumerable<KeyValuePair<NativeAttribute, object?>> attributes)
        {
            var filtered = attributes.Where(x => dom.GetAttribute(element, x.Key) != x.Value).ToArray();
            dom.SetAttributes(element, filtered);
        }

        public static void AppendChildIfNotAppended(this IDOM dom, ulong parent, ulong child)
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
            //var sw = Stopwatch.StartNew();
            //sw.Start();

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
                        
            var currentChildren = current.Children;
            var newChildren = @new.Children;

#pragma warning disable CS8714
            var currentChildrenWithKeys = currentChildren.Where(cc => !string.IsNullOrEmpty(cc.Props.Key) && newChildren.Any(nc => nc.Props.Key == cc.Props.Key) ).ToDictionary(x => x.Props.Key, x => x);

            var currentChildrenWithNoKeys = currentChildren.Where(x => string.IsNullOrEmpty(x.Props.Key) || !currentChildrenWithKeys.ContainsKey(x.Props.Key)).ToArray();

            int j = 0;
            for (int i = 0; i < newChildren.Length; i++)
            {
                var child = newChildren[i];
                
                if(!string.IsNullOrEmpty(child?.Props.Key) && currentChildrenWithKeys.ContainsKey(child?.Props.Key))
                {
                    var currentWithKey = currentChildrenWithKeys[child.Props.Key];
                    child.Children = currentWithKey.Children;
                    child.Component = currentWithKey.Component;                    
                }
                else
                {
                    // This means there is no children to match so the component has to be created
                    if(j >= currentChildrenWithNoKeys.Length)
                    {
                        UpdateTree(null, child, serviceProvider, dom, onRender);
                    }
                    else
                    {
                        var currentWihNoKey = currentChildrenWithNoKeys[j];
                        try
                        {
                            UpdateTree(currentWihNoKey, child, serviceProvider, dom, onRender);
                        } catch(Exception ex)
                        {
                            Console.Error.WriteLine(ex);
                        }
                        
                        j++;
                    }
                    
                }

                //sw.Stop();
                //Console.WriteLine("Update Tree took: {0}ms", sw.ElapsedMilliseconds);

            }
#pragma warning restore CS8714

            // Destroy the children to be destroyed
            var childrenToDestroy = currentChildren.Where(x => !newChildren.Any(y => ReferenceEquals(y.Component, x.Component))).ToArray();
            foreach(var childToDestroy in childrenToDestroy)
            {
                DestroyComponent(childToDestroy, dom);
            }


            var component = current.Component ?? throw new InvalidOperationException("Could not update component");

            // Set children first because if props changed it will re render and can try to append already destroyed elements
            component.SetChildren(newChildren.Select(x => x.Component));
            component.SetProps(@new.Props);            
            @new.Component = component;

            return @new;
        }

        public static void CreateComponent(Element virtualComponent, IServiceProvider serviceProvider, IDOM dom, Action? onRender, bool appendToDom = false)
        {
            //var sw = Stopwatch.StartNew();
            //sw.Start();

            // construct component
            var component = (IComponent)(serviceProvider.GetService(virtualComponent.Type) ?? throw new Exception("Could not create component"));

            component.SetServiceProvider(serviceProvider);
            component.SetProps(virtualComponent.Props);
            
            component.OnRender(onRender);
            component.Initialize(dom);
            virtualComponent.Component = component;

            if(appendToDom)
            {
                dom.AppendToDom(component);
            }

            foreach (var child in virtualComponent.Children)
            {
                CreateComponent(child, serviceProvider, dom, onRender);
            }

            component.SetChildren(virtualComponent.Children.Select(x => x.Component ?? throw new Exception("Could not create component")));

            //sw.Stop();
            //Console.WriteLine("Create Component took: {0}ms", sw.ElapsedMilliseconds);
        }

        public static void DestroyComponent(Element component, IDOM dom)
        {
            foreach (var child in component.Children)
            {
                DestroyComponent(child, dom);                
            }           

            component.Component?.Dispose(dom);            
        }
        
    }
}
