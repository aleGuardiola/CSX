using CSX.NativeComponents;
using CSX.Rendering;
using CSX.Skia;
using CSX.Skia.Events;
using CSX.Skia.Views;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Common.Input;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.OpenTK.Test
{
    public class CSXWindow : SkiaWindow
    {
        SkiaDom _dom;

        ulong _eventId = 1;

        public BaseView? Root => _dom.Root;

        bool forceDraw = true;        
        DrawContext? context;

        bool _transparent;
        bool _showFPS;

        public CSXWindow(SkiaDom dom, GameWindowSettings gameSettings, NativeWindowSettings settings, bool transparent = false, bool showFPS = false) : base(gameSettings, settings)
        {
            _dom = dom;
            _transparent = transparent;
            _showFPS = showFPS;
        }

        protected override void OnMove(WindowPositionEventArgs e)
        {
            unsafe
            {
                var monitor = GLFW.GetWindowMonitor(this.WindowPtr);
            }           
            
            base.OnMove(e);
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            Root?.OnEvent(new KeyDownEvent(GetNewEventId(), (CSXSkiaKey)(int)e.Key));
            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyboardKeyEventArgs e)
        {
            Root?.OnEvent(new KeyUpEvent(GetNewEventId(), (CSXSkiaKey)(int)e.Key));
            base.OnKeyUp(e);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            Root?.OnEvent(new MouseDownEvent(GetNewEventId(), (CSXSkiaMouseButton)(int)e.Button));
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            Root?.OnEvent(new MouseUpEvent(GetNewEventId(), (CSXSkiaMouseButton)(int)e.Button));
            base.OnMouseUp(e);
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            Root?.OnEvent(new OnMouseMoveEvent(GetNewEventId(), e.X, e.Y));
            base.OnMouseMove(e);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            Root?.OnEvent(new MouseWheelEvent(GetNewEventId(), e.OffsetX, e.OffsetY));
            base.OnMouseWheel(e);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            forceDraw = true;
            Root?.SetAttribute(NativeAttribute.Width, (CSXValue)e.Width);
            Root?.SetAttribute(NativeAttribute.Height, (CSXValue)e.Height);
            base.OnResize(e);
        }

        protected override void OnTextInput(TextInputEventArgs e)
        {
            Root?.OnEvent(new TextInputEvent(GetNewEventId(), e.Unicode));
            base.OnTextInput(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            if (Root?.YogaNode.IsDirty != null && Root.YogaNode.IsDirty)
            {
                Root.CalculateLayout();
            }
            base.OnUpdateFrame(args);
        }

        public override void OnPaintSurface(SKPaintGLSurfaceEventArgs e, double time)
        {
            if(e.Info.Width == 0 || e.Info.Height == 0)
            {
                return;
            }

            var canvas = e.Surface.Canvas;
            
            if (forceDraw || context == null)
            {                
                context?.Dispose();
                context = new DrawContext()
                {
                    ImageInfo = e.Info,
                    ShowScreenDraws = false,
                    SurfaceFactory = () =>
                    {
                        return SKSurface.Create(e.Surface.Context, true, e.Info);
                    },
                    CursorFactory = (cursor) =>
                    {
                        Cursor = cursor switch
                        {
                            Skia.Input.CSXSkiaCursor.Default => MouseCursor.Default,
                            Skia.Input.CSXSkiaCursor.IBeam => MouseCursor.IBeam,
                            Skia.Input.CSXSkiaCursor.Crosshair => MouseCursor.Crosshair,
                            Skia.Input.CSXSkiaCursor.Hand => MouseCursor.Hand,
                            Skia.Input.CSXSkiaCursor.VResize => MouseCursor.VResize,
                            Skia.Input.CSXSkiaCursor.HResize => MouseCursor.HResize,
                            Skia.Input.CSXSkiaCursor.Empty => MouseCursor.Empty,
                            Skia.Input.CSXSkiaCursor.Help => MouseCursor.Default,
                            Skia.Input.CSXSkiaCursor.No => MouseCursor.Default,
                            Skia.Input.CSXSkiaCursor.Wait => MouseCursor.Default,
                            Skia.Input.CSXSkiaCursor.Move => MouseCursor.VResize,
                            Skia.Input.CSXSkiaCursor.MoveUp => MouseCursor.VResize,
                            Skia.Input.CSXSkiaCursor.MoveDown => MouseCursor.VResize,
                            _ => throw new NotImplementedException()
                        };
                    }
                };                
            }

            if(Root == null)
            {
                canvas.Clear(_transparent ? SKColors.Transparent : SKColors.White);
            }
            else
            {
                if (Root.NeedsToReDraw())
                {
                    context.SetDeep(Root.Deep);
                    Root.Draw(context.GetCanvas(0), forceDraw, 0, null, 0f, context);
                    Root.MarkAsSeen();
                    Root.YogaNode.MarkLayoutSeen();                    
                }
                canvas.Clear(_transparent ? SKColors.Transparent : SKColors.White);
                foreach (var surface in context.Surfaces.Where(x => x != null))
                {
                    canvas.DrawSurface(surface, 0, 0);
                }
                if(context.ScreenDrawsSurface != null)
                {
                    canvas.DrawSurface(context.ScreenDrawsSurface, 0, 0);
                    context.ScreenDrawsSurface.Canvas.Clear();
                }

                Root.OnEvent(new FrameDrawEvent(GetNewEventId(), time));
                _dom.OnFrame(time);
            }

            if(_showFPS)
            {
                canvas.DrawText($"FPS: {(int)(1.0 / time)}", new SKPoint(10, 10), new SKPaint()
                {
                    Color = SKColors.Red,                    
                });
            }

            //Console.Clear();
            //Console.WriteLine($"Frame Time: {time * 1000}ms");
            //Console.WriteLine($"FPS: {1.0 / time}");

            forceDraw = false;
        }

        ulong GetNewEventId()
            => _eventId ++;

    }
}
