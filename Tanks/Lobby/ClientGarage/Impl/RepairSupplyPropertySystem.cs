using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class RepairSupplyPropertySystem : ECSSystem {
        [OnEventFire]
        public void ShowProperty(ShowItemPropertiesEvent e, RepairPropertyNode repairProperty,
            [JoinBySupply] SingleNode<SupplyCountComponent> item,
            [JoinByScreen] SingleNode<ItemPropertiesScreenComponent> screen) {
            float value = repairProperty.durationConfig.Duration * repairProperty.repairEffectConfig.HpPerMs;
            screen.component.ShowPropertyAndSetValue<RepairSupplyPropertyComponent>(value);
        }

        public class RepairPropertyNode : Node {
            public DurationConfigComponent durationConfig;

            public RepairEffectConfigComponent repairEffectConfig;

            public SupplyGroupComponent supplyGroup;
            public SupplyPropertyComponent supplyProperty;
        }
    }
}