using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Animations.Interpolators
{
    public class LinearInterplator : Interpolator
    {
        public override float GetValue(float timeFraction)
        {
            return timeFraction;
        }
    }
}
