using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Library.ClientDataStructures.API;
using Tanks.Battle.ClientCore.Impl;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class IceTrapSoundsSystem : ECSSystem {
        [OnEventComplete]
        public void PlayExplosionSound(MineExplosionEvent e, IceTrapNode effect, [JoinAll] SingleNode<SoundListenerBattleStateComponent> soundListener) {
            Transform transform = effect.effectInstance.GameObject.transform;
            GetInstanceFromPoolEvent getInstanceFromPoolEvent = new();
            getInstanceFromPoolEvent.Prefab = effect.iceTrapExplosionSound.ExplosionSoundAsset;
            getInstanceFromPoolEvent.AutoRecycleTime = effect.iceTrapExplosionSound.Lifetime;
            GetInstanceFromPoolEvent getInstanceFromPoolEvent2 = getInstanceFromPoolEvent;
            ScheduleEvent(getInstanceFromPoolEvent2, effect);
            Transform instance = getInstanceFromPoolEvent2.Instance;
            GameObject gameObject = instance.gameObject;
            instance.position = transform.position;
            instance.rotation = transform.rotation;
            gameObject.SetActive(true);
            Object.DontDestroyOnLoad(gameObject);
        }

        [OnEventFire]
        public void PrepareCleaningForEffects(NodeRemoveEvent evt, SingleNode<SelfBattleUserComponent> battleUser) {
            Object.FindObjectsOfType<IceTrapExplosionSoundBehaviour>().ForEach(delegate(IceTrapExplosionSoundBehaviour i) {
                i.GetComponentsInChildren<SoundFadeBehaviour>(true).ForEach(delegate(SoundFadeBehaviour s) {
                    s.enabled = true;
                    RFX4_AudioCurves component = s.GetComponent<RFX4_AudioCurves>();
                    RFX4_StartDelay component2 = s.GetComponent<RFX4_StartDelay>();

                    if ((bool)component) {
                        Object.Destroy(component);
                    }

                    if ((bool)component2) {
                        Object.Destroy(component2);
                    }
                });
            });
        }

        public class IceTrapNode : Node {
            public EffectInstanceComponent effectInstance;
            public IcetrapEffectComponent icetrapEffect;

            public IceTrapExplosionSoundComponent iceTrapExplosionSound;
        }
    }
}