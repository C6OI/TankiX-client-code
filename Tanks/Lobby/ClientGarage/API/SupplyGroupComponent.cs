using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [Shared]
    [SerialVersionUID(1438936934029L)]
    public class SupplyGroupComponent : GroupComponent {
        public SupplyGroupComponent(Entity keyEntity)
            : base(keyEntity) { }

        public SupplyGroupComponent(long key)
            : base(key) { }
    }
}