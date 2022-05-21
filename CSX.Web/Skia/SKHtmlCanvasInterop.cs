using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Web.Skia
{
	internal class SKHtmlCanvasInterop : JSModuleInterop
	{
		private const string ModuleName = "CSXSKIA";
		private const string InitGLSymbol = "SKHtmlCanvas.initGL";
		private const string InitRasterSymbol = "SKHtmlCanvas.initRaster";
		private const string DeinitSymbol = "SKHtmlCanvas.deinit";
		private const string RequestAnimationFrameSymbol = "SKHtmlCanvas.requestAnimationFrame";
		private const string PutImageDataSymbol = "SKHtmlCanvas.putImageData";
				
		private readonly string htmlElementId;
		private readonly ActionHelper callbackHelper;

		private DotNetObjectReference<ActionHelper>? callbackReference;

		public static SKHtmlCanvasInterop Import(string elementId, Action callback)
		{
			var interop = new SKHtmlCanvasInterop(elementId, callback);
			interop.ImportModule();
			return interop;
		}

		public SKHtmlCanvasInterop(string elementId, Action renderFrameCallback)
			: base(ModuleName)
		{			
			htmlElementId = elementId;

			callbackHelper = new ActionHelper(renderFrameCallback);
		}

		protected override void OnDisposingModule() =>
			Deinit();

		public GLInfo InitGL()
		{
			if (callbackReference != null)
				throw new InvalidOperationException("Unable to initialize the same canvas more than once.");

			callbackReference = DotNetObjectReference.Create(callbackHelper);

			return Invoke<GLInfo>(InitGLSymbol, htmlElementId, callbackReference);
		}

		public bool InitRaster()
		{
			if (callbackReference != null)
				throw new InvalidOperationException("Unable to initialize the same canvas more than once.");

			callbackReference = DotNetObjectReference.Create(callbackHelper);

			return Invoke<bool>(InitRasterSymbol, htmlElementId, callbackReference);
		}

		public void Deinit()
		{
			if (callbackReference == null)
				return;

			Invoke(DeinitSymbol, htmlElementId);

			callbackReference?.Dispose();
		}

		public void RequestAnimationFrame(bool enableRenderLoop, int rawWidth, int rawHeight) =>
			Invoke(RequestAnimationFrameSymbol, htmlElementId, enableRenderLoop, rawWidth, rawHeight);

		public void PutImageData(IntPtr intPtr, SKSizeI rawSize) =>
			Invoke(PutImageDataSymbol, htmlElementId, intPtr.ToInt64(), rawSize.Width, rawSize.Height);

		public record GLInfo(int ContextId, uint FboId, int Stencils, int Samples, int Depth);
	}
}
