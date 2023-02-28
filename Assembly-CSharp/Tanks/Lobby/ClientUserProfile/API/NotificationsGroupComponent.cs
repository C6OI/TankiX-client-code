using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientUserProfile.API {
    public class NotificationsGroupComponent : GroupComponent {
        public NotificationsGroupComponent(Entity keyEntity)
            : base(keyEntity) { }

        public NotificationsGroupComponent(long key)
            : base(key) { }
    }
}