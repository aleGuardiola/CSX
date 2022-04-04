using CSX.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Lab
{
    public class Element
    {
        public Element? Parent { get; set; }
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public Dictionary<string, string?> Attributes { get; } = new Dictionary<string, string?>();
        public List<Element> Children { get; } = new List<Element>();
    }

    public class MemoryDOM : IDOM
    {
        static Element _root = new Element() { Id = Guid.NewGuid(), Name = "Root" };

        Element Root = _root;

        Dictionary<Guid, Element> elements = new Dictionary<Guid, Element>() { { _root.Id, _root } };

        public void AppendChild(Guid parent, Guid child)
        {
            var p = elements[parent];
            var c = elements[child];

            p.Children.Add(c);
            c.Parent = p;
        }

        public Guid CreateElement(string name)
        {
            var element = new Element() { Id = Guid.NewGuid(), Name = name };
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
}
