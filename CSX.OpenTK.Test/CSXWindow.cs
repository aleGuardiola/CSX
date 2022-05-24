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
        BaseView? _root = null;
        public BaseView? Root 
        { 
            get
            {
                return _root;
            }
            set
            {
                if(value == null)
                {
                    _root = null;
                    return;
                }
                value.SetAttribute(NativeAttribute.Width, (float)Size.X);
                value.SetAttribute(NativeAttribute.Height, (float)Size.Y);
                _root = value;
            }
        }

        bool forceDraw = true;        
        DrawContext? context;

        public CSXWindow(GameWindowSettings gameSettings, NativeWindowSettings settings) : base(gameSettings, settings)
        {
        }

        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            Root?.OnEvent(new KeyDownEvent((CSXSkiaKey)(int)e.Key));
            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyboardKeyEventArgs e)
        {
            Root?.OnEvent(new KeyUpEvent((CSXSkiaKey)(int)e.Key));
            base.OnKeyUp(e);
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {
            Root?.OnEvent(new MouseDownEvent((CSXSkiaMouseButton)(int)e.Button));
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseButtonEventArgs e)
        {
            Root?.OnEvent(new MouseUpEvent((CSXSkiaMouseButton)(int)e.Button));
            base.OnMouseUp(e);
        }

        protected override void OnMouseMove(MouseMoveEventArgs e)
        {
            Root?.OnEvent(new OnMouseMoveEvent(e.X, e.Y));
            base.OnMouseMove(e);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            Root?.OnEvent(new MouseWheelEvent(e.OffsetX, e.OffsetY));
            base.OnMouseWheel(e);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            forceDraw = true;
            _root?.SetAttribute(NativeAttribute.Width, (float)e.Width);
            _root?.SetAttribute(NativeAttribute.Height, (float)e.Height);
            base.OnResize(e);
        }

        protected override void OnTextInput(TextInputEventArgs e)
        {
            _root?.OnEvent(new TextInputEvent(e.Unicode));
            base.OnTextInput(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            if(Root?.YogaNode.IsDirty != null && Root.YogaNode.IsDirty)
            {
                Root.CalculateLayout();
            }
            base.OnUpdateFrame(args);
        }

        public override void OnPaintSurface(SKPaintGLSurfaceEventArgs e, double time)
        {
            var canvas = e.Surface.Canvas;

            if (forceDraw || context == null)
            {                
                context?.Dispose();
                context = new DrawContext()
                {
                    ImageInfo = e.Info,
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
                canvas.Clear(SKColors.White);                
            }
            else
            {
                if (Root.NeedsToReDraw())
                {
                    Root.Draw(context.GetCanvas(0), forceDraw, 0, null, 0f, context);
                    Root.MarkAsSeen();
                    Root.YogaNode.MarkLayoutSeen();

                    
                }
                canvas.Clear(SKColors.White);
                foreach (var surface in context.Surfaces)
                {
                    canvas.DrawSurface(surface, 0, 0);
                }
                Root.OnEvent(new FrameDrawEvent(time));
            }

            Console.Clear();
            Console.WriteLine($"Frame Time: {time}ms,  FPS: {1.0 / time}");

            forceDraw = false;
        }
    }
}
