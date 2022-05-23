using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Animations
{
    public class ParallelAnimation : Animation
    {
        Animation[] Animations { get; }

        public ParallelAnimation(Animation[] animations) : base(animations.Max(a => a.Duration))
        {
            Animations = animations;
        }

        protected override float ChangeFunction(int time)
        {
            return 0;
        }

        protected override void UpdateValue(float value)
        {
            
        }

        public override bool IsPlaying => !IsStopped && Animations.Any(x => x.IsPlaying);

        public override void Update(int deltaTime)
        {
            foreach (var animation in Animations)
            {
                if(animation.IsPlaying)
                {
                    animation.Update(deltaTime);
                }                
            }
        }
    }
}
