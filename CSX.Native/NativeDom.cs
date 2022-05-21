using CSX.NativeComponents;
using CSX.Rendering;
using System.Reactive.Subjects;
using Xamarin.Flex;
using AlignContent = CSX.NativeComponents.AlignContent;
using AlignItems = CSX.NativeComponents.AlignItems;
using AlignSelf = CSX.NativeComponents.AlignSelf;
using Position = CSX.NativeComponents.Position;

namespace CSX.Native
{
    public abstract class NativeDom<T> : IDOM where T : NativeViewNode<T>
    {
        float ViewportWidth = 0;
        float ViewportHeight = 0;

        bool shouldCalculate = false;
        const ulong RootId = 0;
        T RootNode;

        ulong NextId = 1;

        Dictionary<ulong, T> Nodes = new Dictionary<ulong, T>();

        protected Subject<Event> _events = new Subject<Event>();
        public IObservable<Event> Events => _events;

        public NativeDom()
        {
            RootNode = CreateRoot();
            Nodes.Add(RootId, RootNode);
        }

        public ulong CreateElement(NativeElement element)
        {
            var id = GenerateId();

            var node = CreateNode(id, element);
            Nodes.Add(id, node);

            return id;
        }

        public void AppendChild(ulong parent, ulong child)
        {
            var parentNode = Nodes[parent];
            var childNode = Nodes[child];

            parentNode.Children.Add(childNode);
            childNode.Parent = parentNode;
                   

            // If it is the root element set the width as height same as viewport for layout calculations
            if(parent == RootId)
            {
                childNode.FlexNode.Width = ViewportWidth;
                childNode.FlexNode.Height = ViewportHeight;
            }
            else
            {
                parentNode.FlexNode.Add(childNode.FlexNode);
            }

            AppendNode(parentNode, childNode);

            shouldCalculate = true;
        }

        protected void CalculateLayout()
        {
            if(!shouldCalculate)
            {
                return;
            }

            shouldCalculate = false;

            var rootElement = RootNode.Children.FirstOrDefault();
            if(rootElement == null)
            {
                return;
            }

            rootElement.FlexNode.Layout();

            foreach (var node in Nodes)
            {
                if (node.Value.Id == RootId)
                {
                    continue;
                }

                UpdateNodeLayout(node.Value);
            }            
        }

        /// <summary>
        /// This must be called before calling calculate layout
        /// </summary>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void SetViewPortDimmensions(float width, float height)
        {
            ViewportHeight = height;
            ViewportWidth = width;
            var rootElement = Nodes[RootId].Children?.FirstOrDefault();
            if(rootElement == null)
            {
                return;
            }
            rootElement.FlexNode.Width = width;
            rootElement.FlexNode.Height = height;
            shouldCalculate = true;
        }

        public void AppendDom(IDOM dom)
        {
            throw new NotImplementedException();
        }

        public IDOM CreateNewMemoryDom()
        {
            throw new NotImplementedException();
        }

        public void DestroyElement(ulong id)
        {
            var node = Nodes[id];
            Nodes.Remove(id);
            DestroyNode(node);
        }

        public object? GetAttribute(ulong id, NativeAttribute name)
        {
            var node = Nodes[id];
            if(node.Attributes.TryGetValue(name, out var value))
            {
                return value;
            }

            return null;
        }

        public ulong[] GetChildren(ulong parent)
        {
            return Nodes[parent].Children.Select(x => x.Id).ToArray();
        }

        public string GetElementText(ulong id)
        {
            return Nodes[id].Text;
        }

        public ulong GetRootElement()
        {
            return RootId;
        }

        public bool HasChild(ulong parent, ulong child)
        {
            var childNode = Nodes[child];
            return childNode.Parent?.Id == parent;
        }

        public void Remove(ulong id)
        {
            var childNode = Nodes[id];
            childNode.Parent?.Children.Remove(childNode);
            childNode.Parent = null;
            RemoveNode(childNode);
            shouldCalculate = true;
        }

        public void SetAttribute(ulong id, NativeAttribute name, object? value)
        {
            var node = Nodes[id];
            if (TrySetLayoutAttribute(node, name, value))
            {
                shouldCalculate = true;
            }
            else
            {
                SetNodeStyleAttribute(node, name, value);
            }
        }

        public void SetAttributes(ulong id, KeyValuePair<NativeAttribute, object?>[] attributes)
        {
            foreach(var attr in attributes)
            {
                SetAttribute(id, attr.Key, attr.Value);
            }            
        }

        public void SetChildren(ulong id, ulong[] children)
        {
            var parent = Nodes[id];
            parent.Children.Clear();
            
            for(uint i = 0; i < parent.FlexNode.Count; i++)
            {
                parent.FlexNode.RemoveAt(i);
            }

            foreach (var child in children.Select(x => Nodes[x]))
            {
                child.Parent = parent;
                parent.Children.Add(child);
                parent.FlexNode.Add(child.FlexNode);
            }

            SetNodeChildren(parent, parent.Children.ToArray());

            shouldCalculate = true;
        }

        public void SetElementText(ulong id, string text)
        {
            var node = Nodes[id];
            node.Text = text;
            SetNodeText(node, text);
        }

        public bool SupportAppendingDom()
        {
            return false;
        }

        ulong GenerateId()
            => NextId++;

        protected abstract T CreateRoot();

        protected abstract T CreateNode(ulong id, NativeElement element);

        protected abstract void AppendNode(T parent, T child);
        protected abstract void UpdateNodeLayout(T element);
        protected abstract void DestroyNode(T node);
        protected abstract void RemoveNode(T node);
        protected abstract void SetNodeStyleAttribute(T node, NativeAttribute attribute, object? value);
        protected abstract void SetNodeChildren(T parent, T[] children);
        protected abstract void SetNodeText(T node, string text);

        bool TrySetLayoutAttribute(T node, NativeAttribute attribute, object? value)
        {
            var yogaNode = node.FlexNode;
            if (value == null)
            {
                return false;
            }                

            switch (attribute)
            {
                case NativeAttribute.AlignContent:
                    yogaNode.AlignContent = (AlignContent)value switch
                    {
                        AlignContent.Center => Xamarin.Flex.AlignContent.Center,
                        AlignContent.FlexStart => Xamarin.Flex.AlignContent.Start,
                        AlignContent.FlexEnd => Xamarin.Flex.AlignContent.End,
                        AlignContent.Stretch => Xamarin.Flex.AlignContent.Stretch,
                        AlignContent.SpaceBetween => Xamarin.Flex.AlignContent.SpaceBetween,
                        AlignContent.SpaceAround => Xamarin.Flex.AlignContent.SpaceAround,
                        _ => throw new NotImplementedException(),
                    };
                    return true;
                    
                case NativeAttribute.AlignItems:
                    yogaNode.AlignItems = (AlignItems)value switch
                    {
                        AlignItems.Center => Xamarin.Flex.AlignItems.Center,
                        AlignItems.FlexStart => Xamarin.Flex.AlignItems.Start,
                        AlignItems.FlexEnd => Xamarin.Flex.AlignItems.End,
                        AlignItems.Stretch => Xamarin.Flex.AlignItems.Stretch,
                        // AlignItems.Baseline => Align.Baseline,                        
                        _ => throw new NotImplementedException(),
                    };
                    return true;

                case NativeAttribute.AlignSelf:
                    yogaNode.AlignSelf = (AlignSelf)value switch
                    {
                        AlignSelf.Auto => Xamarin.Flex.AlignSelf.Auto,
                        AlignSelf.FlexStart => Xamarin.Flex.AlignSelf.Start,
                        AlignSelf.FlexEnd => Xamarin.Flex.AlignSelf.End,
                        AlignSelf.Center => Xamarin.Flex.AlignSelf.Center,
                        AlignSelf.Stretch => Xamarin.Flex.AlignSelf.Stretch,
                        //AlignSelf.Baseline => Align.Baseline,
                        _ => throw new NotImplementedException(),
                    };
                    return true;

                //case NativeAttribute.AspectRatio:
                //    yogaNode.AspectRatio = (float)value;
                //    return true;

                case NativeAttribute.Bottom:
                    yogaNode.Bottom = (float)value;
                    return true;
                                    
                //case NativeAttribute.Display:
                //    yogaNode.Display = (Display)value switch
                //    {
                //        Display.Flex => YogaDisplay.Flex,
                //        Display.None => YogaDisplay.None,
                //        _ => throw new NotImplementedException()
                //    };
                //    return true;

                //case NativeAttribute.End:
                //    yogaNode.End = (float)value;
                //    return true;

                case NativeAttribute.Flex:
                    yogaNode.Grow = (float)value;
                    return true;

                case NativeAttribute.FlexBasis:
                    yogaNode.Basis = new Basis((float)value);
                    return true;

                case NativeAttribute.FlexDirection:
                    yogaNode.Direction = (FlexDirection)value switch
                    {
                        FlexDirection.Row => Xamarin.Flex.Direction.Row,
                        FlexDirection.RowReverse => Xamarin.Flex.Direction.RowReverse,
                        FlexDirection.Column => Xamarin.Flex.Direction.Column,
                        FlexDirection.ColumnReverse => Xamarin.Flex.Direction.ColumnReverse,
                        _ => throw new NotImplementedException()
                    };
                    return true;

                case NativeAttribute.FlexGrow:
                    yogaNode.Grow = (float)value;
                    return true;

                case NativeAttribute.FlexShrink:
                    yogaNode.Shrink = (float)value;
                    return true;

                case NativeAttribute.FlexWrap:
                    yogaNode.Wrap = (FlexWrap)value switch
                    {
                        FlexWrap.NoWrap => Wrap.NoWrap,
                        FlexWrap.Wrap => Wrap.Wrap,
                        FlexWrap.WrapReverse => Wrap.WrapReverse,
                        _ => throw new NotImplementedException()
                    };
                    return true;

                case NativeAttribute.Height:
                    yogaNode.Height = (float)value;
                    return true;

                case NativeAttribute.JustifyContent:
                    yogaNode.JustifyContent = (JustifyContent)value switch
                    {
                        JustifyContent.FlexStart => Justify.Start,
                        JustifyContent.FlexEnd => Justify.End,
                        JustifyContent.Center => Justify.Center,
                        JustifyContent.SpaceBetween => Justify.SpaceBetween,
                        JustifyContent.SpaceAround => Justify.SpaceAround,
                        _ => throw new NotImplementedException()
                    };
                    return true;

                case NativeAttribute.Left:
                    yogaNode.Left = (float)value;
                    return true;

                //case NativeAttribute.Margin:
                //    yogaNode.Margin = (float)value;
                //    return true;

                case NativeAttribute.MarginBottom:
                    yogaNode.MarginBottom = (float)value;
                    return true;

                //case NativeAttribute.MarginEnd:
                //    yogaNode.MarginEnd = (float)value;
                //    return true;

                //case NativeAttribute.MarginHorizontal:
                //    yogaNode.MarginHorizontal = (float)value;
                //    return true;

                case NativeAttribute.MarginLeft:
                    yogaNode.MarginLeft = (float)value;
                    return true;

                case NativeAttribute.MarginRight:
                    yogaNode.MarginRight = (float)value;
                    return true;

                //case NativeAttribute.MarginStart:
                //    yogaNode.MarginStart = (float)value;
                //    return true;

                case NativeAttribute.MarginTop:
                    yogaNode.MarginTop = (float)value;
                    return true;

                //case NativeAttribute.MarginVertical:
                //    yogaNode.MarginVertical = (float)value;
                //    return true;

                //case NativeAttribute.MaxHeight:
                //    yogaNode.MaxHeight = (float)value;
                //    return true;

                //case NativeAttribute.MaxWidth:
                //    yogaNode.MaxWidth = (float)value;
                //    return true;

                //case NativeAttribute.MinHeight:
                //    yogaNode.MinHeight = (float)value;
                //    return true;

                //case NativeAttribute.MinWidth:
                //    yogaNode.MinWidth = (float)value;
                //    return true;

                //case NativeAttribute.Overflow:
                //    yogaNode.Overflow = (Overflow)value switch
                //    {
                //        Overflow.Visible => YogaOverflow.Visible,
                //        Overflow.Hidden => YogaOverflow.Hidden,
                //        Overflow.Scroll => YogaOverflow.Scroll,
                //        _ => throw new NotImplementedException()
                //    };
                //    return true;

                //case NativeAttribute.Padding:
                //    yogaNode.Padding = (float)value;
                //    return true;

                case NativeAttribute.PaddingBottom:
                    yogaNode.PaddingBottom = (float)value;
                    return true;

                //case NativeAttribute.PaddingEnd:
                //    yogaNode.PaddingEnd = (float)value;
                //    return true;

                //case NativeAttribute.PaddingHorizontal:
                //    yogaNode.PaddingHorizontal = (float)value;
                //    return true;

                case NativeAttribute.PaddingLeft:
                    yogaNode.PaddingLeft = (float)value;
                    return true;

                case NativeAttribute.PaddingRight:
                    yogaNode.PaddingRight = (float)value;
                    return true;

                //case NativeAttribute.PaddingStart:
                //    yogaNode.PaddingStart = (float)value;
                //    return true;

                case NativeAttribute.PaddingTop:
                    yogaNode.PaddingTop = (float)value;
                    return true;

                //case NativeAttribute.PaddingVertical:
                //    yogaNode.PaddingVertical = (float)value;
                //    return true;

                case NativeAttribute.Position:
                    yogaNode.Position = (Position)value switch
                    {
                        Position.Relative => Xamarin.Flex.Position.Relative,
                        Position.Absolute => Xamarin.Flex.Position.Absolute,
                        _ => throw new NotImplementedException()
                    };
                    return true;

                case NativeAttribute.Right:
                    yogaNode.Right = (float)value;
                    return true;

                //case NativeAttribute.Start:
                //    yogaNode.Start = (float)value;
                //    return true;

                case NativeAttribute.Top:
                    yogaNode.Top = (float)value;
                    return true;

                case NativeAttribute.Width:
                    yogaNode.Width = (float)value;
                    return true;

                //case NativeAttribute.BorderBottomWidth:
                //    yogaNode.BorderBottomWidth = (float)value;
                //    return true;

                //case NativeAttribute.BorderLeftWidth:
                //    yogaNode.BorderLeftWidth = (float)value;
                //    return true;

                //case NativeAttribute.BorderRightWidth:
                //    yogaNode.BorderRightWidth = (float)value;
                //    return true;

                //case NativeAttribute.BorderTopWidth:
                //    yogaNode.BorderTopWidth = (float)value;
                //    return true;

                //case NativeAttribute.BorderWidth:
                //    yogaNode.BorderWidth = (float)value;
                //    return true;
                    
                default:
                    return false;
            }
        }

    }
}
