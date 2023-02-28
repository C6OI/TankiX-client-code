using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Lobby.ClientSettings.API;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class CartridgeCaseLifeSystem : ECSSystem {
        [OnEventFire]
        public void OnMapInstanceAppear(NodeAddedEvent e, SingleNode<MapInstanceComponent> mapInstance, [JoinAll] SingleNode<CartridgeCaseSettingComponent> settings) {
            int currentCartridgeCaseAmount = GraphicsSettings.INSTANCE.CurrentCartridgeCaseAmount;
            settings.component.MaximalCartridgeCount = currentCartridgeCaseAmount * 30;
            settings.component.DefaultMode = currentCartridgeCaseAmount <= 0;
        }

        [OnEventFire]
        public void OnCartridgeAppear(NodeAddedEvent e, SingleNode<CartridgeCaseComponent> component, [JoinAll] SingleNode<CartridgeCaseContainerComponent> container,
            [JoinAll] SingleNode<CartridgeCaseSettingComponent> settings) {
            if (settings.component.DefaultMode) {
                component.component.StartSelfDestruction();
                return;
            }

            while (container.component.Cartridges.Count > 0 && container.component.Cartridges.Count >= settings.component.MaximalCartridgeCount) {
                container.component.Cartridges.Dequeue().RecycleObject();
            }

            container.component.Cartridges.Enqueue(component.component.gameObject);
        }

        [OnEventFire]
        public void CleanOnUserLeaveBattle(NodeRemoveEvent e, SingleNode<SelfBattleUserComponent> selfUser, [JoinAll] SingleNode<CartridgeCaseContainerComponent> container,
            [JoinAll] SingleNode<CartridgeCaseSettingComponent> settings) {
            if (!settings.component.DefaultMode) {
                container.component.Cartridges.Clear();
            }
        }
    }
}