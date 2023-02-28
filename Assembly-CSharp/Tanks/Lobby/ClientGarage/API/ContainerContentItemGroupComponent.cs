using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.API {
    public class ContainerContentItemGroupComponent : GroupComponent {
        public ContainerContentItemGroupComponent(Entity keyEntity)
            : base(keyEntity) { }

        public ContainerContentItemGroupComponent(long key)
            : base(key) { }
    }
}