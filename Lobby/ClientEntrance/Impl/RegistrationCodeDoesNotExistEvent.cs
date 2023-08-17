using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientEntrance.Impl {
    [Shared]
    [SerialVersionUID(1471952521158L)]
    public class RegistrationCodeDoesNotExistEvent : Event { }
}