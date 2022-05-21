using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAssembly.JSInterop;

namespace CSX.Web
{
	internal class JSModuleInterop : IDisposable
	{		
		private IJSUnmarshalledObjectReference? module;
		private IJSInProcessRuntime Js = (IJSInProcessRuntime)CSXJSRuntime.GetCurrent();

		string _moduleName;

		public JSModuleInterop(string moduleName)
		{
			_moduleName = moduleName;
		}

		public void ImportModule()
		{
			module = Js.Invoke<IJSUnmarshalledObjectReference>("CSX.module", _moduleName);
		}

		public void Dispose()
		{
			OnDisposingModule();
			Module.Dispose();
		}

		protected IJSUnmarshalledObjectReference Module =>
			module ?? throw new InvalidOperationException("Make sure to run ImportModule() first.");

		protected void Invoke(string identifier, params object?[]? args) =>
			Module.InvokeVoid(identifier, args);

		protected TValue Invoke<TValue>(string identifier, params object?[]? args) =>
			Module.Invoke<TValue>(identifier, args);

		protected virtual void OnDisposingModule() { }
	}
}
