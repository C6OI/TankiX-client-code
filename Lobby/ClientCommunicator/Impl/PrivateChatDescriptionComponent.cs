using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientCommunicator.Impl {
    [Shared]
    [SerialVersionUID(1451454136065L)]
    public class PrivateChatDescriptionComponent : Component {
        public Entity UserA { get; set; }

        public Entity UserB { get; set; }
    }
}