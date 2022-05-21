using CSX.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Rendering
{
    public enum NativeEvent
    {
        Click = 0,
        MouseOver,
        MouseOut,
        Scroll,
        TextChanged
    }

    public static class NativeEventHelper
    {
        public static Type GetEventArgsType(NativeEvent nativeEvent)
        {
            return nativeEvent switch
            {
                NativeEvent.Click => typeof(CursorEventArgs),
                NativeEvent.MouseOver => typeof(CursorEventArgs),
                NativeEvent.MouseOut => typeof(CursorEventArgs),
                NativeEvent.Scroll => typeof(ScrollEventArgs),
                NativeEvent.TextChanged => typeof(TextChangeEventArgs),
                _ => throw new NotImplementedException()
            };
        }
    }

}
