using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Battle.ClientHUD.Impl {
    public class ItemButtonPassiveStateSystem : ECSSystem {
        [OnEventFire]
        public void EnterEnabledState(NodeAddedEvent e, [Combine] ItemButtonNode button, [Context] [JoinByModule] PassiveModuleNode passiveModule,
            [Context] [JoinByModule] SlotWithModuleNode slot, [Context] [JoinByTank] HUDNodes.ActiveSelfTankNode self) {
            button.itemButton.Passive();
        }

        public class ItemButtonNode : Node {
            public ItemButtonComponent itemButton;

            public ModuleGroupComponent moduleGroup;
        }

        public class SlotWithModuleNode : Node {
            public InventoryEnabledStateComponent inventoryEnabledState;

            public ModuleGroupComponent moduleGroup;
            public SlotUserItemInfoComponent slotUserItemInfo;

            public TankGroupComponent tankGroup;
        }

        public class PassiveModuleNode : Node {
            public ModuleGroupComponent moduleGroup;

            public PassiveModuleComponent passiveModule;
        }
    }
}