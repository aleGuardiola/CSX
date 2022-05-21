using CSX.Components;
using CSX.Events;
using CSX.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CSX.NativeComponents
{    
    public record ScrollViewProps : ViewProps<ViewStyleProps>
    {
        public Action<ScrollEventArgs>? OnScroll { get; init; }
    }
    public class ScrollView : ScrollView<ScrollViewProps> { }
    public class ScrollView<TProps> : View<TProps, ViewStyleProps> where TProps : ScrollViewProps
    {
        protected override NativeElement Element => NativeElement.ScrollView;

        protected override ulong OnInitialize(IDOM dom)
        {
            var id = base.OnInitialize(dom);
            var elementId = dom.CreateElement(Element);

            dom.Events.RedirectToCallback(this, NativeEvent.Scroll, (p) => p.OnScroll);

            return elementId;
        }

    }
}
