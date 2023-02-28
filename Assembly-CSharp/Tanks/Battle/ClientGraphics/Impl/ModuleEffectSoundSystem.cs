using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.API;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class ModuleEffectSoundSystem : ECSSystem {
        [OnEventComplete]
        public void PlayEffectRemovingSound(RemoveEffectEvent e, SingleNode<EffectRemovingSoundComponent> effect, [JoinAll] SingleNode<SoundListenerBattleStateComponent> soundListener) {
            effect.component.Sound.Play();
        }

        [OnEventFire]
        public void StopEffectCreateSound(NodeRemoveEvent e, SingleNode<EffectActivationSoundComponent> effect) {
            effect.component.Sound.Stop();
        }

        [OnEventFire]
        public void PlayEffectCreateSound(EffectActivationEvent evt, SingleNode<EffectComponent> effect, [JoinAll] SingleNode<SoundListenerBattleStateComponent> soundListener) {
            effect.Entity.AddComponent<EffectReadyForActivationSoundComponent>();
        }

        [OnEventFire]
        public void PlayEffectCreateSound(NodeAddedEvent evt, EffectActivationNode effect, [JoinAll] SingleNode<SoundListenerBattleStateComponent> soundListener) {
            effect.effectActivationSound.Sound.Play();
        }

        public class EffectActivationNode : Node {
            public EffectActivationSoundComponent effectActivationSound;
            public EffectReadyForActivationSoundComponent effectReadyForActivationSound;
        }
    }
}