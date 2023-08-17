using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientEntrance.Impl {
    [SerialVersionUID(1439375251389L)]
    [Shared]
    public class IntroduceUserByUidEvent : IntroduceUserEvent {
        public string Uid { get; set; }
    }
}