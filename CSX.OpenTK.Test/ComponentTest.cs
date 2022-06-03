using CSX.Animations.Interpolators;
using CSX.Components;
using CSX.Animations;
using static CSX.ComponentFunctions;
using CSX.NativeComponents;
using System.Drawing;

namespace CSX.OpenTK.Test;

public record ComponentTestState
{    
    public float LastElementFlex { get; init; } = 5f;
    public int Alpha = 255;
}
public class ComponentTest : Component<ComponentTestState, ViewProps>
{
    void RunAnimation()
    {
        this.RunParallelAnimation(new[] {
            this.ValueAnimation(3000, 1f, 5f, new BounceInterpolator(), (state, v) => state with { LastElementFlex = v }),
            this.ValueAnimation(3000, 0, 255, new BounceInterpolator(), (state, v) => state with { Alpha = Math.Min((int)v, 255) }),
        });
    }

    protected override Element Render()
    {
        return View(new() { Style = new() { Flex = 1, } }, new()
        {
            View(new(){ Style = new(){ Flex = State.LastElementFlex, BackgroundColor = Color.FromArgb(State.Alpha, Color.PowderBlue), JustifyContent = JustifyContent.Center, AlignItems = AlignItems.Center } }, new()
            {
                Text(new(){ Text = "Hello world CSX", Style=new(){ }, OnPress=(e) => RunAnimation() })
            }),
            View(new(){ Style = new(){ Flex = 1f, BackgroundColor = Color.FromArgb(State.Alpha, Color.SkyBlue) } }),
            View(new(){ Style = new(){ Flex = 1f, BackgroundColor = Color.FromArgb(State.Alpha, Color.SteelBlue) } }),            
        });
    }



}


