using CSX.Events;
using CSX.NativeComponents;
using CSX.Rendering;
using CSX.Skia.Events;
using Facebook.Yoga;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Skia.Views
{
    public abstract class BaseView
    {
        public Subject<Event> EventFired = new Subject<Event>();

        public uint Deep { get; protected set; } = 1;

        public readonly ulong Id;
        public bool IsDirty { get; protected set; } = true;
        public YogaNode YogaNode { get; private set; } = new YogaNode();

        public Dictionary<NativeAttribute, object?> Attributes { get; } = new Dictionary<NativeAttribute, object>();

        public DrawContext DrawContext { get; protected set; }

        public View? Parent { get; internal set; }

        public BaseView(ulong id)
        {
            Id = id;                  
        }

        public void MarkAsSeen()
        {
            IsDirty = false;
            // YogaNode.MarkLayoutSeen();
        }

        public void SetDeep(uint deep)
        {
            var withParent = (deep + 1);
            if (withParent > Deep)
            {
                Deep = withParent;
            }
            Parent?.SetDeep(withParent);
        }

        public float TranslatedY { get; protected set; }
        public float TranslatedX { get; protected set; }

        public float RelativeX { get; protected set; }
        public float RelativeY { get; protected set; }

        public float AbsoulteX { get; protected set; }
        public float AbsoluteY { get; protected set; }

        public float LastDrawedWidth { get; protected set; }
        public float LastDrawedHeight { get; protected set; }

        public float X => AbsoulteX + TranslatedX;
        public float Y => AbsoluteY + TranslatedY;

        bool isMouseOver = false;
        public bool IsLeftClickDown { get; private set; }
        SKPoint mouseLeftClickPosition = SKPoint.Empty;
        SKPoint mousePosition = SKPoint.Empty;

        public virtual void OnEvent(WindowEvent e)
        {
            // e.MarkAsHandled();            
            var x = X;
            var y = Y;
            switch (e)
            {
                case OnMouseMoveEvent mouseMove:
                    {
                        var rect = new SKRect(x, y, x + YogaNode.LayoutWidth, y + YogaNode.LayoutHeight);
                        mousePosition = new SKPoint(mouseMove.X, mouseMove.Y);
                        OnMouseMove(mouseMove);
                        if (isMouseOver)
                        {
                            if (!rect.Contains(mousePosition))
                            {
                                OnMouseLeave(mouseMove);
                                isMouseOver = false;
                            }
                        }
                        else
                        {
                            if (rect.Contains(mousePosition))
                            {
                                OnMouseEnter(mouseMove);
                                isMouseOver = true;
                            }
                        }
                    }
                    break;
                case MouseDownEvent mouseDown:
                    {
                        if (mouseDown.MouseButton == CSXSkiaMouseButton.Left)
                        {
                            IsLeftClickDown = true;
                            mouseLeftClickPosition = mousePosition;
                        }
                        var rect = new SKRect(x, y, x + YogaNode.LayoutWidth, y + YogaNode.LayoutHeight);
                        if (rect.Contains(mousePosition))
                        {
                            OnMouseButtonDown(mouseDown);
                        }
                    }
                    break;
                case MouseUpEvent mouseUp:
                    {
                        IsLeftClickDown = false;
                        var rect = new SKRect(x, y, x + YogaNode.LayoutWidth, y + YogaNode.LayoutHeight);
                        if (mouseUp.MouseButton == CSXSkiaMouseButton.Left)
                        {
                            if (rect.Contains(mouseLeftClickPosition) && rect.Contains(mousePosition))
                            {
                                OnLeftClick(mouseUp);
                            }
                        }
                        if (rect.Contains(mousePosition))
                        {
                            OnMouseButtonUp(mouseUp);
                        }
                    }

                    break;
                case MouseWheelEvent mouseWheel:
                    {
                        var rect = new SKRect(x, y, x + YogaNode.LayoutWidth, y + YogaNode.LayoutHeight);
                        if (rect.Contains(mousePosition))
                        {
                            OnMouseWheel(mouseWheel);
                            isMouseOver = false;
                        }
                    }
                    break;
                case KeyDownEvent keyDown:
                    OnKeyDown(keyDown);
                    break;

                case KeyUpEvent keyUp:
                    OnKeyUp(keyUp);
                    break;

                case TextInputEvent textInput:
                    OnText(textInput);
                    break;

                case FrameDrawEvent frameDraw:
                    OnFrameDraw(frameDraw);
                    break;
            }

        }

        protected virtual void OnFrameDraw(FrameDrawEvent ev)
        {
            ev.MarkAsHandled();
        }

        protected virtual void OnMouseMove(OnMouseMoveEvent ev)
        {
            ev.MarkAsHandled();
        }

        protected virtual void OnMouseButtonDown(MouseDownEvent ev)
        {
            ev.MarkAsHandled();
        }

        protected virtual void OnMouseButtonUp(MouseUpEvent ev)
        {
            ev.MarkAsHandled();
        }

        protected virtual void OnMouseWheel(MouseWheelEvent ev)
        {
            ev.MarkAsHandled();
        }
        protected virtual void OnKeyDown(KeyDownEvent ev)
        {
            ev.MarkAsHandled();
        }
        protected virtual void OnKeyUp(KeyUpEvent ev)
        {
            ev.MarkAsHandled();
        }
        protected virtual void OnText(TextInputEvent ev)
        {
            ev.MarkAsHandled();
        }
        protected virtual void OnLeftClick(MouseUpEvent ev)
        {
            EventFired.OnNext(new Event(Id, NativeEvent.Click, new CursorEventArgs(mouseLeftClickPosition.X, mouseLeftClickPosition.Y)));
            ev.MarkAsHandled();
        }
        protected virtual void OnMouseEnter(OnMouseMoveEvent ev)
        {
            EventFired.OnNext(new Event(Id, NativeEvent.MouseOver, new CursorEventArgs(ev.X, ev.Y)));
            ev.MarkAsHandled();
        }
        protected virtual void OnMouseLeave(OnMouseMoveEvent ev)
        {
            EventFired.OnNext(new Event(Id, NativeEvent.MouseOut, new CursorEventArgs(ev.X, ev.Y)));
            ev.MarkAsHandled();
        }

        public virtual void SetAttribute(NativeAttribute attribute, object? value)
        {
            if (value == null)
            {
                return;
            }

            object? currentValue = null;
            Attributes.TryGetValue(attribute, out currentValue);

            if (value == currentValue || value.Equals(currentValue))
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
                    YogaNode.FlexBasis = ToYogaValue((CSXValue)value);
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
                    YogaNode.Height = ToYogaValue((CSXValue)value);
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
                    YogaNode.MaxHeight = ToYogaValue((CSXValue)value);
                    break;

                case NativeAttribute.MaxWidth:
                    YogaNode.MaxWidth = ToYogaValue((CSXValue)value);
                    break;

                case NativeAttribute.MinHeight:
                    YogaNode.MinHeight = ToYogaValue((CSXValue)value);
                    break;

                case NativeAttribute.MinWidth:
                    YogaNode.MinWidth = ToYogaValue((CSXValue)value);
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
                    YogaNode.Width = ToYogaValue((CSXValue)value);
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


            IsDirty = true;

        }

        static YogaValue ToYogaValue(CSXValue value)
        {
            return value.Unit switch
            {
                CSXUnit.Undefined => YogaValue.Undefined(),
                CSXUnit.Point => YogaValue.Point(value.Value),
                CSXUnit.Percent => YogaValue.Percent(value.Value),
                CSXUnit.Auto => YogaValue.Auto(),
                _ => throw new NotImplementedException()
            };
        }

        public virtual void Mesure()
        {
            // Used for things like text
        }

        public virtual bool NeedsToReDraw()
        {
            return IsDirty || YogaNode.HasNewLayout;
        }

        public virtual bool IsLayoutDirty()
        {
            return YogaNode.IsDirty;
        }

        public bool RectChanged()
        {
            if(
                YogaNode.LayoutX != RelativeX ||
                YogaNode.LayoutY != RelativeY ||
                YogaNode.LayoutHeight != LastDrawedHeight ||
                YogaNode.LayoutWidth != LastDrawedWidth
                )
            {
                return true;
            }

            return false;
        }

        public abstract void CalculateLayout();

        public abstract bool Draw(SKCanvas canvas, bool forceDraw, int level, SKRect? clipRect, float translateY, DrawContext context);        

    }
}
