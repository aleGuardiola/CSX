using CSX.Animations.Interpolators;
using CSX.Components;

namespace CSX.Animations
{
    public static class ComponentExtensions
    {
        public static void RunAnimation(Animation animation)
        {
            int deltaTime = 5;
            Func<Task>? timer = null;
            timer = async () =>
            {
                await Task.Delay(deltaTime);
                animation.Update(deltaTime);
                if (animation.IsPlaying)
                {
                    _ = Task.Run(timer ?? throw new Exception());
                }
            };
            Task.Run(timer);
        }
        public static void RunAnimation<TState, TProps>(this Component<TState, TProps> component, Animation animation) where TProps : Props
                                                                                                                       where TState : IEquatable<TState>
            => RunAnimation(animation);

        public static Animation ValueAnimation<TState, TProps>(this Component<TState, TProps> component, int duration, float start, float end, Interpolator interpolator, Func<TState, float, TState> changeState) where TProps : Props
                                                                                                                                                                                                                         where TState : IEquatable<TState>        
            => new FloatStateAnimation<Component<TState, TProps>, TState, TProps>(duration, component, changeState, start, end, interpolator);

        public static Animation RunValueAnimation<TState, TProps>(this Component<TState, TProps> component, int duration, float start, float end, Interpolator interpolator, Func<TState, float, TState> changeState) where TProps : Props
                                                                                                                                                                                                                         where TState : IEquatable<TState>
        {
            var animation = ValueAnimation(component, duration, start, end, interpolator, changeState);
            RunAnimation(animation);
            return animation;
        }

        public static Animation ParallelAnimation(Animation[] animations)
            => new ParallelAnimation(animations);
        public static Animation ParallelAnimation<TState, TProps>(this Component<TState, TProps> component, Animation[] animations) where TProps : Props
                                                                                                                                    where TState : IEquatable<TState>
            => ParallelAnimation(animations);

        public static Animation QueueAnimation(Animation[] animations)
            => new QueueAnimation(animations);
        public static Animation QueueAnimation<TState, TProps>(this Component<TState, TProps> component, Animation[] animations) where TProps : Props
                                                                                                                                    where TState : IEquatable<TState>
            => QueueAnimation(animations);

        public static Animation RunParallelAnimation<TState, TProps>(this Component<TState, TProps> component, Animation[] animations) where TProps : Props
                                                                                                                                    where TState : IEquatable<TState>
        {
            var animation = ParallelAnimation(animations);
            RunAnimation(animation);
            return animation;
        }
       
        public static Animation RunQueueAnimation<TState, TProps>(this Component<TState, TProps> component, Animation[] animations) where TProps : Props
                                                                                                                                    where TState : IEquatable<TState>
        {
            var animation = QueueAnimation(animations);
            RunAnimation(animation);
            return animation;
        }
            
    }
}
