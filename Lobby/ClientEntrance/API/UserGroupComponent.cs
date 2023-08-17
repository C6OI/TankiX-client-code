using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientEntrance.API {
    [SerialVersionUID(7453043498913563889L)]
    [Shared]
    public class UserGroupComponent : GroupComponent {
        public UserGroupComponent(Entity keyEntity)
            : base(keyEntity) { }

        public UserGroupComponent(long key)
            : base(key) { }
    }
}