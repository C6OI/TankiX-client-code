using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class FlamethrowerMotionAnimationSystem : ECSSystem {
        [OnEventFire]
        public void InitFlamethrowerMotion(NodeAddedEvent evt, InitialFlamethrowerMotionNode weapon) {
            weapon.flamethrowerMotionAnimation.Init(weapon.animation.Animator);
            weapon.Entity.AddComponent<FlamethrowerMotionAnimationReadyComponent>();
        }

        [OnEventFire]
        public void StartWorkingMotion(NodeAddedEvent evt, WorkingFlamethrowerMotionNode weapon) {
            weapon.flamethrowerMotionAnimation.StartWorkingMotion();
        }

        [OnEventFire]
        public void StartIdleMotion(NodeAddedEvent evt, IdleReloadingFlamethrowerMotionNode weapon) {
            weapon.flamethrowerMotionAnimation.StartIdleMotion();
        }

        [OnEventFire]
        public void StopMotion(NodeAddedEvent evt, IdleReloadedFlamethrowerMotionNode weapon) {
            weapon.flamethrowerMotionAnimation.StopMotion();
        }

        [OnEventFire]
        public void StopMotion(NodeRemoveEvent evt, ActiveTankNode tank, [JoinByTank] ReadyFlamethrowerMotionNode weapon) {
            weapon.flamethrowerMotionAnimation.StopMotion();
        }

        public class InitialFlamethrowerMotionNode : Node {
            public AnimationComponent animation;

            public AnimationPreparedComponent animationPrepared;
            public FlamethrowerMotionAnimationComponent flamethrowerMotionAnimation;
        }

        public class ReadyFlamethrowerMotionNode : Node {
            public AnimationComponent animation;

            public AnimationPreparedComponent animationPrepared;
            public FlamethrowerMotionAnimationComponent flamethrowerMotionAnimation;

            public FlamethrowerMotionAnimationReadyComponent flamethrowerMotionAnimationReady;

            public TankGroupComponent tankGroup;
        }

        public class IdleReloadingFlamethrowerMotionNode : Node {
            public AnimationComponent animation;

            public AnimationPreparedComponent animationPrepared;

            public FlamethrowerMotionAnimationComponent flamethrowerMotionAnimation;

            public FlamethrowerMotionAnimationReadyComponent flamethrowerMotionAnimationReady;
            public StreamWeaponIdleComponent streamWeaponIdle;

            public WeaponEnergyComponent weaponEnergy;

            public WeaponEnergyReloadingStateComponent weaponEnergyReloadingState;
        }

        public class IdleReloadedFlamethrowerMotionNode : Node {
            public AnimationComponent animation;

            public AnimationPreparedComponent animationPrepared;

            public FlamethrowerMotionAnimationComponent flamethrowerMotionAnimation;

            public FlamethrowerMotionAnimationReadyComponent flamethrowerMotionAnimationReady;
            public StreamWeaponIdleComponent streamWeaponIdle;

            public WeaponEnergyComponent weaponEnergy;

            public WeaponEnergyFullStateComponent weaponEnergyFullState;
        }

        public class WorkingFlamethrowerMotionNode : Node {
            public AnimationComponent animation;

            public AnimationPreparedComponent animationPrepared;

            public FlamethrowerMotionAnimationComponent flamethrowerMotionAnimation;

            public FlamethrowerMotionAnimationReadyComponent flamethrowerMotionAnimationReady;
            public StreamWeaponWorkingComponent streamWeaponWorking;
        }

        public class ActiveTankNode : Node {
            public TankActiveStateComponent tankActiveState;

            public TankGroupComponent tankGroup;
        }
    }
}