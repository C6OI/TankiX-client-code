using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class BonusTakingSoundSystem : ECSSystem {
        const float DESTROY_DELAY = 0.5f;

        [OnEventComplete]
        public void CreateAndPlayBonusTakingSound(NodeAddedEvent e, SingleNode<BrokenBonusBoxInstanceComponent> bonus,
            [JoinAll] SingleNode<BonusSoundConfigComponent> bonusClientConfig,
            [JoinAll] SingleNode<SoundListenerBattleStateComponent> soundListener) {
            BonusSoundConfigComponent component = bonusClientConfig.component;
            BrokenBonusBoxInstanceComponent component2 = bonus.component;
            AudioSource bonusTakingSound = component.BonusTakingSound;
            AudioSource audioSource = Object.Instantiate(bonusTakingSound);
            Transform transform = audioSource.transform;
            Transform transform2 = component2.Instance.transform;
            transform.position = transform2.position;
            transform.rotation = transform2.rotation;
            audioSource.Play();
            Object.DestroyObject(audioSource.gameObject, audioSource.clip.length + 0.5f);
        }
    }
}