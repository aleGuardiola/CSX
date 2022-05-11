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
        public TData[] Data { get; init; } = new TData[0];
        public double RowHeight { get; init; } = 0;
        public Func<TData, Element>? RenderItem { get; init;}
    }
    public record ListViewState
    {
        public double ScrollY { get; init; } = 0;
    }
    public class ListViewT<TData> : Component<ListViewState, ListViewProps<TData>>
    {
        protected override ListViewState OnInitialize()
        {
            return new();
        }

        protected override Element Render()
        {
            var rowHeight = Props.RowHeight;
            var nodePadding = 100;
            var itemCount = Props.Data.Length;
            var viewportHeight = Props.Style?.Height ?? 0;

            var totalContentHeight = itemCount * rowHeight;

            var startNode = (int)Math.Max( Math.Floor(State.ScrollY / rowHeight) - nodePadding, 0);
            var visibleNodesCount = (int)Math.Min(itemCount - startNode, Math.Ceiling(viewportHeight / rowHeight) + (2 * nodePadding));

            var offsetY = startNode * rowHeight;

            var childrenElements = Props.Data.Skip(startNode).Take(visibleNodesCount).Select(x => Props.RenderItem(x)).ToArray();

            return View(new(), new()
            {
                Text(new() { Text = $"ScrollY: {State.ScrollY}" }),
                ScrollView(Props with { OnScroll = (ev) => SetState(State with { ScrollY = ev.Y }) }, new()
                {
                    View(new() { Style = new() { Height = totalContentHeight } }, new()
                    {
                        View(new() { Style = new() { MarginTop = offsetY } }, new()
                        {
                            childrenElements.ToContent()
                        }),                        
                    })
                })
            });
        }
    }
}
