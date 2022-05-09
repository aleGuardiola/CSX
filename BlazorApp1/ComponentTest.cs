using CSX.Components;
using System.Timers;
using static BlazorApp1.ComponentFunctions;

namespace BlazorApp1;

public record TestProps : Props
{
    public string? Name { get; init; }
    public string? LastName { get; init; }
}
public record ComponentTestState
{
    public bool IsRed { get; init; } = true;
}
public partial class ComponentTest : Component<ComponentTestState, TestProps>
{
    protected override ComponentTestState OnInitialize()
    {
        Func<Task>? timer = null;
        timer = async () =>
        {
            await Task.Delay(250);
            SetState(State with { IsRed = !State.IsRed });
            Task.Run(timer);
        };

        Task.Run(timer);

        return new();           
    }

    protected override Element Render()
    {
        var views = State.IsRed ? new[]
        {
            View(new() { Style = new() { BackgroundColor = State.IsRed ? "#FF0000" : "#0000FF", Width = 100, Height = 100 } }),
            View(new() { Style = new() { BackgroundColor = !State.IsRed ? "#FF0000" : "#0000FF", Width = 100, Height = 100 } }),
            View(new() { Style = new() { BackgroundColor = State.IsRed ? "#FF0000" : "#0000FF", Width = 100, Height = 100 } }),
            View(new() { Style = new() { BackgroundColor = !State.IsRed ? "#FF0000" : "#0000FF", Width = 100, Height = 100 } }),
            View(new() { Style = new() { BackgroundColor = State.IsRed ? "#FF0000" : "#0000FF", Width = 100, Height = 100 } }),
            View(new() { Style = new() { BackgroundColor = !State.IsRed ? "#FF0000" : "#0000FF", Width = 100, Height = 100 } }),
            View(new() { Style = new() { BackgroundColor = State.IsRed ? "#FF0000" : "#0000FF", Width = 100, Height = 100 } }),
            View(new() { Style = new() { BackgroundColor = !State.IsRed ? "#FF0000" : "#0000FF", Width = 100, Height = 100 } }),
            View(new() { Style = new() { BackgroundColor = State.IsRed ? "#FF0000" : "#0000FF", Width = 100, Height = 100 } }),
            View(new() { Style = new() { BackgroundColor = !State.IsRed ? "#FF0000" : "#0000FF", Width = 100, Height = 100 } }),
            View(new() { Style = new() { BackgroundColor = State.IsRed ? "#FF0000" : "#0000FF", Width = 100, Height = 100 } }),
            View(new() { Style = new() { BackgroundColor = !State.IsRed ? "#FF0000" : "#0000FF", Width = 100, Height = 100 } }),
        } : new[]
        {
            View(new() { Style = new() { BackgroundColor = State.IsRed ? "#FF0000" : "#0000FF", Width = 100, Height = 100 } }),
            View(new() { Style = new() { BackgroundColor = !State.IsRed ? "#FF0000" : "#0000FF", Width = 100, Height = 100 } }),
            View(new() { Style = new() { BackgroundColor = State.IsRed ? "#FF0000" : "#0000FF", Width = 100, Height = 100 } }),
            View(new() { Style = new() { BackgroundColor = !State.IsRed ? "#FF0000" : "#0000FF", Width = 100, Height = 100 } }),
            View(new() { Style = new() { BackgroundColor = State.IsRed ? "#FF0000" : "#0000FF", Width = 100, Height = 100 } }),
            View(new() { Style = new() { BackgroundColor = !State.IsRed ? "#FF0000" : "#0000FF", Width = 100, Height = 100 } }),
        };

        return         
        View(new(), views);
    }


}


