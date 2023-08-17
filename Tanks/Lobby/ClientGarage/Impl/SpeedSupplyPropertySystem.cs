using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class SpeedSupplyPropertySystem : ECSSystem {
        [OnEventFire]
        public void ShowProperty(ShowItemPropertiesEvent e, SpeedPropertyNode speedProperty,
            [JoinBySupply] SingleNode<SupplyCountComponent> item,
            [JoinByScreen] SingleNode<ItemPropertiesScreenComponent> screen) {
            SpeedEffectComponent speedEffect = speedProperty.speedEffect;
            float value = (speedEffect.Coeff - 1f) * 100f;
            screen.component.ShowPropertyAndSetValue<SpeedSupplyPropertyComponent>(value);
            float value2 = (speedEffect.WeaponRotationSpeedMultiplier - 1f) * 100f;
            screen.component.ShowPropertyAndSetValue<WeaponRotationSpeedSupplyPropertyComponent>(value2);
        }

        public class SpeedPropertyNode : Node {
            public SpeedEffectComponent speedEffect;

            public SupplyGroupComponent supplyGroup;
            public SupplyPropertyComponent supplyProperty;
        }
    }
}