using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Animations.Interpolators
{
    public class CycleInterpolator : Interpolator
    {
        public float Cycles { get; set; }

        public CycleInterpolator()
        {
            Cycles = 1f;
        }

        public override float GetValue(float timeFraction)
        {
            return (float)Math.Sin(2f * Math.PI * Cycles * timeFraction);
        }
    }
}
