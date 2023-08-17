using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Lobby.ClientUserProfile.src.main.csharp.Impl.Notifications.Component {
    public class NotificationsGroupComponent : GroupComponent {
        public NotificationsGroupComponent(Entity keyEntity)
            : base(keyEntity) { }

        public NotificationsGroupComponent(long key)
            : base(key) { }
    }
}