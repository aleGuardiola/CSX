using CSX.NativeComponents;
using CSX.Rendering;
using Facebook.Yoga;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Skia.Views
{
    public abstract class BaseView
    {
        public readonly ulong Id;
        public bool ShouldCalculateLayout { get; private set; }
        public YogaNode YogaNode { get; private set; } = new YogaNode();

        public Dictionary<NativeAttribute, object> Attributes { get; } = new Dictionary<NativeAttribute, object>();

        public BaseView(ulong id)
        {
            Id = id;
        }

        public virtual void SetAttribute(NativeAttribute attribute, object? value)
        {
            if(value == null)
            { 
                return; 
            }

            Attributes[attribute] = value;

            switch (attribute)
            {
                case NativeAttribute.AlignContent:
                    YogaNode.AlignContent = (AlignContent)value switch
                    {
                        AlignContent.Center => YogaAlign.Center,
                        AlignContent.FlexStart => YogaAlign.FlexStart,
                        AlignContent.FlexEnd => YogaAlign.FlexEnd,
                        AlignContent.Stretch => YogaAlign.Stretch,
                        AlignContent.SpaceBetween => YogaAlign.SpaceBetween,
                        AlignContent.SpaceAround => YogaAlign.SpaceAround,
                        _ => throw new NotImplementedException(),
                    };
                    break;

                case NativeAttribute.AlignItems:
                    YogaNode.AlignItems = (AlignItems)value switch
                    {
                        AlignItems.Center => YogaAlign.Center,
                        AlignItems.FlexStart => YogaAlign.FlexStart,
                        AlignItems.FlexEnd => YogaAlign.FlexEnd,
                        AlignItems.Stretch => YogaAlign.Stretch,
                        AlignItems.Baseline => YogaAlign.Baseline,                        
                        _ => throw new NotImplementedException(),
                    };
                    break;

                case NativeAttribute.AlignSelf:
                    YogaNode.AlignSelf = (AlignSelf)value switch
                    {
                        AlignSelf.Auto => YogaAlign.Auto,
                        AlignSelf.FlexStart => YogaAlign.FlexStart,
                        AlignSelf.FlexEnd => YogaAlign.FlexEnd,
                        AlignSelf.Center => YogaAlign.Center,
                        AlignSelf.Stretch => YogaAlign.Stretch,
                        AlignSelf.Baseline => YogaAlign.Baseline,
                        _ => throw new NotImplementedException(),
                    };
                    break;

                case NativeAttribute.AspectRatio:
                    //YogaNode.AspectRatio = (float)value;
                    break;

                case NativeAttribute.Bottom:
                    YogaNode.Bottom = (float)value;
                    break;

                case NativeAttribute.Display:
                    YogaNode.Display = (Display)value switch
                    {
                        Display.Flex => YogaDisplay.Flex,
                        Display.None => YogaDisplay.None,
                        _ => throw new NotImplementedException()
                    };
                    break;

                case NativeAttribute.End:
                    YogaNode.End = (float)value;
                    break;

                case NativeAttribute.Flex:
                    YogaNode.Flex = (float)value;
                    break;

                case NativeAttribute.FlexBasis:
                    YogaNode.FlexBasis = (float)value;
                    break;

                case NativeAttribute.FlexDirection:
                    YogaNode.FlexDirection = (FlexDirection)value switch
                    {
                        FlexDirection.Row => YogaFlexDirection.Row,
                        FlexDirection.RowReverse => YogaFlexDirection.RowReverse,
                        FlexDirection.Column => YogaFlexDirection.Column,
                        FlexDirection.ColumnReverse => YogaFlexDirection.ColumnReverse,
                        _ => throw new NotImplementedException()
                    };
                    break;

                case NativeAttribute.FlexGrow:
                    YogaNode.FlexGrow = (float)value;
                    break;

                case NativeAttribute.FlexShrink:
                    YogaNode.FlexShrink = (float)value;
                    break;

                case NativeAttribute.FlexWrap:
                    YogaNode.Wrap = (FlexWrap)value switch
                    {
                        FlexWrap.NoWrap => YogaWrap.NoWrap,
                        FlexWrap.Wrap => YogaWrap.Wrap,
                        FlexWrap.WrapReverse => YogaWrap.WrapReverse,                        
                        _ => throw new NotImplementedException()
                    };
                    break;

                case NativeAttribute.Height:
                    YogaNode.Height = (float)value;
                    break;

                case NativeAttribute.JustifyContent:
                    YogaNode.JustifyContent = (JustifyContent)value switch
                    {
                        JustifyContent.FlexStart => YogaJustify.FlexStart,
                        JustifyContent.FlexEnd => YogaJustify.FlexEnd,
                        JustifyContent.Center => YogaJustify.Center,
                        JustifyContent.SpaceBetween => YogaJustify.SpaceBetween,
                        JustifyContent.SpaceAround => YogaJustify.SpaceAround,
                        _ => throw new NotImplementedException()
                    };
                    break;

                case NativeAttribute.Left:
                    YogaNode.Left = (float)value;
                    break;

                case NativeAttribute.Margin:
                    YogaNode.Margin = (float)value;
                    break;

                case NativeAttribute.MarginBottom:
                    YogaNode.MarginBottom = (float)value;
                    break;

                case NativeAttribute.MarginEnd:
                    YogaNode.MarginEnd = (float)value;
                    break;

                case NativeAttribute.MarginHorizontal:
                    YogaNode.MarginHorizontal = (float)value;
                    break;

                case NativeAttribute.MarginLeft:
                    YogaNode.MarginLeft = (float)value;
                    break;

                case NativeAttribute.MarginRight:
                    YogaNode.MarginRight = (float)value;
                    break;

                case NativeAttribute.MarginStart:
                    YogaNode.MarginStart = (float)value;
                    break;

                case NativeAttribute.MarginTop:
                    YogaNode.MarginTop = (float)value;
                    break;

                case NativeAttribute.MarginVertical:
                    YogaNode.MarginVertical = (float)value;
                    break;

                case NativeAttribute.MaxHeight:
                    YogaNode.MaxHeight = (float)value;
                    break;

                case NativeAttribute.MaxWidth:
                    YogaNode.MaxWidth = (float)value;
                    break;

                case NativeAttribute.MinHeight:
                    YogaNode.MinHeight = (float)value;
                    break;

                case NativeAttribute.MinWidth:
                    YogaNode.MinWidth = (float)value;
                    break;

                case NativeAttribute.Overflow:
                    YogaNode.Overflow = (Overflow)value switch
                    {
                        Overflow.Visible => YogaOverflow.Visible,
                        Overflow.Hidden => YogaOverflow.Hidden,
                        Overflow.Scroll => YogaOverflow.Scroll,
                        _ => throw new NotImplementedException()
                    };
                    break;

                case NativeAttribute.Padding:
                    YogaNode.Padding = (float)value;
                    break;

                case NativeAttribute.PaddingBottom:
                    YogaNode.PaddingBottom = (float)value;
                    break;

                case NativeAttribute.PaddingEnd:
                    YogaNode.PaddingEnd = (float)value;
                    break;

                case NativeAttribute.PaddingHorizontal:
                    YogaNode.PaddingHorizontal = (float)value;
                    break;

                case NativeAttribute.PaddingLeft:
                    YogaNode.PaddingLeft = (float)value;
                    break;

                case NativeAttribute.PaddingRight:
                    YogaNode.PaddingRight = (float)value;
                    break;

                case NativeAttribute.PaddingStart:
                    YogaNode.PaddingStart = (float)value;
                    break;

                case NativeAttribute.PaddingTop:
                    YogaNode.PaddingTop = (float)value;
                    break;

                case NativeAttribute.PaddingVertical:
                    YogaNode.PaddingVertical = (float)value;
                    break;

                case NativeAttribute.Position:
                    YogaNode.PositionType = (Position)value switch
                    {
                        Position.Relative => YogaPositionType.Relative,
                        Position.Absolute => YogaPositionType.Absolute,
                        _ => throw new NotImplementedException()
                    };
                    break;

                case NativeAttribute.Right:
                    YogaNode.Right = (float)value;
                    break;

                case NativeAttribute.Start:
                    YogaNode.Start = (float)value;
                    break;

                case NativeAttribute.Top:
                    YogaNode.Top = (float)value;
                    break;

                case NativeAttribute.Width:
                    YogaNode.Width = (float)value;
                    break;

                case NativeAttribute.BorderBottomWidth:
                    YogaNode.BorderBottomWidth = (float)value;
                    break;

                case NativeAttribute.BorderLeftWidth:
                    YogaNode.BorderLeftWidth = (float)value;
                    break;

                case NativeAttribute.BorderRightWidth:
                    YogaNode.BorderRightWidth = (float)value;
                    break;

                case NativeAttribute.BorderTopWidth:
                    YogaNode.BorderTopWidth = (float)value;
                    break;

                case NativeAttribute.BorderWidth:
                    YogaNode.BorderWidth = (float)value;
                    break;
            }

            ShouldCalculateLayout = true;

        }


        public virtual void Mesure()
        {
            // Used for things like text
        }

        public abstract void CalculateLayout();
        

        public abstract void Draw(SKCanvas canvas);
    }
}
