using CSX.Animations.Interpolators;
using CSX.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Animations
{
    public class FloatStateAnimation<TC, TS, TP> : StateAnimation<float, TC, TS, TP>
                                                    where TC : Component<TS, TP>
                                                    where TP : Props
                                                    where TS : IEquatable<TS>
    {

        public float Start { get; }
        public float End { get; }
        public Interpolator Interpolator { get; }

        public FloatStateAnimation(int duration, TC component, Func<TS, float, TS> changeState, float start, float end, Interpolator interpolator) : base(duration, component, changeState)
        {
            Start = start;
            End = end;
            Interpolator = interpolator;
        }

        protected override float ConvertScaleValue(float scaleValue)
        {
            var diff = End - Start;
            return Start + (diff * scaleValue);
        }

        protected override float GetScaleValue(float timeScale)
        {
            return Interpolator.GetValue(timeScale);
        }
    }
}
