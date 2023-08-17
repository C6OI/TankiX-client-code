using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.Impl;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class ShaftAimingAnimationSystem : ECSSystem {
        [OnEventFire]
        public void InitShaftAimingAnimation(NodeAddedEvent evt, InitialShaftAimingAnimationNode weapon) {
            Animator animator = weapon.animation.Animator;
            weapon.shaftAimingAnimation.InitShaftAimingAnimation(animator);
            weapon.Entity.AddComponent<ShaftAimingAnimationReadyComponent>();
        }

        [OnEventFire]
        public void StartAiming(NodeAddedEvent evt, ShaftAimingWorkingStateNode weapon) =>
            weapon.shaftAimingAnimation.StartAiming();

        [OnEventFire]
        public void StopAiming(NodeRemoveEvent evt, ShaftAimingWorkingStateNode weapon) =>
            weapon.shaftAimingAnimation.StopAiming();

        public class InitialShaftAimingAnimationNode : Node {
            public AnimationComponent animation;

            public AnimationPreparedComponent animationPrepared;
            public ShaftAimingAnimationComponent shaftAimingAnimation;
        }

        public class ShaftAimingWorkingStateNode : Node {
            public AnimationComponent animation;

            public AnimationPreparedComponent animationPrepared;

            public ShaftAimingAnimationComponent shaftAimingAnimation;

            public ShaftAimingAnimationReadyComponent shaftAimingAnimationReady;
            public ShaftAimingWorkingStateComponent shaftAimingWorkingState;
        }
    }
}