using CSX.Native;
using CSX.Rendering;
using CSX.Skia.Views;
using CSX.Skia.Views.ScrollBars;
using System.Collections.Concurrent;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace CSX.Skia
{
    public class SkiaDom : IDOM
    {
        ulong _nextId = 1;
        const ulong RootId = 0;
        public BaseView? Root { get; private set; } = null;
        public Dictionary<ulong, BaseView> Views = new Dictionary<ulong, BaseView>();

        Queue<Action<double>> UiThreadActions = new Queue<Action<double>>();
        Queue<Action<double>> UiThreadActionsNextFrame = new Queue<Action<double>>();

        Type ScrollBarViewType = typeof(DefaultScrollBarView);

        public SkiaDom() { }

        Subject<Event> _events = new Subject<Event>();
        public IObservable<Event> Events => _events;//throw new NotImplementedException();

        public void AppendChild(ulong parent, ulong child)
        {
            if(parent == RootId)
            {
                Root = Views[child];
                return;
            }
            var parentView = Views[parent] as View ?? throw new InvalidOperationException("Parent does not support children");
            var childView = Views[child];
            parentView.AppendView(childView);
        }

        public void AppendDom(IDOM dom)
        {
            throw new NotImplementedException();
        }

        public ulong CreateElement(NativeElement element)
        {
            var id = _nextId++;

            View view;

            switch (element)
            {                
                case NativeElement.View:
                    view = new View(id);
                    break;
                case NativeElement.Button:
                    view = new ButtonView(id);
                    break;
                case NativeElement.Image:
                    view = new ImageView(id);
                    break;
                case NativeElement.ScrollView:
                    view = new ScrollView(id, Activator.CreateInstance(ScrollBarViewType, new object[] { id }) as View ?? throw new InvalidOperationException("Cannot create scrollbar view"));
                    break;
                case NativeElement.Text:
                    view = new Text(id);
                    break;
                case NativeElement.TextInput:
                    view = new TextInput(id);
                    break;
                case NativeElement.SkiaCanvas:
                    view = new SkiaCanvas(id);
                    break;
                default:
                    throw new NotImplementedException();
            }

            view.EventFired.Subscribe(@event =>
            {
                _events.OnNext(@event);
            });

            Views[id] = view;

            return id;
        }

        public void RunOnUIThread(Action<double> action, bool forNextFrame = false)
        {
            if(forNextFrame)
            {
                UiThreadActionsNextFrame.Enqueue(action);
                return;
            }

            lock(UiThreadActions)
            {
                UiThreadActions.Enqueue(action);
            }            
        }

        public void OnFrame(double time)
        {
            lock(UiThreadActions)
            {
                while (UiThreadActions.Count > 0)
                {
                    if (UiThreadActions.TryDequeue(out var action))
                    {
                        action(time);
                    }
                }
            }
            
            while (UiThreadActionsNextFrame.Count > 0)
            {
                UiThreadActions.Enqueue(UiThreadActionsNextFrame.Dequeue());
            }
        }

        public IDOM CreateNewMemoryDom()
        {
            throw new NotImplementedException();
        }

        public void DestroyElement(ulong id)
        {
            Views.Remove(id);
        }

        public object? GetAttribute(ulong id, NativeAttribute name)
        {
            object? value;
            Views[id].Attributes.TryGetValue(name, out value);
            return value;
        }

        public ulong[] GetChildren(ulong parent)
        {
            return (Views[parent] as View ?? throw new InvalidOperationException("Parent does not support children")).Children.Select(x => x.Id).ToArray();
        }

        public string GetElementText(ulong id)
        {
            return (Views[id] as IViewWithText ?? throw new InvalidOperationException("View does not handle text")).TextContent;
        }

        public ulong GetRootElement()
        {
            return RootId;
        }

        public bool HasChild(ulong parent, ulong child)
        {
            return (Views[parent] as View ?? throw new InvalidOperationException("Parent does not support children")).Children.Any(x => x.Id == child);
        }

        public void Remove(ulong id)
        {
            var view = Views[id];
            view.Parent?.RemoveWithId(id);
        }

        public void SetAttribute(ulong id, NativeAttribute name, object? value)
        {
            var view = Views[id];
            view.SetAttribute(name, value);
        }

        public void SetAttributes(ulong id, KeyValuePair<NativeAttribute, object?>[] attributes)
        {
            var view = Views[id];
            foreach(var attr in attributes)
            {
                view.SetAttribute(attr.Key, attr.Value);
            }
        }

        public void SetChildren(ulong id, ulong[] children)
        {
            var view = Views[id] as View ?? throw new InvalidOperationException("Parent does not support children");
            view.SetChildren(children.Select(i => Views[i]).ToArray());
        }

        public void SetElementText(ulong id, string text)
        {
            var view = Views[id] as IViewWithText ?? throw new InvalidOperationException("View does not handle text");
            view.TextContent = text;
        }

        public bool SupportAppendingDom()
        {
            return false;
        }
    }
}