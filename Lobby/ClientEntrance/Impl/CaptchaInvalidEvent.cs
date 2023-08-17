using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientEntrance.Impl {
    [Shared]
    [SerialVersionUID(1442566222029L)]
    public class CaptchaInvalidEvent : Event { }
}