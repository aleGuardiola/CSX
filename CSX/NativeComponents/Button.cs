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
    public record ButtonStyleProps : ViewStyleProps
    {
        public ButtonStyleProps()
        {
            AlignContent = null;
            FlexDirection = null;
        }
    }
    public record ButtonProps : ViewProps<ButtonStyleProps>
    {
        public string Text { get; init; } = "";
    }
    public class Button : View<ButtonProps, ButtonStyleProps>
    {
        protected override NativeElement Element => NativeElement.Button;

        protected override ulong OnInitialize(IDOM dom)
        {
            return base.OnInitialize(dom);
        }

        protected override void Render(IDOM dom)
        {
            base.Render(dom);
            dom.SetElementText(DOMElement, Props.Text);
        }

        protected override void OnDestroy(IDOM dom)
        {        
            dom.Remove(DOMElement);
            dom.DestroyElement(DOMElement);
        }

    }
}
