using CSX.Rendering;
using Xamarin.Flex;

namespace CSX.Native
{
    public class NativeViewNode<T> where T : NativeViewNode<T>
    {
        public NativeViewNode(ulong id, NativeElement element)
        {
            Id = id;
            Element = element;
            FlexNode = new Item();
        }

        public ulong Id { get; }
        public string Text { get; set; } = "";
        public NativeElement Element { get; }
        public T? Parent { get; set; }
        public Dictionary<NativeAttribute, object> Attributes { get; } = new Dictionary<NativeAttribute, object>();
        public List<T> Children { get; } = new List<T>();
        public Item FlexNode { get; }
    }
}