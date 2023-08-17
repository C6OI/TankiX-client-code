using Lobby.ClientNavigation.API;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Lobby.ClientGarage.Impl {
    public class MineSupplyPropertySystem : ECSSystem {
        [OnEventFire]
        public void ShowProperty(ShowItemPropertiesEvent e, MinePropertyNode mineProperty,
            [JoinBySupply] SingleNode<SupplyCountComponent> item,
            [JoinByScreen] SingleNode<ItemPropertiesScreenComponent> screen) {
            MineConfigComponent mineConfig = mineProperty.mineConfig;

            screen.component.ShowPropertyAndSetValue<MineDamageSupplyPropertyComponent>(mineConfig.DamageFrom,
                mineConfig.DamageTo);
        }

        public class MinePropertyNode : Node {
            public MineConfigComponent mineConfig;

            public SupplyGroupComponent supplyGroup;
            public SupplyPropertyComponent supplyProperty;
        }
    }
}