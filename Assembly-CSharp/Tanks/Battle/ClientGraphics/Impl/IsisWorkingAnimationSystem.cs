using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class IsisWorkingAnimationSystem : ECSSystem {
        [OnEventFire]
        public void InitIsisAnimation(NodeAddedEvent evt, InitialIsisWorkingAnimationNode weapon) {
            weapon.isisWorkingAnimation.InitIsisWorkingAnimation(weapon.animation.Animator);
            weapon.Entity.AddComponent<IsisWorkingAnimationReadyComponent>();
        }

        [OnEventFire]
        public void StartAnimation(NodeAddedEvent evt, IsisWorkingStateNode workingState, [Context] [JoinByTank] ReadyIsisWorkingAnimationNode weapon) {
            weapon.isisWorkingAnimation.StartWorkingAnimation();
        }

        [OnEventFire]
        public void StopAnimation(NodeRemoveEvent evt, IsisWorkingStateNode workingState, [JoinByTank] ReadyIsisWorkingAnimationNode weapon) {
            weapon.isisWorkingAnimation.StopWorkingAnimation();
        }

        public class InitialIsisWorkingAnimationNode : Node {
            public AnimationComponent animation;

            public AnimationPreparedComponent animationPrepared;

            public IsisWorkingAnimationComponent isisWorkingAnimation;
        }

        public class ReadyIsisWorkingAnimationNode : Node {
            public AnimationComponent animation;

            public AnimationPreparedComponent animationPrepared;

            public IsisWorkingAnimationComponent isisWorkingAnimation;

            public IsisWorkingAnimationReadyComponent isisWorkingAnimationReady;

            public TankGroupComponent tankGroup;
        }

        public class IsisWorkingStateNode : Node {
            public StreamWeaponWorkingComponent streamWeaponWorking;

            public TankGroupComponent tankGroup;
        }
    }
}