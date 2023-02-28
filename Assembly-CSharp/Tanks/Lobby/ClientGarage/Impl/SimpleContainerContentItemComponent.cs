using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class SimpleContainerContentItemComponent : Component {
        public long MarketItemId { get; set; }

        public string NameLokalizationKey { get; set; }
    }
}