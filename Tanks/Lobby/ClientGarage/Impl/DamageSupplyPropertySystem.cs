using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class DamageSupplyPropertySystem : ECSSystem {
        [OnEventFire]
        public void ShowProperty(ShowItemPropertiesEvent e, DamagePropertyNode damageProperty,
            [JoinBySupply] SingleNode<SupplyCountComponent> item,
            [JoinByScreen] SingleNode<ItemPropertiesScreenComponent> screen) {
            float value = (damageProperty.damageEffect.DamageCoefficient - 1f) * 100f;
            screen.component.ShowPropertyAndSetValue<DamageSupplyPropertyComponent>(value);
        }

        public class DamagePropertyNode : Node {
            public DamageEffectComponent damageEffect;

            public SupplyGroupComponent supplyGroup;
            public SupplyPropertyComponent supplyProperty;
        }
    }
}