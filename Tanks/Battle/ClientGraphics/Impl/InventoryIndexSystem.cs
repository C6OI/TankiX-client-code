using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.Impl;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class InventoryIndexSystem : ECSSystem {
        [OnEventFire]
        public void SetIndexOnInventoryAdded(NodeAddedEvent e, SingleNode<RepairInventoryComponent> node) =>
            node.Entity.AddComponent(new IndexComponent(0));

        [OnEventFire]
        public void SetIndexOnInventoryAdded(NodeAddedEvent e, SingleNode<ArmorInventoryComponent> node) =>
            node.Entity.AddComponent(new IndexComponent(1));

        [OnEventFire]
        public void SetIndexOnInventoryAdded(NodeAddedEvent e, SingleNode<DamageInventoryComponent> node) =>
            node.Entity.AddComponent(new IndexComponent(2));

        [OnEventFire]
        public void SetIndexOnInventoryAdded(NodeAddedEvent e, SingleNode<SpeedInventoryComponent> node) =>
            node.Entity.AddComponent(new IndexComponent(3));

        [OnEventFire]
        public void SetIndexOnInventoryAdded(NodeAddedEvent e, SingleNode<MineInventoryComponent> node) =>
            node.Entity.AddComponent(new IndexComponent(4));
    }
}