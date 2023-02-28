using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class RailgunAnimationSystem : ECSSystem {
        [OnEventFire]
        public void InitRailgunChargingAnimation(NodeAddedEvent evt, InitialRailgunAnimationNode weapon) {
            weapon.railgunAnimation.InitRailgunAnimation(weapon.animation.Animator, weapon.discreteWeaponEnergy.ReloadEnergyPerSec, weapon.railgunChargingWeapon.ChargingTime);
            weapon.Entity.AddComponent<RailgunAnimationReadyComponent>();
        }

        [OnEventFire]
        public void StartRailgunChargingAnimation(BaseRailgunChargingShotEvent evt, RailgunAnimationReadyNode weapon) {
            weapon.railgunAnimation.StartChargingAnimation();
        }

        [OnEventFire]
        public void StartReloading(NodeAddedEvent evt, SingleNode<WeaponEnergyReloadingStateComponent> reloading, [Context] [JoinByTank] RailgunAnimationReadyNode weapon) {
            weapon.railgunAnimation.StartReloading();
        }

        [OnEventFire]
        public void StopReloading(NodeAddedEvent evt, SingleNode<WeaponEnergyFullStateComponent> full, [Context] [JoinByTank] RailgunAnimationReadyNode weapon) {
            weapon.railgunAnimation.StopReloading();
        }

        [OnEventFire]
        public void StopAnyRailgunAnimation(NodeRemoveEvent evt, ActiveTankNode tank, [JoinByTank] RailgunAnimationReadyNode weapon) {
            weapon.railgunAnimation.StartChargingAnimation();
            weapon.animation.Animator.SetTrigger("shot");
        }

        public class InitialRailgunAnimationNode : Node {
            public AnimationComponent animation;

            public AnimationPreparedComponent animationPrepared;

            public DiscreteWeaponEnergyComponent discreteWeaponEnergy;
            public RailgunAnimationComponent railgunAnimation;

            public RailgunChargingWeaponComponent railgunChargingWeapon;

            public TankGroupComponent tankGroup;
        }

        public class RailgunAnimationReadyNode : Node {
            public AnimationComponent animation;
            public RailgunAnimationComponent railgunAnimation;

            public RailgunAnimationReadyComponent railgunAnimationReady;

            public TankGroupComponent tankGroup;
        }

        public class ActiveTankNode : Node {
            public TankActiveStateComponent tankActiveState;

            public TankGroupComponent tankGroup;
        }
    }
}