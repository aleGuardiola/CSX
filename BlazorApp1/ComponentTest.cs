using CSX.Animations.Interpolators;
using CSX.Components;
using CSX.Animations;
using static CSX.ComponentFunctions;

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
    public double FirstElementFlex { get; init; } = 1;
    public string InputText { get; init; } = "";
    public int[] ListViewCount { get; init; } = Enumerable.Range(1, 50000).ToArray();
    public string[] ListViewContent { get; init; } = Enumerable.Range(0, 50000).Select(x => "Alejandro" + x).ToArray();
    public double WelcomeFontSize { get; init; } = 50;
}
public partial class ComponentTest : Component<ComponentTestState, TestProps>
{

    protected override ComponentTestState OnInitialize()
    {
        
        return base.OnInitialize();
    }

    protected override void OnViewInit()
    {
        this.RunValueAnimation(3000, 50, 200, new BounceInterpolator(), (state, v) => state with { WelcomeFontSize = v });
    }

    protected override Element Render()
    {
        var listviews = ListView<int>(
            new()
            {
                Data = State.ListViewCount,
                RowHeight = 200,
                RenderItem = (v) => ListView<string>(new()
                {
                    Key = v.ToString(),
                    Style = new() { Height = 200 },
                    Data = State.ListViewContent,
                    RenderItem = (name) => View(new() { Style = new() { Height = 30 } }, new() { Text(new() { Text = name }) }),
                    RowHeight = 30
                }),
                Style = new() { Flex = 1, Height = 1000 }
            });
        return ScrollView(new() { Style = new() { Flex = 1 } }, new()
        {
            listviews,            
        });
    }

    

}


