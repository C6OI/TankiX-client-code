using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class DurationSupplyPropertySystem : ECSSystem {
        [OnEventFire]
        public void ShowProperty(ShowItemPropertiesEvent e, DurationPropertyNode durationProperty,
            [JoinBySupply] SingleNode<SupplyCountComponent> item,
            [JoinByScreen] SingleNode<ItemPropertiesScreenComponent> screen) {
            long num = durationProperty.durationConfig.Duration / 1000;
            screen.component.ShowPropertyAndSetValue<DurationSupplyPropertyComponent>(num);
        }

        public class DurationPropertyNode : Node {
            public DurationConfigComponent durationConfig;

            public SupplyGroupComponent supplyGroup;
            public SupplyPropertyComponent supplyProperty;
        }
    }
}