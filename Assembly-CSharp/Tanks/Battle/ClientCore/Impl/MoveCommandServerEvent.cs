using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientCore.Impl {
    [Shared]
    [SerialVersionUID(-4956413533647444536L)]
    public class MoveCommandServerEvent : Event {
        public MoveCommand MoveCommand { get; set; }
    }
}