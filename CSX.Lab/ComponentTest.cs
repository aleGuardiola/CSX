using CSX.Components;
using static CSX.ComponentFunctions;

namespace BlazorApp1;

public record TestProps : Props
{
    public string? Name { get; init; }
    public string? LastName { get; init; }
}
public record ComponentTestState
{
    
}
public partial class ComponentTest : Component<ComponentTestState, TestProps>
{

    protected override ComponentTestState OnInitialize()
    {
        return new();           
    }

    protected override Element Render()
    {

        var newPorps = Props with { LastName = "Jordano" };

        return View(new() { Style = new() { Flex = 1 } }, new()
        {
            Text(new(){ Text=Props.Name }),
            Text()
        });
    }

    

}


