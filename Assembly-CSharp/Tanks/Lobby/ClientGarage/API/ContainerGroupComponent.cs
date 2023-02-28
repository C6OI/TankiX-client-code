using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.API {
    public class ContainerGroupComponent : GroupComponent {
        public ContainerGroupComponent(Entity keyEntity)
            : base(keyEntity) { }

        public ContainerGroupComponent(long key)
            : base(key) { }
    }
}