using Platform.Kernel.ECS.ClientEntitySystem.API;

namespace Tanks.Battle.ClientCore.API {
    public class HealthSystem : ECSSystem {
        [OnEventComplete]
        public void TankGetHit(HealthChangedEvent e, SelfTankWithLastHealth selfTank) {
            selfTank.lastHealth.LastHealth = selfTank.health.CurrentHealth;
        }

        [OnEventFire]
        public void AddLastHealth(NodeAddedEvent e, SelfTankNode selfTank) {
            selfTank.Entity.AddComponent<LastHealthComponent>();
        }

        [OnEventFire]
        public void RemoveLastHealth(NodeRemoveEvent e, SelfTankNode selfTank) {
            selfTank.Entity.RemoveComponent<LastHealthComponent>();
        }

        public class SelfTankNode : Node {
            public HealthComponent health;
            public SelfTankComponent selfTank;
        }

        public class SelfTankWithLastHealth : SelfTankNode {
            public LastHealthComponent lastHealth;
        }
    }
}