using Microsoft.JSInterop;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Web.Skia
{
	internal class SizeWatcherInterop : JSModuleInterop
	{
		private const string ModuleName = "CSXSKIA";
		private const string ObserveSymbol = "SizeWatcher.observe";
		private const string UnobserveSymbol = "SizeWatcher.unobserve";
				
		private readonly string htmlElementId;
		private readonly FloatFloatActionHelper callbackHelper;

		private DotNetObjectReference<FloatFloatActionHelper>? callbackReference;

		public static SizeWatcherInterop Import(string elementId, Action<SKSize> callback)
		{
			var interop = new SizeWatcherInterop(elementId, callback);
			interop.ImportModule();
			interop.Start();
			return interop;
		}

		public SizeWatcherInterop(string elementId, Action<SKSize> callback)
			: base(ModuleName)
		{			
			htmlElementId = elementId;
			callbackHelper = new FloatFloatActionHelper((x, y) => callback(new SKSize(x, y)));
		}

		protected override void OnDisposingModule() =>
			Stop();

		public void Start()
		{
			if (callbackReference != null)
				return;

			callbackReference = DotNetObjectReference.Create(callbackHelper);
			
			Invoke(ObserveSymbol, htmlElementId, callbackReference);
		}

		public void Stop()
		{
			if (callbackReference == null)
				return;

			Invoke(UnobserveSymbol, htmlElementId);

			callbackReference?.Dispose();
			callbackReference = null;
		}
	}
}
