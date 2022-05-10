using CSX.Animations;
using CSX.Animations.Interpolators;
using CSX.Components;
using static BlazorApp1.ComponentFunctions;

namespace BlazorApp1;

public record TestProps : Props
{
    public string? Name { get; init; }
    public string? LastName { get; init; }
}
public record ComponentTestState
{
    public double FontSize { get; init; } = 50;
    public double LastNameFontSize { get; init; } = 50;
}
public partial class ComponentTest : Component<ComponentTestState, TestProps>
{
    protected override ComponentTestState OnInitialize()
    {        
        this.RunParallelAnimation(new[] {
            this.ValueAnimation(1000, 50, 500, new BounceInterpolator(), (s, v) => s with { FontSize = v}),
            this.ValueAnimation(100, 50, 300, new DecelerateInterpolator(), (s, v) => s with { LastNameFontSize = v})
        });

        return new();           
    }

    protected override Element Render()
    {        
        return View(new() { Style = new() { Flex=1 } }, new[]
        {
            Text(new(){ Style=new(){ Color="#FF0000", FontSize=State.FontSize } }, new[]
            {
                String(Props.Name)
            }),
            Text(new(){ Style=new() { FontSize=State.LastNameFontSize } }, new[]
            {
                String(Props.LastName)
            }),
            View(new(){ Style = new(){ Flex=1, BackgroundColor = "powderblue" }}),
            View(new(){ Style = new(){ Flex=2, BackgroundColor = "skyblue" }}),
            View(new(){ Style = new(){ Flex=3, BackgroundColor = "steelblue" }}),
        });
    }


}


