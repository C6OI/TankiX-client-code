using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Battle.ClientHUD.Impl {
    public class ItemButtonCooldownStateSystem : ECSSystem {
        [OnEventFire]
        public void InitializeCooldownFill(NodeAddedEvent e, SlotCooldownStateNode slot, [Context] [JoinByModule] ItemButtonNode itemButton) {
            StartCooldown(slot, itemButton);
        }

        [OnEventFire]
        public void InitializeCooldownFill(NodeRemoveEvent e, SlotCooldownStateNode slot, [JoinByModule] ItemButtonNode itemButton) {
            itemButton.itemButton.FinishCooldown();

            if (slot.Entity.HasComponent<InventoryEnabledStateComponent>()) {
                itemButton.itemButton.Enable();
            } else {
                itemButton.itemButton.Disable();
            }
        }

        void StartCooldown(SlotCooldownStateNode slot, ItemButtonNode itemButton) {
            float timeInSec = slot.inventoryCooldownState.CooldownTime / 1000f - (Date.Now.UnityTime - slot.inventoryCooldownState.CooldownStartTime.UnityTime);

            if (!itemButton.itemButton.isRage) {
                itemButton.itemButton.StartCooldown(timeInSec, slot.Entity.HasComponent<InventoryEnabledStateComponent>());

                if (itemButton.itemButton.ammunitionCountWasIncreased && itemButton.itemButton.MaxItemAmmunitionCount > 1 && itemButton.itemButton.ItemAmmunitionCount == 1) {
                    itemButton.itemButton.FinishCooldown();
                }
            } else {
                itemButton.itemButton.StartRageCooldown(timeInSec, slot.Entity.HasComponent<InventoryEnabledStateComponent>());
            }
        }

        public class ItemButtonNode : Node {
            public ItemButtonComponent itemButton;

            public ModuleGroupComponent moduleGroup;
        }

        public class SlotCooldownStateNode : Node {
            public InventoryCooldownStateComponent inventoryCooldownState;

            public ModuleGroupComponent moduleGroup;
            public SlotUserItemInfoComponent slotUserItemInfo;
        }
    }
}