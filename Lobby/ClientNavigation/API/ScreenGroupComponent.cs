using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Lobby.ClientNavigation.API {
    public class ScreenGroupComponent : GroupComponent {
        public ScreenGroupComponent(Entity keyEntity)
            : base(keyEntity) { }

        public ScreenGroupComponent(long key)
            : base(key) { }
    }
}