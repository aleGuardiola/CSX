using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSX.Animations
{
    public abstract class Animation
    {
        public int Duration { get; }
        
        public Animation(int duration)
        {
            Duration = duration;            
        }

        protected abstract float ChangeFunction(int time);
        protected abstract void UpdateValue(float value);

        public void Update(int deltaTime)
        {
            var value = ChangeFunction(deltaTime);
            UpdateValue(value);
        }

    }
}
