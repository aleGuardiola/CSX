using CSX.Events;
using CSX.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Components
{
    public abstract class DOMComponent<TProps> : EventDispatcherHandler, IComponent<TProps> where TProps : Props
    {
        Action? _onRenderHandler;
        TProps? _props;
        public TProps Props => _props ?? throw new InvalidOperationException("Component has not been initialized");

        Props IComponent.Props => Props;

        IReadOnlyCollection<IComponent> _children = new IComponent[0];
        public IReadOnlyCollection<IComponent> Children => _children;

        Guid _domElement;
        public Guid DOMElement => _domElement;

        public void SetProps(TProps props)
        {
            if (_props == null)
            {
                _props = props;
                return;
            }

            if (_props.Equals(props))
            {
                return;
            }

            _props = props;
            NotifyChange();
        }

        public void SetChildren(IEnumerable<IComponent> children)
        {
            _children = children.ToList().AsReadOnly();
        }

        public void SetProps(object props)
            => SetProps((TProps)props);

        public void Initialize(IDOM dom)
        {
            _domElement = OnInitialize(dom);
            // first view render
            NotifyChange();            
        }

        public void OnRender(Action handler)
        {
            _onRenderHandler = handler;
        }

        public void NotifyChange()
        {
            _onRenderHandler?.Invoke();
        }
        
        public void SetServiceProvider(IServiceProvider serviceProvider) { }
        
        public void RenderView(IDOM dom)
        {            
            foreach (var child in Children)
            {
                child.RenderView(dom);
            }

            Render(dom);
        }

        public void Destroy(IDOM dom)
        {
            OnDestroy(dom);
        }

        protected abstract Guid OnInitialize(IDOM dom);
        protected abstract void OnDestroy(IDOM dom);
        protected abstract void Render(IDOM dom);

    }
}
