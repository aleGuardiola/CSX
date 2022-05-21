using CSX;
using CSX.Animations;
using CSX.Animations.Interpolators;
using CSX.Components;
using System.Drawing;
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
}
public partial class ComponentTest : Component<ComponentTestState, TestProps>
{
    protected override ComponentTestState OnInitialize()
    {
        return new();           
    }

    protected override Element Render()
    {

        return View(new() { Style = new() { Flex = 1 } }, new()
        {            
            Enumerable.Range(0, 1000).Select(num => 
            ListView<string>(new()
            {
                Key=num.ToString(),
                Style = new() { Height = 200 },
                Data = Enumerable.Range(0, 50000).Select(x => "Alejandro" + x).ToArray(),
                RenderItem = (name) => View(new() { Style = new() { Height = 30, BackgroundColor = Color.Red } }, new() { Text(new() { Text = name }) }),
                RowHeight = 30
            })).ToContent()
            
        });
    }

    

}


