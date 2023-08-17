using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Battle.ClientRemoteServer.Impl {
    [SerialVersionUID(-4956413533647444536L)]
    [Shared]
    public class MoveCommandServerEvent : Event {
        public MoveCommand MoveCommand { get; set; }
    }
}