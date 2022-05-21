using CSX.Events;
using CSX.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Components
{
    public abstract class DOMComponent<TProps> : BaseComponent<TProps>, IComponent<TProps> where TProps : Props
    {
        bool shoudRerender = true;

        protected Action? OnRenderHandler;
        TProps? _props;
                
        public override TProps Props => _props;

        IComponent[] _oldChildren = new IComponent[0];
        IComponent[] _children = new IComponent[0];
        public override IReadOnlyCollection<IComponent> Children => _children;
        protected IReadOnlyCollection<IComponent> OldChildren => _oldChildren;

        IDOM? _dom;

        ulong _domElement;
        public override ulong DOMElement => _domElement;
                
        public override void SetProps(TProps props)
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
            ReRender();
        }

        public override void SetChildren(IEnumerable<IComponent> children)
        {
            // Only re render when children have changed order or quantity
            var newChildren = children.ToArray();

            if(newChildren.Length != _children.Length)
            {
                shoudRerender = true;
            }
            else
            {
                for(var i = 0; i < newChildren.Length; i++)
                {
                    if(!ReferenceEquals(newChildren[i], _children[i]))
                    {
                        shoudRerender = true;
                        break;
                    }
                }
            }

            _oldChildren = _children;
            _children = newChildren;
        }

        public override void SetProps(object props)
            => SetProps((TProps)props);

        public override void Initialize(IDOM dom)
        {
            _dom = dom;
            _domElement = OnInitialize(dom);            
        }

        public override void OnRender(Action? handler)
        {
            OnRenderHandler = handler;
        }

        public void ReRender()
        {
            RenderView(_dom);
        }

        public void NotifyChange()
        {
            OnRenderHandler?.Invoke();
        }
        
        public override void SetServiceProvider(IServiceProvider serviceProvider) { }
        
        public override void RenderView(IDOM dom)
        {
            Render(dom);

            shoudRerender = false;
        }

        public override void Dispose(IDOM dom)
        {
            base.Dispose(dom);
            OnDestroy(dom);
        }

        /// <summary>
        /// This method is only true when chilren have changed so it can safely be used to detect that
        /// </summary>
        /// <returns></returns>
        public override bool ShouldRender()
        {
            return shoudRerender;
        }

        /// <summary>
        /// This is for most cases rendering children
        /// </summary>
        /// <param name="dom"></param>
        protected void RenderChildren(IDOM dom)
        {
            // Not rendering children if they have not changes in theri order or types
            foreach(var child in Children.Where(x => x.ShouldRender()))
            {
                child.RenderView(_dom);
            }

            dom.SetChildren(DOMElement, Children.Select(x => x.DOMElement).ToArray());            
        }

        protected abstract ulong OnInitialize(IDOM dom);
        protected abstract void OnDestroy(IDOM dom);
        protected abstract void Render(IDOM dom);        
    }
}
