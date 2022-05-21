using CSX.Events;
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
        Props IComponent.Props => Props;
        public abstract TProps Props { get; }
        public abstract void SetProps(TProps props);        
        public abstract void Initialize(IDOM dom);
        public abstract void OnRender(Action? action);
        public abstract void RenderView(IDOM dom);
        public abstract void SetChildren(IEnumerable<IComponent> children);
        public abstract void SetProps(object props);
        public abstract void SetServiceProvider(IServiceProvider serviceProvider);
        public abstract bool ShouldRender();
        public virtual void Dispose(IDOM dom)
        {
            // Dispose all subscriptions
            DisposeSubscriptions.OnNext(null);
            DisposeSubscriptions.OnCompleted();
        }
    }

    public static class ObservableExtensions
    {
        /// <summary>
        /// Automatically unsubscribe when component gets destroyed
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TProps"></typeparam>
        /// <param name="observable"></param>
        /// <param name="component"></param>
        /// <returns></returns>
        public static IObservable<T> TakeUntilDestroy<T, TProps>(this IObservable<T> observable, BaseComponent<TProps> component) where TProps : Props                                                                                                                                  
        {
            return observable.TakeUntil(component.DisposeSubscriptions);
        }

        internal static void RedirectToCallback<TEventArgs, TProps>(
            this IObservable<Event> observable, 
            BaseComponent<TProps> component,
            NativeEvent eventType,
            Func<TProps, Action<TEventArgs>?> callbackSelector) where TProps : Props 
                                                               where TEventArgs : CSXEventArgs
        {
            observable                
                .Where(x => x.ElementId == component.DOMElement)
                .Where(x => x.EventType == eventType)
                .TakeUntilDestroy(component).Subscribe(ev =>
            {
                var props = (TProps)component.Props;
                var callback = callbackSelector(props);
                callback?.Invoke((TEventArgs)ev.Payload);
            });
        }
    }
}
