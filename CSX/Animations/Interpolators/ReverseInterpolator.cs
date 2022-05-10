using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Animations.Interpolators
{
    public class ReverseInterpolator : Interpolator
    {
        public override float GetValue(float timeFraction)
        {
            return 1f - timeFraction;
        }
    }
}
