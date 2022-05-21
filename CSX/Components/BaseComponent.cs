using CSX.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Components
{
    public abstract class BaseComponent<TProps> : IComponent<TProps> where TProps : Props
    {
        internal Subject<object?> DisposeSubscriptions = new Subject<object?>();

        public abstract ulong DOMElement { get; }
        public abstract IReadOnlyCollection<IComponent> Children { get; }
        public abstract Props Props { get; }
        TProps IComponent<TProps>.Props => (TProps)Props;
        public abstract void SetProps(TProps props);        
        public abstract void Initialize(IDOM dom);
        public abstract void OnRender(Action? action);
        public abstract void RenderView(IDOM dom);
        public abstract void SetChildren(IEnumerable<IComponent> children);
        public abstract void SetProps(object props);
        public abstract void SetServiceProvider(IServiceProvider serviceProvider);
        public abstract bool ShouldRender();

        public virtual void Destroy(IDOM dom)
        {
            // Dispose all subscriptions
            DisposeSubscriptions.OnNext(null);
            DisposeSubscriptions.OnCompleted();
        }

    }

    public static class ObservableExtensions
    {
        public static IObservable<T> TakeUntilDestroy<T>(this IObservable<T> observable, BaseComponent component)
        {
            return observable.TakeUntil(component.DisposeSubscriptions);
        }
    }
}
