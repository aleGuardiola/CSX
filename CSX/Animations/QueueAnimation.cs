namespace CSX.Animations
{
    public class QueueAnimation : Animation
    {
        public Animation[] Animations { get; }
        Animation Current;
        int CurrentIndex = 0;

        public QueueAnimation(Animation[] animations) : base(animations.Sum(x => x.Duration))
        {
            Animations = animations;
            Current = animations[0];
        }

        public override bool IsPlaying => !IsStopped && !(CurrentIndex == Animations.Length - 1 && !Current.IsPlaying);

        protected override float ChangeFunction(int time)
        {
            throw new NotImplementedException();
        }

        protected override void UpdateValue(float value)
        {
            throw new NotImplementedException();
        }

        public override void Update(int deltaTime)
        {
            if(Current.IsPlaying)
            {
                Current.Update(deltaTime);
            }
            else if(CurrentIndex < Animations.Length - 1)
            {
                CurrentIndex++;
                Current = Animations[CurrentIndex];
                Current.Update(deltaTime);
            }
        }
        
    }
}
