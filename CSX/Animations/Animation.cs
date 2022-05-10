namespace CSX.Animations
{
    public abstract class Animation
    {
        public int CurrentDuration { get; private set; }
        public int Duration { get; }

        protected bool IsStopped = false;
        public virtual bool IsPlaying => !IsStopped && Duration > CurrentDuration;

        public Animation(int duration)
        {
            CurrentDuration = 0;
            Duration = duration;            
        }

        protected abstract float ChangeFunction(int time);
        protected abstract void UpdateValue(float value);

        public virtual void Update(int deltaTime)
        {
            CurrentDuration += deltaTime;
            var value = ChangeFunction(CurrentDuration);
            UpdateValue(value);
        }

        public void Stop()
        {
            IsStopped = true;
        }

    }
}
