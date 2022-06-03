using OpenTK.Windowing.Desktop;
using OpenTK.Graphics;
using OpenTK;
using OpenTK.Windowing.Common;
using SkiaSharp;
using OpenTK.Graphics.OpenGL;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;

namespace CSX.OpenTK.Test
{
    public abstract class SkiaWindow : GameWindow
    {
        private const SKColorType colorType = SKColorType.Rgba8888;
        private const GRSurfaceOrigin surfaceOrigin = GRSurfaceOrigin.BottomLeft;

        private GRContext? grContext;
        private GRGlFramebufferInfo glInfo;
        private GRBackendRenderTarget? renderTarget;
        private SKSurface? surface;
        private SKCanvas? canvas;

        private SKSizeI lastSize;

        public SkiaWindow(GameWindowSettings gameSettings, NativeWindowSettings settings) : base(gameSettings, settings) 
        {
            
        }

        public override void SwapBuffers()
        {
            base.SwapBuffers();
        }
             
        protected override void OnRenderFrame(FrameEventArgs args)
        {            
            // create the contexts if not done already
            if (grContext == null)
            {
                var glInterface = GRGlInterface.Create();
                grContext = GRContext.CreateGl(glInterface);
            }

            // get the new surface size
            var newSize = new SKSizeI(Size.X, Size.Y);

            // manage the drawing surface
            if (renderTarget == null || lastSize != newSize || !renderTarget.IsValid)
            {
                // create or update the dimensions
                lastSize = newSize;

                Span<int> framebufferSpan = new Span<int>(new int[1]);
                Span<int> stencilSpan = new Span<int>(new int[1]);
                Span<int> samplesSpan = new Span<int>(new int[1]);

                GL.GetInteger(GetPName.DrawFramebufferBinding, framebufferSpan);
                GL.GetInteger(GetPName.StencilBits, stencilSpan);
                GL.GetInteger(GetPName.Samples, samplesSpan);
                var maxSamples = grContext.GetMaxSurfaceSampleCount(colorType);

                var samples = samplesSpan[0];
                var framebuffer = framebufferSpan[0];
                var stencil = stencilSpan[0];

                if (samples > maxSamples)
                    samples = maxSamples;
                glInfo = new GRGlFramebufferInfo((uint)framebuffer, colorType.ToGlSizedFormat());

                // destroy the old surface
                surface?.Dispose();
                surface = null;
                canvas = null;

                // re-create the render target
                renderTarget?.Dispose();
                renderTarget = new GRBackendRenderTarget(newSize.Width, newSize.Height, samples, stencil, glInfo);
            }


            // create the surface
            if (surface == null)
            {
                surface = SKSurface.Create(grContext, renderTarget, surfaceOrigin, colorType);
                canvas = surface.Canvas;
            }

            using (new SKAutoCanvasRestore(canvas, true))
            {
                // start drawing
                OnPaintSurface(new SKPaintGLSurfaceEventArgs(surface, renderTarget, surfaceOrigin, colorType), args.Time);
            }

            // update the control
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            canvas.Flush();
#pragma warning restore CS8602 // Dereference of a possibly null reference.
            SwapBuffers();

            base.OnRenderFrame(args);
        }

        public abstract void OnPaintSurface(SKPaintGLSurfaceEventArgs e, double time);        

    }
}
