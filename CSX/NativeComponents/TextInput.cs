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
    
    public record TextInputProps : TextProps
    {
        public Action<TextChangeEventArgs>? OnTextChange { get; init; }
    }
    public class TextInput : Text<TextInputProps>
    {
        protected override NativeElement Element => NativeElement.TextInput;

        protected override ulong OnInitialize(IDOM dom)
        {
            var elementId = base.OnInitialize(dom);

            dom.Events.RedirectToCallback(this, NativeEvent.TextChanged, (p) => p.OnTextChange);

            return elementId;
        }
    }
}
