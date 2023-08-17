using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Lobby.ClientGarage.API;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class EffectIndexSystem : ECSSystem {
        [OnEventFire]
        public void SetIndexOnEffectAdded(NodeAddedEvent e, SingleNode<RepairEffectComponent> node) =>
            node.Entity.AddComponent(new IndexComponent(0));

        [OnEventFire]
        public void SetIndexOnEffectAdded(NodeAddedEvent e, SingleNode<ArmorEffectComponent> node) =>
            node.Entity.AddComponent(new IndexComponent(1));

        [OnEventFire]
        public void SetIndexOnEffectAdded(NodeAddedEvent e, SingleNode<DamageEffectComponent> node) =>
            node.Entity.AddComponent(new IndexComponent(2));

        [OnEventFire]
        public void SetIndexOnEffectAdded(NodeAddedEvent e, SingleNode<SpeedEffectComponent> node) =>
            node.Entity.AddComponent(new IndexComponent(3));
    }
}