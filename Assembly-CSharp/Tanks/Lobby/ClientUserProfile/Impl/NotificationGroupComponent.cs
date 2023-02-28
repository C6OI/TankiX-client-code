using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientUserProfile.Impl {
    [SerialVersionUID(1486736500959L)]
    [Shared]
    public class NotificationGroupComponent : GroupComponent {
        public NotificationGroupComponent(Entity keyEntity)
            : base(keyEntity) { }

        public NotificationGroupComponent(long key)
            : base(key) { }
    }
}