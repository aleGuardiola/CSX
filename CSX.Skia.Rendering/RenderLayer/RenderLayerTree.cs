using CSX.Skia.Rendering.Render;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Skia.Rendering.RenderLayer
{
    public static class RenderLayerTree
    {
        public static RenderLayerElement Create(RenderNode renderNode)
        {
            // Create a first rendeLayer tree based on the stacking context ( elements with absolute position and with a zIndex that also have children )
            // Order the elements in the stacking context by their drawing order so we have a nice paint order tree
            // 1.Background and borders
            // 2.Negative z-index children
            // 3.Normal flow elements (children that are not absolute)
            // 4. Z-index == 0 and/or absolute positioned children
            // 5. Positive z-index children

            var drawOrderTree = new RenderLayerElement(renderNode, null);

            Composite(drawOrderTree);

            return drawOrderTree;
        }

        static void Composite(RenderLayerElement renderLayerElement)
        {
            renderLayerElement.Validate();

            if(LayerOverlaps(renderLayerElement))
            {
                renderLayerElement.IsComposed = true;
            }
            else if(DoesLayerNeedsComposing(renderLayerElement))
            {
                renderLayerElement.IsComposed = true;
            }
            else if(renderLayerElement.IsStackingContext)
            {
                renderLayerElement.IsComposed = true;
            }
            else
            {
                renderLayerElement.IsComposed = false;
            }

            // check children in the subtree
            for(var i = 0; i < renderLayerElement.Children.Length; i++)
            {
                Composite(renderLayerElement.Children[i]);
            }

            // determine if children status changed in the subtree
            if(!IsSubtreeValid(renderLayerElement))
            {
                // if any children have change their status re calculate
                Composite(renderLayerElement);
            }
        }

        static bool DoesLayerNeedsComposing(RenderLayerElement renderLayerElement)
        {
            // Reasons that benefit to create a composite layer
            // opacity, transform, filter, reflection TODO
            if (renderLayerElement.RenderNode.Opacity < 1f)
            {
                return true;
            }
            if (renderLayerElement.Transform != null)
            {
                return true;
            }

            // scrolling
            if(renderLayerElement.RenderNode.IsScrolling)
            {
                return true;
            }

            // content rendered separately, SkiaCanvas
            if(renderLayerElement.RenderNode.RenderElements.Any(x => x is SKiaCanvasRenderElement))
            {
                return true;
            }

            // Reasons that is necessary to create a composite layer
            // composite descendats may need composite parents TODO

            // composited negative z-index child rquire parent to be composited too
            if(renderLayerElement.Children.Any(x => x.IsComposed && x.RenderNode.ZIndex < 0))
            {
                return true;
            }

            return false;
        }

        static bool IsSubtreeValid(RenderLayerElement renderLayerElement)
        {
            for(var i = 0; i < renderLayerElement.Children.Length; i++)
            {
                var child = renderLayerElement.Children[i];
                if(!child.IsValid)
                {
                    return false;
                }
                if(!IsSubtreeValid(child))
                {
                    return false;
                }
            }

            return true;
        }

        static bool LayerOverlaps(RenderLayerElement renderLayerElement)
        {
            var stackingContext = GetStackingContext(renderLayerElement);
            return LayerOverlaps(stackingContext, renderLayerElement);
        }

        static bool LayerOverlaps(RenderLayerElement tree, RenderLayerElement layerElement)
        {
            // If something my be animated behind it, assume it overlaps and skip the computation
            // Otherwise check with the bounding boxes of previus composited content, dont need to check outside of stacking context

            if(ReferenceEquals(tree, layerElement))
            {
                return false;
            }

            // check overlap only in composed layers
            if(tree.IsComposed)
            {
                if(tree.RenderNode.Rect.IntersectsWith(layerElement.RenderNode.Rect))
                {
                    return true;
                }
            }

            // if something my be animating behind
            if(tree.Transform != null)
            {
                return true;
            }

            for(var i = 0; i < tree.Children.Length; i++)
            {
                if(LayerOverlaps(tree.Children[i], layerElement))
                {
                    return true;
                }
            }

            return false;
        }

        static RenderLayerElement GetStackingContext(RenderLayerElement layerElement)
        {
            if(layerElement.IsStackingContext)
            {
                return layerElement;
            }

            if(layerElement.Parent == null)
            {
                return layerElement;
            }

            return GetStackingContext(layerElement.Parent);
        }
        

    }
}
