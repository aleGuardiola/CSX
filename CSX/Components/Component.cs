using CSX.Events;
using CSX.Rendering;

namespace CSX.Components
{
    public abstract class Component<TState, TProps> : EventDispatcherHandler, IComponent<TProps> where TState : IEquatable<TState>
                                                                                                 where TProps : Props
    {
        IDOM? _dom;
        Action? _onRenderHandler;
        
        IServiceProvider? _serviceProvider;

        TState? _state;
        protected TState State => _state ?? throw new InvalidOperationException("Component has not been initialized");

        TProps? _props;
        public TProps Props => _props ?? throw new InvalidOperationException("Component has not been initialized");

        Props IComponent.Props => Props;

        IReadOnlyCollection<IComponent> _children = new IComponent[0];
        public IReadOnlyCollection<IComponent> Children => _children;
        public Guid DOMElement => RootComponent?.Component?.DOMElement ?? throw new InvalidOperationException("Component has not been initialized");

        VirtualComponent? RootComponent;

        public void SetProps(TProps props)
        {            
            if(_props == null)
            {
                _props = props;
                return;
            }

            if (_props.Equals(props))
            {
                return;
            }

            _props = props;
            NotifyAndRender();
        }

        public void SetChildren(IEnumerable<IComponent> children)
        {
            _children = children.ToList().AsReadOnly();
        }

        public void SetProps(object props)
            => SetProps((TProps)props);

        public void SetState(TState state)
        {
            if(State.Equals(state))
            {
                return;
            }

            _state = state;
            NotifyAndRender();
        }
        
        public void Initialize(IDOM dom)
        {
            _dom = dom;
            
            if (_props == null)
            {
                throw new InvalidOperationException("Cannot initialize component without props");
            }

            _state = OnInitialize();

            if (_state == null)
            {
                throw new InvalidOperationException("Component state cannot be initialized without state");
            }

            // first view render
            NotifyAndRender();
        }

        public void OnRender(Action handler)
        {
            _onRenderHandler = handler;
        }

        public void NotifyAndRender()
        {
            var virtualDom = Render();
            RootComponent = ComponentFactory.UpdateTree(RootComponent, virtualDom, _serviceProvider ?? throw new Exception(), _dom ?? throw new Exception());

            _onRenderHandler?.Invoke();
        }
        public void SetServiceProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public void RenderView(IDOM dom)
        {
            RootComponent?.Component?.RenderView(dom);
        }
        public void Destroy(IDOM dom)
        {
            if (RootComponent != null)
            {
                ComponentFactory.DestroyComponent(RootComponent, dom);
            }
            
            OnDestroy();            
        }

        protected virtual void OnDestroy() { }        
        protected abstract TState OnInitialize();
        protected virtual VirtualComponent Render() { throw new NotImplementedException(); }
    }
}
