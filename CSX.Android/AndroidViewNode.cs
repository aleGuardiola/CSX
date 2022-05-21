using Android.Views;
using CSX.Rendering;
using CSX.Native;

namespace CSX.Android
{
    public class AndroidViewNode : NativeViewNode<AndroidViewNode>
    {
        public AndroidViewNode(ulong id, NativeElement element, View androidView) : base(id, element)
        {
            AndroidView = androidView;
        }

        public View AndroidView { get; }
    }
}
