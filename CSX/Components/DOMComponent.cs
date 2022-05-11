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
        bool shoudRerender = false;

        protected Action? OnRenderHandler;
        TProps? _props;
        public TProps Props => _props ?? throw new InvalidOperationException("Component has not been initialized");

        Props IComponent.Props => Props;

        IComponent[] _oldChildren = new IComponent[0];
        IComponent[] _children = new IComponent[0];
        public IReadOnlyCollection<IComponent> Children => _children;
        protected IReadOnlyCollection<IComponent> OldChildren => _oldChildren;

        IDOM? _dom;

        ulong _domElement;
        public ulong DOMElement => _domElement;

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
            ReRender();
        }

        public void SetChildren(IEnumerable<IComponent> children)
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

        public void SetProps(object props)
            => SetProps((TProps)props);

        public void Initialize(IDOM dom)
        {
            _dom = dom;
            _domElement = OnInitialize(dom);
            // first view render
            ReRender();                 
        }

        public void OnRender(Action handler)
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
        
        public void SetServiceProvider(IServiceProvider serviceProvider) { }
        
        public void RenderView(IDOM dom)
        {
            Render(dom);

            foreach (var child in Children)
            {
                if(child.ShouldRender())
                {
                    child.RenderView(dom);
                }                
            }
            
            shoudRerender = false;
        }

        public void Destroy(IDOM dom)
        {
            OnDestroy(dom);
        }

        /// <summary>
        /// This method is only true when chilren have changed so it can safely be used to detect that
        /// </summary>
        /// <returns></returns>
        public bool ShouldRender()
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
            if(!ShouldRender())
            {
                return;
            }

            // Remove old children
            // dom.RemoveAllChildren(DOMElement);
            dom.SetChildren(DOMElement, Children.Select(x => x.DOMElement).ToArray());
            // Append new childs
            //foreach (var child in Children)
            //{                
            //    dom.AppendChildIfNotAppended(DOMElement, child.DOMElement);
            //}
        }

        protected abstract ulong OnInitialize(IDOM dom);
        protected abstract void OnDestroy(IDOM dom);
        protected abstract void Render(IDOM dom);        
    }
}
