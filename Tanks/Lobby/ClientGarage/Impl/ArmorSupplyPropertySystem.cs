using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class ArmorSupplyPropertySystem : ECSSystem {
        [OnEventFire]
        public void ShowProperty(ShowItemPropertiesEvent e, ArmorPropertyNode armorProperty,
            [JoinBySupply] SingleNode<SupplyCountComponent> item,
            [JoinByScreen] SingleNode<ItemPropertiesScreenComponent> screen) {
            float value = (armorProperty.armorEffect.ArmorCoefficient - 1f) * 100f;
            screen.component.ShowPropertyAndSetValue<ArmorSupplyPropertyComponent>(value);
        }

        public class ArmorPropertyNode : Node {
            public ArmorEffectComponent armorEffect;

            public SupplyGroupComponent supplyGroup;
            public SupplyPropertyComponent supplyProperty;
        }
    }
}