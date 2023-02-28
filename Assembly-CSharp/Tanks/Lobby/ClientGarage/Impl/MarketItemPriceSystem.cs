using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class MarketItemPriceSystem : ECSSystem {
        [OnEventFire]
        public void ChangePackPrice(PackPriceChangedEvent e, MarketItemNode marketItem) {
            marketItem.packPrice.PackPrice = e.PackPrice;
            marketItem.packPrice.PackXPrice = e.PackXPrice;
        }

        public class MarketItemNode : Node {
            public MarketItemComponent marketItem;

            public PackPriceComponent packPrice;
        }
    }
}