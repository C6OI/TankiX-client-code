using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientCommunicator.API {
    [Shared]
    [SerialVersionUID(1446186452500L)]
    public class ChatGroupComponent : GroupComponent {
        public ChatGroupComponent(Entity keyEntity)
            : base(keyEntity) { }

        public ChatGroupComponent(long key)
            : base(key) { }
    }
}