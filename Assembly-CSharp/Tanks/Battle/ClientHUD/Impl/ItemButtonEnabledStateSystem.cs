using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Battle.ClientHUD.Impl {
    public class ItemButtonEnabledStateSystem : ECSSystem {
        [OnEventFire]
        public void EnterEnabledState(NodeAddedEvent e, [Combine] ItemButtonNode button, [Context] [JoinByModule] EnabledSlotWithModuleNode slotWithModule,
            [Context] [JoinByTank] HUDNodes.ActiveSelfTankNode self) {
            if (slotWithModule.Entity.HasComponent<InventorySlotTemporaryBlockedComponent>()) {
                button.itemButton.Disable();
            } else {
                button.itemButton.Enable();
            }
        }

        [OnEventFire]
        public void OnTankLeaveActiveState(NodeRemoveEvent e, HUDNodes.ActiveSelfTankNode self, [JoinAll] [Combine] ItemButtonNode button) {
            button.itemButton.Disable();
        }

        [OnEventFire]
        public void Enable(NodeRemoveEvent e, SingleNode<InventorySlotTemporaryBlockedComponent> inventory, [JoinByModule] ItemButtonNode button) {
            if (inventory.Entity.HasComponent<InventoryEnabledStateComponent>()) {
                button.itemButton.Enable();
            } else {
                button.itemButton.Disable();
            }
        }

        [OnEventFire]
        public void Disable(NodeAddedEvent e, SlotBlockedNode inventory, [Context] [JoinByModule] ItemButtonNode button) {
            button.itemButton.Disable();
        }

        public class ItemButtonNode : Node {
            public ItemButtonComponent itemButton;

            public ModuleGroupComponent moduleGroup;
        }

        public class SlotWithModuleNode : Node {
            public ModuleGroupComponent moduleGroup;
            public SlotUserItemInfoComponent slotUserItemInfo;
        }

        public class EnabledSlotWithModuleNode : SlotWithModuleNode {
            public InventoryEnabledStateComponent inventoryEnabledState;
        }

        public class SlotBlockedNode : SlotWithModuleNode {
            public InventorySlotTemporaryBlockedComponent inventorySlotTemporaryBlocked;
        }
    }
}