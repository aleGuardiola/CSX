using Microsoft.JSInterop;

namespace CSX.Web.Skia
{
	internal class DpiWatcherInterop : JSModuleInterop
	{
		private const string ModuleName = "CSXSKIA";
		private const string StartSymbol = "DpiWatcher.start";
		private const string StopSymbol = "DpiWatcher.stop";
		private const string GetDpiSymbol = "DpiWatcher.getDpi";

		private static DpiWatcherInterop? instance;

		private event Action<double>? callbacksEvent;
		private readonly FloatFloatActionHelper callbackHelper;

		private DotNetObjectReference<FloatFloatActionHelper>? callbackReference;

		public static DpiWatcherInterop Import(Action<double>? callback = null)
		{
			var interop = Get();
			interop.ImportModule();
			if (callback != null)
				interop.Subscribe(callback);
			return interop;
		}

		public static DpiWatcherInterop Get() =>
			instance ??= new DpiWatcherInterop();

		private DpiWatcherInterop()
			: base(ModuleName)
		{
			callbackHelper = new FloatFloatActionHelper((o, n) => callbacksEvent?.Invoke(n));
		}

		protected override void OnDisposingModule() =>
			Stop();

		public void Subscribe(Action<double> callback)
		{
			var shouldStart = callbacksEvent == null;

			callbacksEvent += callback;

			var dpi = shouldStart
				? Start()
				: GetDpi();

			callback(dpi);
		}

		public void Unsubscribe(Action<double> callback)
		{
			callbacksEvent -= callback;

			if (callbacksEvent == null)
				Stop();
		}

		private double Start()
		{
			if (callbackReference != null)
				return GetDpi();

			callbackReference = DotNetObjectReference.Create(callbackHelper);

			return Invoke<double>(StartSymbol, callbackReference);
		}

		private void Stop()
		{
			if (callbackReference == null)
				return;

			Invoke(StopSymbol);

			callbackReference?.Dispose();
			callbackReference = null;
		}

		public double GetDpi() =>
			Invoke<double>(GetDpiSymbol);
	}
}
