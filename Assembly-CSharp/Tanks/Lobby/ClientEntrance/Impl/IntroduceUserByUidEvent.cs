using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientEntrance.Impl {
    [Shared]
    [SerialVersionUID(1439375251389L)]
    public class IntroduceUserByUidEvent : IntroduceUserEvent {
        public string Uid { get; set; }
    }
}