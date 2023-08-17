using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class VulcanTurbineAnimationSystem : ECSSystem {
        [OnEventFire]
        public void InitVulcanTurbineAnimation(NodeAddedEvent evt, InitialVulcanTurbineAnimationNode weapon) {
            Animator animator = weapon.animation.Animator;
            float speedUpTime = weapon.vulcanWeapon.SpeedUpTime;
            float slowDownTime = weapon.vulcanWeapon.SlowDownTime;
            weapon.vulcanTurbineAnimation.Init(animator, speedUpTime, slowDownTime);
            weapon.Entity.AddComponent<VulcanTurbineAnimationReadyComponent>();
        }

        [OnEventFire]
        public void StartSpeedUp(NodeAddedEvent evt, VulcanSpeedUpNode speedUpState,
            [Context] [JoinByTank] ReadyVulcanTurbineAnimationNode weapon, [Context] [JoinByTank] ActiveTankNode tank) =>
            weapon.vulcanTurbineAnimation.StartSpeedUp();

        [OnEventFire]
        public void StartSlowDown(NodeAddedEvent evt, VulcanSlowDownNode slowDownState,
            [Context] [JoinByTank] ReadyVulcanTurbineAnimationNode weapon, [Context] [JoinByTank] ActiveTankNode tank) =>
            weapon.vulcanTurbineAnimation.StartSlowDown();

        [OnEventFire]
        public void StartShooting(NodeAddedEvent evt, VulcanShootingNode shootingState,
            [JoinByTank] [Context] ReadyVulcanTurbineAnimationNode weapon, [JoinByTank] [Context] ActiveTankNode tank) =>
            weapon.vulcanTurbineAnimation.StartShooting();

        [OnEventFire]
        public void StopTurbineOnDeath(NodeRemoveEvent evt, ActiveTankNode tank,
            [JoinByTank] ReadyVulcanTurbineAnimationNode weapon) {
            weapon.vulcanTurbineAnimation.StopTurbine();
            weapon.Entity.RemoveComponent<VulcanTurbineAnimationReadyComponent>();
        }

        public class InitialVulcanTurbineAnimationNode : Node {
            public AnimationComponent animation;

            public AnimationPreparedComponent animationPrepared;

            public TankGroupComponent tankGroup;

            public VulcanTurbineAnimationComponent vulcanTurbineAnimation;
            public VulcanWeaponComponent vulcanWeapon;
        }

        public class ReadyVulcanTurbineAnimationNode : Node {
            public AnimationComponent animation;

            public AnimationPreparedComponent animationPrepared;

            public TankGroupComponent tankGroup;
            public VulcanTurbineAnimationComponent vulcanTurbineAnimation;

            public VulcanTurbineAnimationReadyComponent vulcanTurbineAnimationReady;
        }

        public class VulcanSlowDownNode : Node {
            public TankGroupComponent tankGroup;
            public VulcanSlowDownComponent vulcanSlowDown;
        }

        public class VulcanSpeedUpNode : Node {
            public TankGroupComponent tankGroup;
            public VulcanSpeedUpComponent vulcanSpeedUp;
        }

        public class VulcanShootingNode : Node {
            public TankGroupComponent tankGroup;
            public VulcanShootingComponent vulcanShooting;
        }

        public class ActiveTankNode : Node {
            public TankActiveStateComponent tankActiveState;

            public TankGroupComponent tankGroup;
        }
    }
}