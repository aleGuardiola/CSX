﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Animations.Interpolators
{
    public class DecelerateInterpolator : Interpolator
    {
        public float Factor { get; set; }

        public DecelerateInterpolator()
        {
            Factor = 1f;
        }

        public override float GetValue(float timeFraction)
        {
            return 1f - (float)Math.Pow(1f - timeFraction, 2f * Factor);
        }
    }
}
