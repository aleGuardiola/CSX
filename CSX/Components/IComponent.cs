using CSX.Rendering;

namespace CSX.Components
{
    public interface IComponent
    {
        Guid DOMElement { get; }
        public void Initialize(IDOM dom);
        public void Destroy(IDOM dom);
        public IReadOnlyCollection<IComponent> Children { get; }
        public Props Props { get; }
        public void SetProps(object props);
        public void SetChildren(IEnumerable<IComponent> children);
        public void OnRender(Action? action);
        void RenderView(IDOM dom);
        void SetServiceProvider(IServiceProvider serviceProvider);
    }
    
    public interface IComponent<TProps> : IComponent where TProps : Props
    {             
        new public TProps Props { get; }
        public void SetProps(TProps props); 
    }
}
