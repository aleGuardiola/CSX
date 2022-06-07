using CSX.Skia.Rendering.Graphic;
using CSX.Skia.Rendering.Render;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Skia.Rendering.RenderLayer
{
    public class RenderLayerElement
    {
        public RenderNode RenderNode { get; }
        public RenderLayerElement? Parent { get; }
        public RenderLayerElement[] Children { get; }
        public bool IsStackingContext { get; }

        // This properties is for composited layers only to propagate in the painting process
        public float Opacity = 1f;
        public Transform? Transform = null;

        public bool IsRoot => Parent?.Parent == null;

        bool _isComposed = false;
        public bool IsComposed { get => _isComposed; 
            set {
                if(value != _isComposed)
                {
                    IsValid = false;
                }
                _isComposed = value;                
            } }
        public bool IsValid { get; private set; } = false;

        public RenderLayerElement(RenderNode renderNode, RenderLayerElement? parent)
        {
            RenderNode = renderNode;
            Parent = parent;

            // Create a first rendeLayer tree based on the stacking context ( elements with absolute position and with a zIndex that also have children )
            // Order the elements in the stacking context by their drawing order so we have a nice paint order tree
            // 1.Background and borders
            // 2.Negative z-index children
            // 3.Normal flow elements (children that are not absolute)
            // 4. Z-index == 0 and/or absolute positioned children
            // 5. Positive z-index children


            // determine if the element is an stacking context
            if (renderNode.IsRoot || (renderNode.IsAbsolute && renderNode.ZIndex != 0))
            {
                IsComposed = true;
                IsStackingContext = true;
            }

            // order children by draw order

            // Negative z-index children
            var negativeZIndex = renderNode.Children.Where(x => x.ZIndex < 0).OrderBy(x => x.ZIndex);
            // Normal flow elements (children that are not absolute)
            var normalFlow = renderNode.Children.Where(x => !x.IsAbsolute);
            // Z-index == 0 and/or absolute positioned children
            var absolute = renderNode.Children.Where(x => x.IsAbsolute && x.ZIndex == 0).OrderBy(x => x.ZIndex);
            // Positive z-index children
            var positiveZIndex = renderNode.Children.Where(x => x.IsAbsolute && x.ZIndex > 0).OrderBy(x => x.ZIndex);

            var children = negativeZIndex.Concat(normalFlow).Concat(absolute).Concat(positiveZIndex).Select(x => new RenderLayerElement(x, this)).ToArray();

            Children = children;
        }

        /// <summary>
        /// Paint layer
        /// </summary>
        public void Paint(GraphicContext drawingContext)
        {
            if(!IsComposed)
            {
                // Only composed layers can paint
                return;
            }

            // draw everything in paint order
            RenderNode.Paint(drawingContext);

            // paint subtree in the same composited layer
            for(var i = 0; i < Children.Length; i++)
            {
                var child = Children[i];
                PaintNode(drawingContext, child);
            }
        }

        void PaintNode(GraphicContext drawingContext, RenderLayerElement element)
        {
            if (IsComposed)
            {
                // Only paint items inside the composed layer
                return;
            }

            // draw everything in paint order
            RenderNode.Paint(drawingContext);

            // paint subtree in the same composited layer
            for (var i = 0; i < Children.Length; i++)
            {
                var child = Children[i];
                PaintNode(drawingContext, child);
            }
        }

        public void Validate()
        {
            IsValid = true;
        }

    }
}
