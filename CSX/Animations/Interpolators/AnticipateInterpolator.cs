using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Animations.Interpolators
{
    public class AnticipateInterpolator : Interpolator
    {
        public float Tension { get; set; }

        public AnticipateInterpolator()
        {
            Tension = 1f;
        }

        //(T+1)×t3–T×t2
        public override float GetValue(float timeFraction)
        {
            return (Tension + 1) * (float)Math.Pow(timeFraction, 3) - Tension * (float)Math.Pow(timeFraction, 2);
        }
    }
}
