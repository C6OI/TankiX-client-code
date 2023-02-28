using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class InventorySlotTemporaryBlockSystem : ECSSystem {
        [OnEventFire]
        public void SetInventoryAsTemporaryBlocked(NodeAddedEvent e, SingleNode<InventorySlotTemporaryBlockedByServerComponent> node) {
            node.Entity.AddComponentIfAbsent<InventorySlotTemporaryBlockedComponent>();
        }

        [OnEventFire]
        public void SetInventoryAsTemporaryBlocked(NodeAddedEvent e, SingleNode<InventorySlotTemporaryBlockedByClientComponent> node) {
            node.Entity.AddComponentIfAbsent<InventorySlotTemporaryBlockedComponent>();
        }

        [OnEventFire]
        public void CheckInventoryAsTemporaryBlocked(NodeRemoveEvent e, SingleNode<InventorySlotTemporaryBlockedByServerComponent> node) {
            if (!node.Entity.HasComponent<InventorySlotTemporaryBlockedByClientComponent>()) {
                node.Entity.RemoveComponentIfPresent<InventorySlotTemporaryBlockedComponent>();
            }
        }

        [OnEventFire]
        public void CheckInventoryAsTemporaryBlocked(NodeRemoveEvent e, SingleNode<InventorySlotTemporaryBlockedByClientComponent> node) {
            if (!node.Entity.HasComponent<InventorySlotTemporaryBlockedByServerComponent>()) {
                node.Entity.RemoveComponentIfPresent<InventorySlotTemporaryBlockedComponent>();
            }
        }

        [OnEventFire]
        public void CleanUpClientBlockFromSlot(NodeRemoveEvent e, SingleNode<TankGroupComponent> node, [JoinSelf] SingleNode<InventorySlotTemporaryBlockedByClientComponent> slot) {
            slot.Entity.RemoveComponent<InventorySlotTemporaryBlockedByClientComponent>();
        }
    }
}