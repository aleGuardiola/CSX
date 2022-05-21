using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Lab
{
    public class StopwatchInterceptor : IInterceptor
    {
        Dictionary<string, Average> Averages;

        public StopwatchInterceptor(Dictionary<string, Average> averages)
        {
            Averages = averages;
        }

        public void Intercept(IInvocation invocation)
        {
            var sw = Stopwatch.StartNew();

            invocation.Proceed();

            sw.Stop();

            if (Averages.TryGetValue(invocation.Method.Name, out Average? value))
            { 
                value?.Add(sw.ElapsedTicks);
            }
            else
            {
                var newValue = new Average();
                newValue.Add(sw.ElapsedTicks);
                Averages.Add(invocation.Method.Name, newValue);
            }
        }
    }
}
