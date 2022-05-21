using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Web.Skia
{
	public class ActionHelper
	{
		private readonly Action action;

		public ActionHelper(Action action)
		{
			this.action = action;
		}

		[JSInvokable]
		public void Invoke() => action?.Invoke();
	}
}
