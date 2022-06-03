using CSX.Events;
using CSX.Rendering;
using System.Diagnostics;

namespace CSX.Components
{
    public record EmptyState();
    public abstract class Component<TProps> : Component<EmptyState, TProps> where TProps : Props
    {
        protected override EmptyState OnInitialize()
        {
            return new();
        }
    }

    public abstract class Component<TState, TProps> : BaseComponent<TProps> where TState : IEquatable<TState>
                                                                                                 where TProps : Props
    {
        IDOM? _dom;
        Action? _onRenderHandler;
        
        IServiceProvider? _serviceProvider;

        TState? _state;
        public TState State => _state ?? throw new InvalidOperationException("Component has not been initialized");

        TProps? _props;
        public override TProps Props => _props ?? throw new InvalidOperationException("Component has no props");

        IReadOnlyCollection<IComponent> _children = new IComponent[0];
        public override IReadOnlyCollection<IComponent> Children => _children;
        public override ulong DOMElement => RootComponent?.Component?.DOMElement ?? throw new InvalidOperationException("Component has not been initialized");
        bool _componentRendered = false;

        Element? RootComponent;

        public override void SetProps(TProps props)
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
            ReRender();            
        }

        public void RunOnUIThread(Action<double> action, bool forNextFrame = false)
        {
            _dom?.RunOnUIThread(action, forNextFrame);
        }

        public override void SetChildren(IEnumerable<IComponent> children)
        {
            _children = children.ToList().AsReadOnly();
        }

        public override void SetProps(object props)
        {
            SetProps((TProps)props);            
        }            

        public void SetState(TState state)
        {
            if(State.Equals(state))
            {
                return;
            }

            _state = state;
            
            if(_componentRendered)
            {
                ReRender();
            }            

            //NotifyAndRender();
        }
        
        public override void Initialize(IDOM dom)
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
            // ReRender();            
        }

        public override void OnRender(Action? handler)
        {
            _onRenderHandler = handler;
        }

        /// <summary>
        /// Render this component in the DOM
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void ReRender()
        {                     
            RenderView(_dom);           
        }

        /// <summary>
        /// Force the whole app to be re rendered
        /// </summary>
        /// <exception cref="Exception"></exception>
        public void NotifyAndRender()
        {
            var virtualDom = Render();
            RootComponent = ComponentFactory.UpdateTree(RootComponent, virtualDom, _serviceProvider ?? throw new Exception(), _dom ?? throw new Exception(), _onRenderHandler);

            _onRenderHandler?.Invoke();
        }
        public override void SetServiceProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        public override void RenderView(IDOM dom)
        {
            var virtualDom = Render();
            
            // Update Tree
            RootComponent = ComponentFactory.UpdateTree(RootComponent, virtualDom, _serviceProvider ?? throw new Exception(), _dom ?? throw new Exception(), _onRenderHandler);
            // Render Children                       

            RootComponent?.Component?.RenderView(dom);

            if(!_componentRendered)
            {
                OnViewInit();
                _componentRendered = true;
            }

        }
        public override void Dispose(IDOM dom)
        {
            if (RootComponent != null)
            {
                ComponentFactory.DestroyComponent(RootComponent, dom);
            }
            
            OnDestroy();            
        }

        protected virtual void OnDestroy() { }        
        protected virtual TState OnInitialize()
        {
            return Activator.CreateInstance<TState>();
        }

        protected virtual void OnViewInit()
        {

        }

        protected abstract Element Render();

        public override bool ShouldRender()
        {
            return true;
        }
    }
}
