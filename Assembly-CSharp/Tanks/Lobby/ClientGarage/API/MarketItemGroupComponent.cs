using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientProtocol.API;

namespace Tanks.Lobby.ClientGarage.API {
    [Shared]
    [SerialVersionUID(63290793489633843L)]
    public class MarketItemGroupComponent : GroupComponent {
        public MarketItemGroupComponent(Entity keyEntity)
            : base(keyEntity) { }

        public MarketItemGroupComponent(long key)
            : base(key) { }
    }
}