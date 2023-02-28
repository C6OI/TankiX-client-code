using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Platform.Library.ClientUnityIntegration.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Battle.ClientHUD.Impl {
    public class ItemButtonAmmunitionSystem : ECSSystem {
        [OnEventFire]
        public void InventoryAmmunitionChanged(InventoryAmmunitionChangedEvent e, SingleNode<InventoryAmmunitionComponent> node, [JoinByModule] SingleNode<ItemButtonComponent> hud,
            [JoinByModule] Optional<UserItemCounterNode> userItemCounter, [JoinByModule] Optional<SlotCooldownStateNode> slotCooldownNode) {
            if (!userItemCounter.IsPresent() || userItemCounter.IsPresent() && userItemCounter.Get().userItemCounter.Count > 0) {
                hud.component.ItemAmmunitionCount = node.component.CurrentCount;

                if (slotCooldownNode.IsPresent()) {
                    StartCooldown(slotCooldownNode.Get(), hud.component);
                }
            }
        }

        void StartCooldown(SlotCooldownStateNode slot, ItemButtonComponent item) {
            float timeInSec = slot.inventoryCooldownState.CooldownTime / 1000f - (Date.Now.UnityTime - slot.inventoryCooldownState.CooldownStartTime.UnityTime);

            if (!item.isRage) {
                item.StartCooldown(timeInSec, slot.Entity.HasComponent<InventoryEnabledStateComponent>());
            } else {
                item.StartRageCooldown(timeInSec, slot.Entity.HasComponent<InventoryEnabledStateComponent>());
            }
        }

        public class SlotCooldownStateNode : Node {
            public InventoryCooldownStateComponent inventoryCooldownState;

            public ModuleGroupComponent moduleGroup;
            public SlotUserItemInfoComponent slotUserItemInfo;
        }

        public class UserItemCounterNode : Node {
            public UserItemCounterComponent userItemCounter;
        }
    }
}