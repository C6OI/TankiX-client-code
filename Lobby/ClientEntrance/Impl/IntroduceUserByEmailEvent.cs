using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientEntrance.Impl {
    [Shared]
    [SerialVersionUID(1458846544326L)]
    public class IntroduceUserByEmailEvent : IntroduceUserEvent {
        public string Email { get; set; }
    }
}