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
        public void StopTurbine(NodeAddedEvent evt, VulcanIdleNode idle, [Context] [JoinByTank] ReadyVulcanTurbineAnimationNode weapon, [Context] [JoinByTank] ActiveTankNode tank) {
            weapon.vulcanTurbineAnimation.StopTurbine();
        }

        [OnEventFire]
        public void StartSpeedUp(NodeAddedEvent evt, VulcanSpeedUpNode speedUpState, [Context] [JoinByTank] ReadyVulcanTurbineAnimationNode weapon,
            [Context] [JoinByTank] ActiveTankNode tank) {
            weapon.vulcanTurbineAnimation.StartSpeedUp();
        }

        [OnEventFire]
        public void StartSlowDown(NodeAddedEvent evt, VulcanSlowDownNode slowDownState, [Context] [JoinByTank] ReadyVulcanTurbineAnimationNode weapon,
            [Context] [JoinByTank] ActiveTankNode tank) {
            weapon.vulcanTurbineAnimation.StartSlowDown();
        }

        [OnEventFire]
        public void StartShooting(NodeAddedEvent evt, VulcanShootingNode shootingState, [Context] [JoinByTank] ReadyVulcanTurbineAnimationNode weapon,
            [Context] [JoinByTank] ActiveTankNode tank) {
            weapon.vulcanTurbineAnimation.StartShooting();
        }

        [OnEventFire]
        public void StopTurbineOnDeath(NodeRemoveEvent evt, ActiveTankNode tank, [JoinByTank] ReadyVulcanTurbineAnimationNode weapon) {
            weapon.vulcanTurbineAnimation.StartSlowDown();
        }

        public class InitialVulcanTurbineAnimationNode : Node {
            public AnimationComponent animation;

            public AnimationPreparedComponent animationPrepared;

            public TankGroupComponent tankGroup;

            public VulcanTurbineAnimationComponent vulcanTurbineAnimation;
            public VulcanWeaponComponent vulcanWeapon;
        }

        public class ReadyVulcanTurbineAnimationNode : InitialVulcanTurbineAnimationNode {
            public VulcanTurbineAnimationReadyComponent vulcanTurbineAnimationReady;
        }

        public class VulcanIdleNode : ReadyVulcanTurbineAnimationNode {
            public VulcanIdleComponent vulcanIdle;
        }

        public class VulcanSlowDownNode : ReadyVulcanTurbineAnimationNode {
            public VulcanSlowDownComponent vulcanSlowDown;
        }

        public class VulcanSpeedUpNode : ReadyVulcanTurbineAnimationNode {
            public VulcanSpeedUpComponent vulcanSpeedUp;
        }

        public class VulcanShootingNode : ReadyVulcanTurbineAnimationNode {
            public WeaponStreamShootingComponent weaponStreamShooting;
        }

        public class ActiveTankNode : Node {
            public TankActiveStateComponent tankActiveState;

            public TankGroupComponent tankGroup;
        }
    }
}