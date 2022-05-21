using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Rendering
{
    public class MemoryDom : IDOM
    {
        const ulong RootId = 0;

        public IObservable<Event> Events { get; }
        Func<ulong> NewId;

        Dictionary<ulong, MemoryNode> Nodes = new Dictionary<ulong, MemoryNode>()
        {
            { RootId, new MemoryNode(RootId, NativeElement.Root) }
        };

        public MemoryDom(IObservable<Event> events, Func<ulong> newId)
        {
            Events = events;
            NewId = newId;
        }

        public void AppendChild(ulong parent, ulong child)
        {
            var parentNode = Nodes[parent];
            var childNode = Nodes[child];
            parentNode.Children.Add(childNode);
            childNode.Parent = parentNode;
        }

        public ulong CreateElement(NativeElement element)
        {
            var id = NewId();
            var node = new MemoryNode(id, element);
            Nodes[id] = node;
            return id;
        }

        public void DestroyElement(ulong id)
        {
            Nodes.Remove(id);
        }

        public object? GetAttribute(ulong id, NativeAttribute name)
        {
            var node = Nodes[id];
            if(node.Attributes.TryGetValue(name, out var attr))
            {
                return attr;
            }

            return null;
        }

        public ulong GetRootElement()
        {
            return RootId;
        }

        public bool HasChild(ulong parent, ulong child)
        {            
            var childNode = Nodes[child];
            return childNode.Parent?.Id == parent;
        }

        public void Remove(ulong id)
        {
            var childNode = Nodes[id];
            childNode.Parent?.Children.Remove(childNode);
            childNode.Parent = null;           
        }

        public void SetAttribute(ulong id, NativeAttribute name, object? value)
        {
            var node = Nodes[id];
            if (value == null)
            {
                if(node.Attributes.ContainsKey(name))
                {
                    node.Attributes.Remove(name);
                }                
            }            
            else
            {
                node.Attributes[name] = value;
            }            
        }

        public void SetAttributes(ulong id, KeyValuePair<NativeAttribute, object?>[] attributes)
        {
            foreach(var attr in attributes)
            {
                SetAttribute(id, attr.Key, attr.Value);
            }
        }

        public void SetChildren(ulong id, ulong[] children)
        {
            var parentNode = Nodes[id];

            parentNode.Children.Clear();
            foreach(var child in children.Select(x => Nodes[x]))
            {
                parentNode.Children.Add(child);
                child.Parent = parentNode;
            }
        }
        public ulong[] GetChildren(ulong parent)
        {
            var parentNode = Nodes[parent];
            return parentNode.Children.Select(x => x.Id).ToArray();
        }
        public void SetElementText(ulong id, string text)
        {
            _ = text ?? throw new ArgumentNullException(nameof(text));

            var node = Nodes[id];
            node.Text = text;
        }

        public string GetElementText(ulong id)
        {
            var node = Nodes[id];
            return node.Text;
        }

        public IDOM CreateNewMemoryDom()
        {
            throw new NotImplementedException();
        }
        public void AppendDom(IDOM dom)
        {
            throw new NotImplementedException();
        }
        public bool SupportAppendingDom()
            => false;

    }

}
