using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientHUD.API;

namespace Tanks.Battle.ClientHUD.Impl {
    public class HealthBarSystem : ECSSystem {
        [OnEventFire]
        public void InitHealthBarProgressOnRemoteTanks(NodeAddedEvent e, RemoteTankNode tank, [Context] [JoinByTank] AttachedHealthBarNode healthBar) {
            UpdateHealth(tank.health, tank.healthConfig, healthBar.healthBar);
        }

        [OnEventFire]
        public void ChangeProgressValueOnAnyHealthBar(HealthChangedEvent e, TankNode tank, [JoinByTank] AttachedHealthBarNode healthBar) {
            UpdateHealth(tank.health, tank.healthConfig, healthBar.healthBar);
        }

        void UpdateHealth(HealthComponent health, HealthConfigComponent healthConfig, HealthBarComponent healthBar) {
            float progressValue = health.CurrentHealth / healthConfig.BaseHealth;
            healthBar.ProgressValue = progressValue;
        }

        public class TankNode : Node {
            public HealthComponent health;

            public HealthConfigComponent healthConfig;
            public TankComponent tank;

            public TankGroupComponent tankGroup;
        }

        public class RemoteTankNode : TankNode {
            public RemoteTankComponent remoteTank;
        }

        public class AttachedHealthBarNode : Node {
            public HealthBarComponent healthBar;

            public TankGroupComponent tankGroup;
        }

        public class TankIncarnationNode : Node {
            public TankGroupComponent tankGroup;
            public TankIncarnationComponent tankIncarnation;
        }
    }
}