using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Lobby.ClientUserProfile.API {
    [Shared]
    [SerialVersionUID(7453043498913563889L)]
    public class UserGroupComponent : GroupComponent {
        public UserGroupComponent(Entity keyEntity)
            : base(keyEntity) { }

        public UserGroupComponent(long key)
            : base(key) { }
    }
}