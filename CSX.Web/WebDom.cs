using CSX.Events;
using CSX.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CSX.Web
{
    public class WebDom : IDOM
    {
        ulong RootId = 0;
        ulong NextId = 1;

        CsxJsInterop CsxJsInterop = CsxJsInterop.Current;
        Dictionary<NativeElement, Func<ulong>> ElementRenders;

        Dictionary<ulong, HtmlNode> Nodes = new Dictionary<ulong, HtmlNode>()
        {
            { 0, new HtmlNode(0, NativeElement.Root) }
        };

        Subject<Event> _eventPublisher = new Subject<Event>();
        public IObservable<Event> Events => _eventPublisher;

        public WebDom()
        {
            CsxJsInterop.SetEventHandler((e) => {
                _eventPublisher.OnNext(new Event(e.ElementId, e.EventType, e.Payload.Deserialize(NativeEventHelper.GetEventArgsType(e.EventType)) as CSXEventArgs ?? throw new Exception("event payload null from web")));
            });

            // Render for each element
            ElementRenders = new Dictionary<NativeElement, Func<ulong>>()
            {
                { NativeElement.View, () => {
                    var id = GetNewId();

                    CsxJsInterop.CreateElement("div", id);
                    CsxJsInterop.SetElementAttribute(id, "class", "csx-view");

                    return id;
                }},
                { NativeElement.Text, () => {
                    var id = GetNewId();

                    CsxJsInterop.CreateElement("div", id);
                    CsxJsInterop.SetElementAttribute(id, "class", "csx-view");

                    return id;
                }},
                { NativeElement.Button, () => {
                    var id = GetNewId();

                    CsxJsInterop.CreateElement("button", id);
                    CsxJsInterop.SetElementAttribute(id, "type", "button");

                    return id;
                }},
                { NativeElement.Image, () => {
                    var id = GetNewId();

                    CsxJsInterop.CreateElement("img", id);                    

                    return id;
                }},
                { NativeElement.TextInput, () => {
                    var id = GetNewId();

                    CsxJsInterop.CreateElement("input", id);
                    CsxJsInterop.SetElementAttribute(id, "type", "text");

                    return id;
                }},
                { NativeElement.ScrollView, () => {
                    var id = GetNewId();

                    CsxJsInterop.CreateElement("div", id);
                    CsxJsInterop.SetElementAttribute(id, "class", "csx-scroll-view");

                    return id;
                }}
            };
        }

        public ulong CreateElement(NativeElement element)
        {
            var id = ElementRenders[element].Invoke();
            var node = new HtmlNode(id, element);
            Nodes.Add(id, node);
            return id;
        }

        public void AppendChild(ulong parent, ulong child)
        {
            var parentNode = Nodes[parent];
            var childNode = Nodes[child];
           
            childNode.Parent = parentNode;
            parentNode.Children.Add(childNode);

            CsxJsInterop.AttachElement(parent, child);
        }

        public void Remove(ulong id)
        {
            var node = Nodes[id];
            node.Parent?.Children.Remove(node);
            node.Parent = null;
            CsxJsInterop.RemoveElement(id);
        }

        public void DestroyElement(ulong id)
        {            
            Nodes.Remove(id);
            CsxJsInterop.DestroyElement(id);
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
            var node = Nodes[child];
            return node.Parent?.Id == parent;
        }

        public void SetAttribute(ulong id, NativeAttribute name, object? value)
        {            
            var node = Nodes[id];

            UpdateElementHtmlStyle(node, name, value);

            var css = CSSHelper.GetCss(node.HtmlStyle);
            CsxJsInterop.SetElementAttribute(id, "style", css);
        }

        public void SetAttributes(ulong id, KeyValuePair<NativeAttribute, object?>[] attributes)
        {
            var node = Nodes[id];

            foreach (var attr in attributes)
            {
                UpdateElementHtmlStyle(node, attr.Key, attr.Value);
            }

            var css = CSSHelper.GetCss(node.HtmlStyle);
            CsxJsInterop.SetElementAttribute(id, "style", css);
        }

        public void SetChildren(ulong id, ulong[] children)
        {
            var parent = Nodes[id];
            parent.Children.Clear();
            foreach(var child in children.Select(x => Nodes[x]))
            {
                child.Parent = parent;
                parent.Children.Add(child);
            }

            CsxJsInterop.SetChildren(id, string.Join(',', children));
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

            CsxJsInterop.SetElementText(id, text);
        }

        public string GetElementText(ulong id)
        {
            var node = Nodes[id];
            return node.Text;
        }

        static void UpdateElementHtmlStyle(HtmlNode node, NativeAttribute name, object? value)
        {
            var cssName = CSSHelper.GetCssPropertyName(name);

            if (value == null)
            {
                node.Attributes.Remove(name);
                node.HtmlStyle.Remove(cssName);
            }
            else
            {
                node.Attributes[name] = value;
                var cssValue = CSSHelper.GetCssValue(name, value);
                node.HtmlStyle[cssName] = cssValue;
            }
        }

        ulong GetNewId()
        {
            return NextId++;
        }

        public bool SupportAppendingDom()
            => false;
        

        public void AppendDom(IDOM dom)
        {
            throw new NotImplementedException();
        }

        public IDOM CreateNewMemoryDom()
        {
            throw new NotImplementedException();
        }

    }
}
