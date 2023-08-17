using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class DisplaySupplyPropertiesSystem : ECSSystem {
        [OnEventFire]
        public void ShowSupplyProperty(ShowItemPropertiesEvent e, SupplyItemNode item,
            [JoinBySupply] SingleNode<SupplyPropertyComponent> supplyProperty) =>
            ScheduleEvent<ShowItemPropertiesEvent>(supplyProperty);

        public class SupplyItemNode : Node {
            public GarageItemComponent garageItem;

            public SupplyGroupComponent supplyGroup;

            public UserItemComponent userItem;
        }
    }
}