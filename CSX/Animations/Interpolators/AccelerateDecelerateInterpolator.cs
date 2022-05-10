using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Animations.Interpolators
{
    public class AccelerateDecelerateInterpolator : Interpolator
    {
        //cos((t+1)π)/2+0.5
        public override float GetValue(float timeFraction)
        {
            return (float)Math.Cos((timeFraction + 1f) * Math.PI) / 2f + 0.5f;
        }
    }
}
