using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;

namespace Tanks.Battle.ClientCore.Impl {
    public class VulcanGyroscopeSystem : ECSSystem {
        [OnEventFire]
        public void SyncGyroscopePower(UpdateEvent evt, VulcanGyroscopeNode vulcanGyroscope) {
            float state = vulcanGyroscope.vulcanWeaponState.State;
            vulcanGyroscope.weaponGyroscopeRotation.GyroscopePower = state;
        }

        [OnEventFire]
        public void DisableGyroscope(NodeRemoveEvent e, VulcanGyroscopeNode vulcanGyroscope) =>
            vulcanGyroscope.weaponGyroscopeRotation.GyroscopePower = 0f;

        [OnEventFire]
        public void AddGyroscopeComponent(NodeAddedEvent evt, VulcanStartSpeedUpNode vulcanSpeedUp,
            [JoinByTank] ActiveTankNode selfActiveTank) =>
            SetupGyroscope(vulcanSpeedUp, vulcanSpeedUp.vulcanWeaponState.State);

        static void SetupGyroscope(VulcanNode vulcan, float state) {
            VulcanGyroscopeEnabledComponent component = new();
            vulcan.Entity.AddComponent(component);
            vulcan.weaponGyroscopeRotation.GyroscopePower = state;
        }

        [OnEventFire]
        public void AddGyroscopeComponentShootingState(NodeAddedEvent evt,
            VulcanShootingWithoutGyroscopeEnabledNode vulcanShooting, [JoinByTank] ActiveTankNode selfActiveTank) =>
            SetupGyroscope(vulcanShooting, vulcanShooting.vulcanWeaponState.State);

        [OnEventFire]
        public void RemoveGyroscopeComponent(NodeRemoveEvent evt, VulcanSlowDownForNRNode nr,
            [JoinSelf] VulcanSlowDownNode vulcan) {
            Entity entity = vulcan.Entity;
            entity.RemoveComponent<VulcanGyroscopeEnabledComponent>();
        }

        public class VulcanNode : Node {
            public VulcanWeaponStateComponent vulcanWeaponState;

            public WeaponGyroscopeRotationComponent weaponGyroscopeRotation;
        }

        public class VulcanGyroscopeNode : VulcanNode {
            public VulcanGyroscopeEnabledComponent vulcanGyroscopeEnabled;
        }

        public class VulcanStartSpeedUpNode : VulcanNode {
            public TankGroupComponent tankGroup;
            public VulcanSpeedUpComponent vulcanSpeedUp;
        }

        [Not(typeof(VulcanGyroscopeEnabledComponent))]
        public class VulcanShootingWithoutGyroscopeEnabledNode : VulcanNode {
            public TankGroupComponent tankGroup;
            public VulcanShootingComponent vulcanShooting;
        }

        public class VulcanSlowDownNode : Node {
            public VulcanGyroscopeEnabledComponent vulcanGyroscopeEnabled;
            public VulcanSlowDownComponent vulcanSlowDown;

            public VulcanWeaponStateComponent vulcanWeaponState;
        }

        public class VulcanSlowDownForNRNode : Node {
            public VulcanSlowDownComponent vulcanSlowDown;

            public VulcanWeaponStateComponent vulcanWeaponState;
        }

        public class ActiveTankNode : Node {
            public TankActiveStateComponent tankActiveState;

            public TankGroupComponent tankGroup;
        }
    }
}