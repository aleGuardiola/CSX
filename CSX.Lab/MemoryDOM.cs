using CSX.Components;
using CSX.NativeComponents;
using CSX.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Lab
{
    public class ElementA
    {
        public ElementA? Parent { get; set; }
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public Dictionary<string, string?> Attributes { get; } = new Dictionary<string, string?>();
        public List<ElementA> Children { get; } = new List<ElementA>();
    }

    public class MemoryDOM : IDOM
    {
        static ElementA _root = new ElementA() { Id = Guid.NewGuid(), Name = "Root" };

        ElementA Root = _root;

        Dictionary<Guid, ElementA> elements = new Dictionary<Guid, ElementA>() { { _root.Id, _root } };

        public void AppendChild(Guid parent, Guid child)
        {
            var p = elements[parent];
            var c = elements[child];

            p.Children.Add(c);
            c.Parent = p;
        }

        public Guid CreateElement(string name)
        {
            var element = new ElementA() { Id = Guid.NewGuid(), Name = name };
            elements.Add(element.Id, element);
            return element.Id;
        }

        public void DestroyElement(Guid id)
        {
            elements.Remove(id);
        }

        public string? GetAttribute(Guid id, string name)
        {
            return elements[id].Attributes.GetValueOrDefault(name);
        }

        public Guid GetRootElement()
        {
            return Root.Id;
        }

        public bool HasChild(Guid parent, Guid child)
        {
            return elements[parent].Children.Contains(elements[child]);
        }

        public void Remove(Guid id)
        {
            elements[id]?.Parent?.Children.Remove(elements[id]);
        }

        public void SetAttribute(Guid id, string name, string? value)
        {
            elements[id].Attributes[name] = value;
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
