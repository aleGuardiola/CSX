using CSX.NativeComponents;
using CSX.Rendering;
using CSX.Skia.Events;
using Facebook.Yoga;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Skia.Views.ScrollBars
{
    public class DefaultScrollBarView : View
    {
        public float ScrollBarWidth = 17f;
        public float ScrollBarMinimumHeight = 20f;
        public Color ScrollBarBackgroundColor = ColorTranslator.FromHtml("#424242");
        public Color ScrollBarColor = ColorTranslator.FromHtml("#686868");
        public Color ScrollBarButtonColor = Color.White;
        public Color ScrollBarButtonDisabledColor = ColorTranslator.FromHtml("#808080");
        public Color ScrollBarButtonHoverColor = ColorTranslator.FromHtml("#4f4f4f");
        
        DefaultScrollBarButton _up;
        DefaultScrollBarButton _down;
        DefaultScrollBarBar _bar;

        float _lastScrollPosition = 0f;

        public DefaultScrollBarView(ulong id) : base(id)
        {
            SetAttribute(NativeAttribute.BackgroundColor, ScrollBarBackgroundColor);
            SetAttribute(NativeAttribute.Width, ScrollBarWidth);
            YogaNode.Height = YogaValue.Percent(100f);            
            SetAttribute(NativeAttribute.Position, Position.Absolute);
            SetAttribute(NativeAttribute.Top, 0f);
            SetAttribute(NativeAttribute.Right, 0f);

            _up = new DefaultScrollBarButton(id, true);
            _up.SetAttribute(NativeAttribute.Top, 0f);
            _up.SetAttribute(NativeAttribute.Left, 0f);
            _up.SetAttribute(NativeAttribute.Width, ScrollBarWidth);
            _up.SetAttribute(NativeAttribute.Height, ScrollBarWidth);
            
            _down = new DefaultScrollBarButton(id, false);
            _down.SetAttribute(NativeAttribute.Bottom, 0f);
            _down.SetAttribute(NativeAttribute.Left, 0f);
            _down.SetAttribute(NativeAttribute.Width, ScrollBarWidth);
            _down.SetAttribute(NativeAttribute.Height, ScrollBarWidth);
            
            _bar = new DefaultScrollBarBar(id);
            _bar.SetAttribute(NativeAttribute.Top, ScrollBarWidth);
            _bar.SetAttribute(NativeAttribute.Left, 2f);
            _bar.SetAttribute(NativeAttribute.BackgroundColor, ScrollBarColor);
            _bar.SetAttribute(NativeAttribute.Width, ScrollBarWidth - 4f);
            _bar.SetAttribute(NativeAttribute.Height, 100f);

            AppendView(_up);
            AppendView(_bar);
            AppendView(_down);

        }

        public override bool NeedsToReDraw()
        {
            //return false;
            var scrollView = Parent as ScrollView ?? throw new InvalidOperationException("Parent is not scroll view");
            var scrollPosition = scrollView.Content.GetScrollPosition();
            if (scrollPosition != _lastScrollPosition)
            {
                _lastScrollPosition = scrollPosition;
                return true;
            }            
            else
            {

            }

            if(base.NeedsToReDraw())
            {
                return true;
            }

            return false;
        }

        public override bool Draw(SKCanvas canvas, bool forceDraw, int level, SKRect? clipRect, float translateY, DrawContext context)
        {
            var scrollView = Parent as ScrollView ?? throw new InvalidOperationException("Parent is not scroll view");
            var totalContentLenght = scrollView.Content.GetContentHeight();
            var maxScroll = scrollView.GetMaxScroll();

            if(maxScroll == 0)
            {
                _bar.SetAttribute(NativeAttribute.Height, 0f);
                _up.IsDisabled = true;
                _down.IsDisabled = true;

                return base.Draw(canvas, forceDraw, level, clipRect, translateY, context);
            }

            var barHeight = Math.Max((YogaNode.LayoutHeight / totalContentLenght) * ((YogaNode.LayoutHeight - ScrollBarWidth) - ScrollBarWidth), ScrollBarMinimumHeight);
            _bar.SetAttribute(NativeAttribute.Height, barHeight);                       

            var scrollBarPostion = Math.Min(1f, Math.Max(0f, scrollView.Content.GetScrollPosition() / maxScroll));

            if(scrollBarPostion == 1f)
            {
                _down.IsDisabled = true;
            }
            else if(scrollBarPostion == 0f)
            {
                _up.IsDisabled = true;
            }
            else
            {
                _up.IsDisabled = false;
                _down.IsDisabled = false;
            }

            var scrollBarStart = ScrollBarWidth + (_bar.YogaNode.LayoutHeight / 2f);
            var scrollBarStop = (YogaNode.LayoutHeight - ScrollBarWidth) - (_bar.YogaNode.LayoutHeight / 2f);
            var scrollBarTotal = scrollBarStop - scrollBarStart;
            var scrollBarY = ScrollBarWidth + scrollBarPostion * scrollBarTotal;

            _bar.SetAttribute(NativeAttribute.Top, scrollBarY);

            return base.Draw(canvas, forceDraw, level, clipRect, translateY, context);
        }

    }


    public class DefaultScrollBarButton : View
    {
        bool _isUp;
        bool _isDisabled = false;

        public bool IsDisabled
        {
            get => _isDisabled;
            set
            { 
                if(value != _isDisabled)
                {
                    IsDirty = true;
                }
                _isDisabled = value;                
            }
        }

        public DefaultScrollBarButton(ulong id, bool isUp) : base(id)
        {
            SetAttribute(NativeAttribute.Position, Position.Absolute);
            _isUp = isUp;
        }

        protected override void OnMouseEnter(OnMouseMoveEvent ev)
        {
            if(_isDisabled)
            {
                return;
            }
            var scrollBar = Parent as DefaultScrollBarView ?? throw new InvalidOperationException("Parent is not scroll view");
            SetAttribute(NativeAttribute.BackgroundColor, scrollBar.ScrollBarButtonHoverColor);
            base.OnMouseEnter(ev);
        }

        protected override void OnMouseLeave(OnMouseMoveEvent ev)
        {
            var scrollBar = Parent as DefaultScrollBarView ?? throw new InvalidOperationException("Parent is not scroll view");
            SetAttribute(NativeAttribute.BackgroundColor, scrollBar.ScrollBarBackgroundColor);
            base.OnMouseLeave(ev);
        }

        protected override void OnLeftClick(MouseUpEvent ev)
        {
            if(_isDisabled)
            {
                return;
            }

            var scrollBar = Parent as DefaultScrollBarView ?? throw new InvalidOperationException("Parent is not scroll view");
            var scrollView = scrollBar.Parent as ScrollView ?? throw new InvalidOperationException("Parent is not scroll view");
            if (_isUp)
            {
                scrollView.MoveScrollBarPosition(100f);
            }
            else
            {
                scrollView.MoveScrollBarPosition(-100f);
            }
            base.OnLeftClick(ev);
        }

        public override bool Draw(SKCanvas canvas, bool forceDraw, int level, SKRect? clipRect, float translateY, DrawContext context)
        {
            var scrollBar = Parent as DefaultScrollBarView ?? throw new InvalidOperationException("Parent is not scroll view");
            base.Draw(canvas, forceDraw, level, clipRect, translateY, context);

            var h = YogaNode.LayoutHeight;

            var px = AbsoulteX;
            var py = AbsoluteY;

            var c = h / 2f;
            var y = c - ((h * .25f) / 2);
            
            var path = new SKPath();

            if(_isUp)
            {
                path.MoveTo(c + px, y + py);

                path.LineTo(y + px, (h - y) + py);
                path.LineTo((h - y) + px, (h - y) + py);
                path.LineTo(c + px, y + py);
            }
            else
            {
                path.MoveTo(y + px, y + py);

                path.LineTo(c + px, (h - y) + py);
                path.LineTo((h - y) + px, y + py);
                path.LineTo(y + px, y + py);
            }

            var color = _isDisabled 
                ? new SKColor(scrollBar.ScrollBarButtonDisabledColor.R, scrollBar.ScrollBarButtonDisabledColor.G, scrollBar.ScrollBarButtonDisabledColor.B, scrollBar.ScrollBarButtonDisabledColor.A)
                : new SKColor(scrollBar.ScrollBarButtonColor.R, scrollBar.ScrollBarButtonColor.G, scrollBar.ScrollBarButtonColor.B, scrollBar.ScrollBarButtonColor.A);

            canvas.DrawPath(path, new SKPaint()
            {
                Color = color,
                Style = SKPaintStyle.Fill,
                IsAntialias = true
            });

            return true;
        }

    }

    public class DefaultScrollBarBar : View
    {
        bool _isDragging;
        SKPoint _mouseDownPosition;
        SKPoint _mousePosition;
        float _positionWhenUp;

        public DefaultScrollBarBar(ulong id) : base(id)
        {
            SetAttribute(NativeAttribute.Position, Position.Absolute);
        }

        protected override void OnMouseMove(OnMouseMoveEvent ev)
        {
            _mousePosition = new SKPoint(ev.X, ev.Y);
            base.OnMouseMove(ev);
        }

        protected override void OnMouseButtonDown(MouseDownEvent ev)
        {
            if (ev.MouseButton == CSXSkiaMouseButton.Left)
            {                
                _mouseDownPosition = _mousePosition;                
                _positionWhenUp = YogaNode.Top.Value;
                _isDragging = true;
            }
            base.OnMouseButtonDown(ev);
        }

        public override void OnEvent(WindowEvent ev)
        {
            if(ev is MouseUpEvent e)
            {
                OnAnyMouseButtonUp(e);
            }
            base.OnEvent(ev);
        }

        protected void OnAnyMouseButtonUp(MouseUpEvent ev)
        {
            if (ev.MouseButton == CSXSkiaMouseButton.Left)
            {
                _isDragging = false;
            }
            base.OnMouseButtonUp(ev);
        }

        protected override void OnFrameDraw(FrameDrawEvent ev)
        {
            var scrollBar = Parent as DefaultScrollBarView ?? throw new InvalidOperationException("Parent is not scroll view");
            var scrollView = scrollBar.Parent as ScrollView ?? throw new InvalidOperationException("Parent is not scroll view");

            if(!_isDragging)
            {
                return;
            }

            var maxScroll = scrollView.GetMaxScroll();
            var scrollBarStart = scrollBar.ScrollBarWidth + (YogaNode.LayoutHeight / 2f);
            var scrollBarStop = (scrollBar.YogaNode.LayoutHeight - scrollBar.ScrollBarWidth) - (YogaNode.LayoutHeight / 2f);
            var scrollBarTotal = scrollBarStop - scrollBarStart;


            var difference = _mousePosition.Y - _mouseDownPosition.Y;
            var y = _positionWhenUp + difference;

            var scrollPosition = ((y - scrollBar.ScrollBarWidth) / scrollBarTotal) * maxScroll;

            scrollView.SetScrollPosition(scrollPosition);

            base.OnFrameDraw(ev);
        }

    }

}
