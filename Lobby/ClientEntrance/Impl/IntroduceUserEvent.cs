using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientEntrance.Impl {
    public class IntroduceUserEvent : Event {
        [ProtocolOptional] public string Captcha { get; set; }
    }
}