using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class BonusTakingSoundSystem : ECSSystem {
        const float DESTROY_DELAY = 0.5f;

        [OnEventComplete]
        public void CreateAndPlayBonusTakingSound(NodeAddedEvent e, SingleNode<BrokenBonusBoxInstanceComponent> bonus, [JoinAll] SingleNode<BonusSoundConfigComponent> bonusClientConfig,
            [JoinAll] SingleNode<SoundListenerBattleStateComponent> soundListener) {
            BonusSoundConfigComponent component = bonusClientConfig.component;
            BrokenBonusBoxInstanceComponent component2 = bonus.component;
            AudioSource bonusTakingSound = component.BonusTakingSound;
            GetInstanceFromPoolEvent getInstanceFromPoolEvent = new();
            getInstanceFromPoolEvent.Prefab = bonusTakingSound.gameObject;
            getInstanceFromPoolEvent.AutoRecycleTime = bonusTakingSound.clip.length + 0.5f;
            GetInstanceFromPoolEvent getInstanceFromPoolEvent2 = getInstanceFromPoolEvent;
            ScheduleEvent(getInstanceFromPoolEvent2, bonus);
            Transform instance = getInstanceFromPoolEvent2.Instance;
            Transform transform = component2.Instance.transform;
            instance.position = transform.position;
            instance.rotation = transform.rotation;
            instance.gameObject.SetActive(true);
            instance.GetComponent<AudioSource>().Play();
        }
    }
}