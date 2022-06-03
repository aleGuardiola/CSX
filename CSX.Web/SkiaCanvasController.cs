﻿using CSX.Events;
using CSX.Web.Skia;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Web
{
    public class SkiaCanvasController : IDisposable
    {
        private SKHtmlCanvasInterop interop = null!;
        private SizeWatcherInterop sizeWatcher = null!;
        private DpiWatcherInterop dpiWatcher = null!;
        private string _htmlCanvasId;

        private SKSizeI pixelSize;
        private byte[]? pixels;
        private GCHandle pixelsHandle;
        private bool ignorePixelScaling;
        private double dpi;
        private SKSize canvasSize;
        private bool enableRenderLoop;

        public SkiaCanvasController(string htmlCanvasId)
        {
            _htmlCanvasId = htmlCanvasId;
        }

        public Action<SKPaintSurfaceEventArgs>? OnPaintSurface { get; set; }

        public bool EnableRenderLoop
        {
            get => enableRenderLoop;
            set
            {
                if (enableRenderLoop != value)
                {
                    enableRenderLoop = value;
                    Invalidate();
                }
            }
        }

        public bool IgnorePixelScaling
        {
            get => ignorePixelScaling;
            set
            {
                if (ignorePixelScaling != value)
                {
                    ignorePixelScaling = value;
                    Invalidate();
                }
            }
        }

        public void OnAfterFirstRender()
        {
            interop = SKHtmlCanvasInterop.Import(_htmlCanvasId, OnRenderFrame);
            interop.InitRaster();

            sizeWatcher = SizeWatcherInterop.Import(_htmlCanvasId, OnSizeChanged);
            dpiWatcher = DpiWatcherInterop.Import(OnDpiChanged);
        }

        public void Invalidate()
        {
            if (canvasSize.Width <= 0 || canvasSize.Height <= 0 || dpi <= 0)
                return;

            interop.RequestAnimationFrame(EnableRenderLoop, (int)(canvasSize.Width * dpi), (int)(canvasSize.Height * dpi));
        }

        private void OnRenderFrame()
        {
            if (canvasSize.Width <= 0 || canvasSize.Height <= 0 || dpi <= 0)
                return;

            var info = CreateBitmap(out var unscaledSize);
            var userVisibleSize = IgnorePixelScaling ? unscaledSize : info.Size;

            using (var surface = SKSurface.Create(info, pixelsHandle.AddrOfPinnedObject(), info.RowBytes))
            {
                if (IgnorePixelScaling)
                {
                    var canvas = surface.Canvas;
                    canvas.Scale((float)dpi);
                    canvas.Save();
                }

                OnPaintSurface?.Invoke(new SKPaintSurfaceEventArgs(surface, info.WithSize(userVisibleSize), info));
            }

            interop.PutImageData(pixelsHandle.AddrOfPinnedObject(), info.Size);
        }

        private SKImageInfo CreateBitmap(out SKSizeI unscaledSize)
        {
            var size = CreateSize(out unscaledSize);
            var info = new SKImageInfo(size.Width, size.Height, SKImageInfo.PlatformColorType, SKAlphaType.Opaque);

            if (pixels == null || pixelSize.Width != info.Width || pixelSize.Height != info.Height)
            {
                FreeBitmap();

                pixels = new byte[info.BytesSize];
                pixelsHandle = GCHandle.Alloc(pixels, GCHandleType.Pinned);
                pixelSize = info.Size;
            }

            return info;
        }

        private SKSizeI CreateSize(out SKSizeI unscaledSize)
        {
            unscaledSize = SKSizeI.Empty;

            var w = canvasSize.Width;
            var h = canvasSize.Height;

            if (!IsPositive(w) || !IsPositive(h))
                return SKSizeI.Empty;

            unscaledSize = new SKSizeI((int)w, (int)h);
            return new SKSizeI((int)(w * dpi), (int)(h * dpi));

            static bool IsPositive(double value)
            {
                return !double.IsNaN(value) && !double.IsInfinity(value) && value > 0;
            }
        }

        private void FreeBitmap()
        {
            if (pixels != null)
            {
                pixelsHandle.Free();
                pixels = null;
            }
        }

        private void OnDpiChanged(double newDpi)
        {
            dpi = newDpi;

            Invalidate();
        }

        private void OnSizeChanged(SKSize newSize)
        {
            canvasSize = newSize;

            Invalidate();
        }

        public void Dispose()
        {
            dpiWatcher.Unsubscribe(OnDpiChanged);
            sizeWatcher.Dispose();
            interop.Dispose();

            FreeBitmap();
        }
    }
}