using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Animations.Interpolators
{
    public class QuadraticInterpolator : Interpolator
    {
        public float Exponent
        {
            get;
            set;
        }

        public QuadraticInterpolator()
        {
            Exponent = 2f;
        }

        public override float GetValue(float timeFraction)
        {
            return (float)Math.Pow(timeFraction, Exponent);
        }
    }
}
