using CSX.Components;
using CSX.Compilation;

namespace CSX.Lab
{
    public record TestProps : Props
    {

    }  
    public partial class ComponentTest : Component<ComponentTest.CState, TestProps>
    {
        public record CState(
            string? Name,
            string? LastName
            );

        protected override CState OnInitialize()
        {
            return new(
                Name: "Alejandro",
                LastName: "Guardiola"
                );
        }

    }
}
