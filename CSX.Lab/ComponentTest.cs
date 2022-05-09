using CSX.Components;
using static CSX.Lab.ComponentFunctions;

namespace CSX.Lab;

public record TestProps : Props
{
    public string? Name { get; init; }
    public string? LastName { get; init; }
}
public partial class ComponentTest : Component<TestProps>
{
    protected override Element Render()
    {
        return         
        View(new() { Style = new() { BackgroundColor = "f00" } }, new[]
        {
            Text(new() {  }, new[] {
                String(Props.Name ?? "")
            })
        });
    }


}


