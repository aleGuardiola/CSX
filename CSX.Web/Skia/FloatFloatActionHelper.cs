using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Web.Skia
{
	public class FloatFloatActionHelper
	{
		private readonly Action<float, float> action;

		public FloatFloatActionHelper(Action<float, float> action)
		{
			this.action = action;
		}

		[JSInvokable]
		public void Invoke(float width, float height) => action?.Invoke(width, height);
	}
}
