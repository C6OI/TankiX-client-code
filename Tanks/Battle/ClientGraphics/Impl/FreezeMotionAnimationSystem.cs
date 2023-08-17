using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class FreezeMotionAnimationSystem : ECSSystem {
        [OnEventFire]
        public void Init(NodeAddedEvent evt, InitialFreezeMotionAnimationNode weapon) {
            weapon.freezeMotionAnimation.Init(weapon.animation.Animator, weapon.streamWeaponEnergy.ReloadEnergyPerSec);
            weapon.Entity.AddComponent<FreezeMotionAnimationReadyComponent>();
        }

        [OnEventFire]
        public void SwitchState(NodeAddedEvent evt, WorkingFreezeMotionAnimationNode weapon) =>
            weapon.freezeMotionAnimation.StartWorking(weapon.weaponEnergy.Energy);

        [OnEventFire]
        public void SwitchState(NodeAddedEvent evt, IdleFreezeMotionAnimationNode weapon) =>
            weapon.freezeMotionAnimation.StartIdle(weapon.weaponEnergy.Energy);

        [OnEventFire]
        public void StopMotion(NodeRemoveEvent evt, ActiveTankNode tank,
            [JoinByTank] ReadyFreezeMotionAnimationNode weapon) {
            weapon.Entity.RemoveComponent<FreezeMotionAnimationReadyComponent>();
            weapon.freezeMotionAnimation.StopMotion();
        }

        public class InitialFreezeMotionAnimationNode : Node {
            public AnimationComponent animation;

            public AnimationPreparedComponent animationPrepared;
            public FreezeMotionAnimationComponent freezeMotionAnimation;

            public StreamWeaponEnergyComponent streamWeaponEnergy;
        }

        public class ReadyFreezeMotionAnimationNode : Node {
            public AnimationComponent animation;

            public AnimationPreparedComponent animationPrepared;

            public FreezeMotionAnimationComponent freezeMotionAnimation;

            public FreezeMotionAnimationReadyComponent freezeMotionAnimationReady;
            public TankGroupComponent tankGroup;
        }

        public class WorkingFreezeMotionAnimationNode : Node {
            public AnimationComponent animation;

            public AnimationPreparedComponent animationPrepared;
            public FreezeMotionAnimationComponent freezeMotionAnimation;

            public FreezeMotionAnimationReadyComponent freezeMotionAnimationReady;

            public StreamWeaponWorkingComponent streamWeaponWorking;

            public WeaponEnergyComponent weaponEnergy;
        }

        public class IdleFreezeMotionAnimationNode : Node {
            public AnimationComponent animation;

            public AnimationPreparedComponent animationPrepared;
            public FreezeMotionAnimationComponent freezeMotionAnimation;

            public FreezeMotionAnimationReadyComponent freezeMotionAnimationReady;

            public StreamWeaponIdleComponent streamWeaponIdle;

            public WeaponEnergyComponent weaponEnergy;
        }

        public class ActiveTankNode : Node {
            public TankActiveStateComponent tankActiveState;

            public TankGroupComponent tankGroup;
        }
    }
}