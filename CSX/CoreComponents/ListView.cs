using CSX.Components;
using CSX.NativeComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static CSX.ComponentFunctions;

namespace CSX.CoreComponents
{
    public record ListViewProps<TData> : ScrollViewProps
    {
        public double? RowHeight { get; init; }
        public IEnumerable<TData>? Data { get; init; }
        public Func<TData, Element>? RenderItem { get; init; }
    }
    public record ListViewState
    {
        public double PlaceholderHeight { get; init; } = 0;
    }
    public class ListView<TData> : Component<ListViewState, ListViewProps<TData>>
    {        
        protected override Element Render()
        {           

            return ScrollView(Props, new[]
            {               

                // placeholder view
                View(new(){ Style=new(){ Height=State.PlaceholderHeight } })
            });
        }
    }
}
