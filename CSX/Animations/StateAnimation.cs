using CSX.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Animations
{
    public abstract class StateAnimation<T, TC, TS, TP> : Animation 
                                                    where TC : Component<TS, TP>
                                                    where TP : Props
                                                    where TS : IEquatable<TS>
    {
        public TC Component { get; }
        public Func<TS, T, TS> ChangeState { get; }

        public StateAnimation(int duration, TC component, Func<TS, T, TS> changeState) : base(duration)
        {
            Component = component;
            ChangeState = changeState;
        }

        protected abstract float GetScaleValue(float timeScale);
        protected abstract T ConvertScaleValue(float scaleValue);

        protected override float ChangeFunction(int time)
        {
            var timeScale = (float)time / (float)Duration;
            var valueScale = GetScaleValue(timeScale);
            return valueScale;
        }

        protected override void UpdateValue(float value)
        {
            var valueConverted = ConvertScaleValue(value);
            var newState = ChangeState.Invoke(Component.State, valueConverted);
            Component.SetState(newState);
        }
    }
}
