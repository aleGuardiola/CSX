using CSX.Animations.Interpolators;
using CSX.Components;
using CSX.Animations;
using static CSX.ComponentFunctions;
using CSX.NativeComponents;
using System.Drawing;
using CSX.Styling;

namespace CSX.OpenTK.Test;

public record ComponentTestState
{    
    public float LastElementFlex { get; init; } = 5f;
    public int Alpha = 255;
}
public class ComponentTest : Component<ComponentTestState, ViewProps>
{

    protected override void OnViewInit()
    {
        RunAnimation();
        base.OnViewInit();
    }

    void RunAnimation()
    {
        this.RunValueAnimation(5000, 0f, 5f, new AccelerateDecelerateInterpolator(), (state, v) => state with { LastElementFlex = v });        
    }

    protected override Element Render()
    {        
        return View(new() { Style = new() { Flex = 1, } }, new()
        {
            View(new(){ Style = new(){ Flex = State.LastElementFlex, BackgroundColor = Color.FromArgb(State.Alpha, Color.PowderBlue), JustifyContent = JustifyContent.Center, AlignItems = AlignItems.Center } }, new()
            {
                Text(new(){ Text = "Welcome to CSX", OnPress=(e) => RunAnimation() })
            }),
            View(new(){ Style = ComponentTestStyles.Styles.Apply(x => x.View) }),
            View(new(){ Style = ComponentTestStyles.Styles.Apply(x => x.View) }),
        });
    }    

    public class ComponentTestStyles
    {
        public ViewStyleProps? View { get; init; }
        

        public static StyleSheet<ComponentTestStyles> Styles = StyleSheet.Create(new ComponentTestStyles()
        {
            View = new()
            {
                Flex = 1f,
                BackgroundColor = Color.SkyBlue
            }
        });
    }

}


