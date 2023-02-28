using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [Shared]
    [SerialVersionUID(1485840793372L)]
    public class UserItemGroupComponent : GroupComponent {
        public UserItemGroupComponent(Entity keyEntity)
            : base(keyEntity) { }

        public UserItemGroupComponent(long key)
            : base(key) { }
    }
}