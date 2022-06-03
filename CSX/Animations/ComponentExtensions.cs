using CSX.Animations.Interpolators;
using CSX.Components;

namespace CSX.Animations
{
    public static class ComponentExtensions
    {        
        public static void RunAnimation<TState, TProps>(this Component<TState, TProps> component, Animation animation) where TProps : Props
                                                                                                                       where TState : IEquatable<TState>
        {
            Action? runAnimation = null;
            runAnimation = () =>
            {
                component.RunOnUIThread((deltaTime) =>
                {
                    animation.Update((int)(deltaTime * 1000));
                    if (animation.IsPlaying)
                    {
                        runAnimation?.Invoke();
                    }
                }, true);
            };

            runAnimation();
        }
            

        public static Animation ValueAnimation<TState, TProps>(this Component<TState, TProps> component, int duration, float start, float end, Interpolator interpolator, Func<TState, float, TState> changeState) where TProps : Props
                                                                                                                                                                                                                         where TState : IEquatable<TState>        
            => new FloatStateAnimation<Component<TState, TProps>, TState, TProps>(duration, component, changeState, start, end, interpolator);

        public static Animation RunValueAnimation<TState, TProps>(this Component<TState, TProps> component, int duration, float start, float end, Interpolator interpolator, Func<TState, float, TState> changeState) where TProps : Props
                                                                                                                                                                                                                         where TState : IEquatable<TState>
        {
            var animation = ValueAnimation(component, duration, start, end, interpolator, changeState);
            RunAnimation(component, animation);
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
            RunAnimation(component, animation);
            return animation;
        }
       
        public static Animation RunQueueAnimation<TState, TProps>(this Component<TState, TProps> component, Animation[] animations) where TProps : Props
                                                                                                                                    where TState : IEquatable<TState>
        {
            var animation = QueueAnimation(animations);
            RunAnimation(component, animation);
            return animation;
        }
            
    }
}
