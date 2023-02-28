using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.API;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class SpiderMineSoundsSystem : ECSSystem {
        [OnEventFire]
        public void StopRunningSound(MineExplosionEvent e, SpiderSoundsNode mine) {
            mine.spiderMineSounds.RunSoundController.FadeOut();
        }

        [OnEventFire]
        public void StopRunningSound(RemoveEffectEvent e, SpiderSoundsNode mine) {
            mine.spiderMineSounds.RunSoundController.FadeOut();
        }

        [OnEventFire]
        public void StartRunningSound(NodeAddedEvent e, [Combine] ActiveWithTargetSpiderSoundsNode mine, SingleNode<SoundListenerBattleStateComponent> listener) {
            UpdateRunningSound(mine);
        }

        void UpdateRunningSound(ActiveWithTargetSpiderSoundsNode mine) {
            Entity target = mine.unitTarget.Target;

            if (target.HasComponent<RigidbodyComponent>()) {
                if (!mine.spiderAnimator.OnGround) {
                    mine.spiderMineSounds.RunSoundController.FadeOut();
                } else {
                    mine.spiderMineSounds.RunSoundController.FadeIn();
                }
            }
        }

        [OnEventFire]
        public void UpdateRunningSound(UpdateEvent e, ActiveWithTargetSpiderSoundsNode mine, [JoinAll] SingleNode<SoundListenerBattleStateComponent> listener) {
            UpdateRunningSound(mine);
        }

        [OnEventFire]
        public void StopRunningSound(NodeRemoveEvent e, SingleNode<UnitTargetComponent> node, [JoinSelf] ActiveSpiderSoundsNode spider) {
            spider.spiderMineSounds.RunSoundController.FadeOut();
        }

        public class SpiderSoundsNode : Node {
            public SpiderAnimatorComponent spiderAnimator;
            public SpiderMineSoundsComponent spiderMineSounds;
        }

        public class ActiveSpiderSoundsNode : SpiderSoundsNode {
            public EffectActiveComponent effectActive;
        }

        public class ActiveWithTargetSpiderSoundsNode : ActiveSpiderSoundsNode {
            public UnitTargetComponent unitTarget;
        }
    }
}