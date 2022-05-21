using CSX.NativeComponents;
using CSX.Rendering;
using Facebook.Yoga;
using System.Reactive.Subjects;

namespace CSX.Native
{
    public abstract class NativeDom<T> : IDOM where T : NativeViewNode<T>
    {
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

            parentNode.YogaNode.Append(childNode.YogaNode);

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

            RootNode.YogaNode.CalculateLayout();

            foreach (var node in Nodes)
            {
                if (node.Value.Id == RootId || !node.Value.YogaNode.HasNewLayout)
                {
                    continue;
                }

                UpdateNodeLayout(node.Value);
            }            
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
            foreach (var child in children.Select(x => Nodes[x]))
            {
                child.Parent = parent;
                parent.Children.Add(child);
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
            var yogaNode = node.YogaNode;
            if (value == null)
            {
                return false;
            }                

            switch (attribute)
            {
                case NativeAttribute.AlignContent:
                    yogaNode.AlignContent = (AlignContent)value switch
                    {
                        AlignContent.Center => YogaAlign.Center,
                        AlignContent.FlexStart => YogaAlign.FlexStart,
                        AlignContent.FlexEnd => YogaAlign.FlexEnd,
                        AlignContent.Stretch => YogaAlign.Stretch,
                        AlignContent.SpaceBetween => YogaAlign.SpaceBetween,
                        AlignContent.SpaceAround => YogaAlign.SpaceAround,
                        _ => throw new NotImplementedException(),
                    };
                    return true;
                    
                case NativeAttribute.AlignItems:
                    yogaNode.AlignItems = (AlignItems)value switch
                    {
                        AlignItems.Center => YogaAlign.Center,
                        AlignItems.FlexStart => YogaAlign.FlexStart,
                        AlignItems.FlexEnd => YogaAlign.FlexEnd,
                        AlignItems.Stretch => YogaAlign.Stretch,
                        AlignItems.Baseline => YogaAlign.Baseline,                        
                        _ => throw new NotImplementedException(),
                    };
                    return true;

                case NativeAttribute.AlignSelf:
                    yogaNode.AlignSelf = (AlignSelf)value switch
                    {
                        AlignSelf.Auto => YogaAlign.Auto,
                        AlignSelf.FlexStart => YogaAlign.FlexStart,
                        AlignSelf.FlexEnd => YogaAlign.FlexEnd,
                        AlignSelf.Center => YogaAlign.Center,
                        AlignSelf.Stretch => YogaAlign.Stretch,
                        AlignSelf.Baseline => YogaAlign.Baseline,
                        _ => throw new NotImplementedException(),
                    };
                    return true;

                case NativeAttribute.AspectRatio:
                    yogaNode.AspectRatio = (float)value;
                    return true;

                case NativeAttribute.Bottom:
                    yogaNode.Bottom = (float)value;
                    return true;
                                    
                case NativeAttribute.Display:
                    yogaNode.Display = (Display)value switch
                    {
                        Display.Flex => YogaDisplay.Flex,
                        Display.None => YogaDisplay.None,
                        _ => throw new NotImplementedException()
                    };
                    return true;

                case NativeAttribute.End:
                    yogaNode.End = (float)value;
                    return true;

                case NativeAttribute.Flex:
                    yogaNode.Flex = (float)value;
                    return true;

                case NativeAttribute.FlexBasis:
                    yogaNode.FlexBasis = (float)value;
                    return true;

                case NativeAttribute.FlexDirection:
                    yogaNode.FlexDirection = (FlexDirection)value switch
                    {
                        FlexDirection.Row => YogaFlexDirection.Row,
                        FlexDirection.RowReverse => YogaFlexDirection.RowReverse,
                        FlexDirection.Column => YogaFlexDirection.Column,
                        FlexDirection.ColumnReverse => YogaFlexDirection.ColumnReverse,
                        _ => throw new NotImplementedException()
                    };
                    return true;

                case NativeAttribute.FlexGrow:
                    yogaNode.FlexGrow = (float)value;
                    return true;

                case NativeAttribute.FlexShrink:
                    yogaNode.FlexShrink = (float)value;
                    return true;

                case NativeAttribute.FlexWrap:
                    yogaNode.Wrap = (FlexWrap)value switch
                    {
                        FlexWrap.NoWrap => YogaWrap.NoWrap,
                        FlexWrap.Wrap => YogaWrap.Wrap,
                        FlexWrap.WrapReverse => YogaWrap.WrapReverse,
                        _ => throw new NotImplementedException()
                    };
                    return true;

                case NativeAttribute.Height:
                    yogaNode.Height = (float)value;
                    return true;

                case NativeAttribute.JustifyContent:
                    yogaNode.JustifyContent = (JustifyContent)value switch
                    {
                        JustifyContent.FlexStart => YogaJustify.FlexStart,
                        JustifyContent.FlexEnd => YogaJustify.FlexEnd,
                        JustifyContent.Center => YogaJustify.Center,
                        JustifyContent.SpaceBetween => YogaJustify.SpaceBetween,
                        JustifyContent.SpaceAround => YogaJustify.SpaceAround,
                        _ => throw new NotImplementedException()
                    };
                    return true;

                case NativeAttribute.Left:
                    yogaNode.Left = (float)value;
                    return true;

                case NativeAttribute.Margin:
                    yogaNode.Margin = (float)value;
                    return true;

                case NativeAttribute.MarginBottom:
                    yogaNode.MarginBottom = (float)value;
                    return true;

                case NativeAttribute.MarginEnd:
                    yogaNode.MarginEnd = (float)value;
                    return true;

                case NativeAttribute.MarginHorizontal:
                    yogaNode.MarginHorizontal = (float)value;
                    return true;

                case NativeAttribute.MarginLeft:
                    yogaNode.MarginLeft = (float)value;
                    return true;

                case NativeAttribute.MarginRight:
                    yogaNode.MarginRight = (float)value;
                    return true;

                case NativeAttribute.MarginStart:
                    yogaNode.MarginStart = (float)value;
                    return true;

                case NativeAttribute.MarginTop:
                    yogaNode.MarginTop = (float)value;
                    return true;

                case NativeAttribute.MarginVertical:
                    yogaNode.MarginVertical = (float)value;
                    return true;

                case NativeAttribute.MaxHeight:
                    yogaNode.MaxHeight = (float)value;
                    return true;

                case NativeAttribute.MaxWidth:
                    yogaNode.MaxWidth = (float)value;
                    return true;

                case NativeAttribute.MinHeight:
                    yogaNode.MinHeight = (float)value;
                    return true;

                case NativeAttribute.MinWidth:
                    yogaNode.MinWidth = (float)value;
                    return true;

                case NativeAttribute.Overflow:
                    yogaNode.Overflow = (Overflow)value switch
                    {
                        Overflow.Visible => YogaOverflow.Visible,
                        Overflow.Hidden => YogaOverflow.Hidden,
                        Overflow.Scroll => YogaOverflow.Scroll,
                        _ => throw new NotImplementedException()
                    };
                    return true;

                case NativeAttribute.Padding:
                    yogaNode.Padding = (float)value;
                    return true;

                case NativeAttribute.PaddingBottom:
                    yogaNode.PaddingBottom = (float)value;
                    return true;

                case NativeAttribute.PaddingEnd:
                    yogaNode.PaddingEnd = (float)value;
                    return true;

                case NativeAttribute.PaddingHorizontal:
                    yogaNode.PaddingHorizontal = (float)value;
                    return true;

                case NativeAttribute.PaddingLeft:
                    yogaNode.PaddingLeft = (float)value;
                    return true;

                case NativeAttribute.PaddingRight:
                    yogaNode.PaddingRight = (float)value;
                    return true;

                case NativeAttribute.PaddingStart:
                    yogaNode.PaddingStart = (float)value;
                    return true;

                case NativeAttribute.PaddingTop:
                    yogaNode.PaddingTop = (float)value;
                    return true;

                case NativeAttribute.PaddingVertical:
                    yogaNode.PaddingVertical = (float)value;
                    return true;

                case NativeAttribute.Position:
                    yogaNode.PositionType = (Position)value switch
                    {
                        Position.Relative => YogaPositionType.Relative,
                        Position.Absolute => YogaPositionType.Absolute,
                        _ => throw new NotImplementedException()
                    };
                    return true;

                case NativeAttribute.Right:
                    yogaNode.Right = (float)value;
                    return true;

                case NativeAttribute.Start:
                    yogaNode.Start = (float)value;
                    return true;

                case NativeAttribute.Top:
                    yogaNode.Top = (float)value;
                    return true;

                case NativeAttribute.Width:
                    yogaNode.Width = (float)value;
                    return true;

                case NativeAttribute.BorderBottomWidth:
                    yogaNode.BorderBottomWidth = (float)value;
                    return true;

                case NativeAttribute.BorderLeftWidth:
                    yogaNode.BorderLeftWidth = (float)value;
                    return true;

                case NativeAttribute.BorderRightWidth:
                    yogaNode.BorderRightWidth = (float)value;
                    return true;

                case NativeAttribute.BorderTopWidth:
                    yogaNode.BorderTopWidth = (float)value;
                    return true;

                case NativeAttribute.BorderWidth:
                    yogaNode.BorderWidth = (float)value;
                    return true;
                    
                default:
                    return false;
            }
        }

    }
}
