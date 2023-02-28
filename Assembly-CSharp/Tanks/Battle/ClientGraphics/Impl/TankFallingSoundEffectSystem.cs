using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Platform.Kernel.ECS.ClientEntitySystem.Impl;
using Tanks.Battle.ClientCore.API;
using UnityEngine;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class TankFallingSoundEffectSystem : ECSSystem {
        const float DESTROY_OFFSET_SEC = 0.2f;

        const string UNKNOWN_FALLING_EXCEPTON = "Illegal type of falling";

        const string NO_FALLING_CLIPS_EXCEPTON = "No audio clips for falling";

        [OnEventFire]
        public void PlayFallingSound(TankFallEvent evt, TankFallingSoundEffectNode tank, [JoinAll] SingleNode<MapDustComponent> map) {
            TankFallingSoundEffectComponent tankFallingSoundEffect = tank.tankFallingSoundEffect;
            float fallingPower = evt.FallingPower;
            float minPower = tankFallingSoundEffect.MinPower;
            float maxPower = tankFallingSoundEffect.MaxPower;
            float num = Mathf.Clamp01((fallingPower - minPower) / (maxPower - minPower));

            if (!(num <= 0f) && evt.FallingType != 0) {
                Transform soundRootTransform = tank.tankSoundRoot.SoundRootTransform;
                AudioSource audioSource = PrepareAudioSource(evt, tankFallingSoundEffect, map.component, soundRootTransform);
                audioSource.volume = num;
                audioSource.Play();
            }
        }

        AudioSource PrepareAudioSource(TankFallEvent evt, TankFallingSoundEffectComponent tankFallingSoundEffect, MapDustComponent mapDust, Transform root) {
            AudioSource audioSource;

            switch (evt.FallingType) {
                case TankFallingType.TANK:
                case TankFallingType.VERTICAL_STATIC:
                    audioSource = tankFallingSoundEffect.CollisionSourceAsset;
                    break;

                case TankFallingType.FLAT_STATIC:
                case TankFallingType.SLOPED_STATIC_WITH_TRACKS:
                    audioSource = tankFallingSoundEffect.FallingSourceAsset;
                    break;

                case TankFallingType.SLOPED_STATIC_WITH_COLLISION: {
                    DustEffectBehaviour effectByTag = mapDust.GetEffectByTag(evt.FallingTransform, Vector2.zero);

                    if (effectByTag == null) {
                        audioSource = tankFallingSoundEffect.FallingSourceAsset;
                        break;
                    }

                    DustEffectBehaviour.SurfaceType surface = effectByTag.surface;

                    audioSource = surface != DustEffectBehaviour.SurfaceType.Metal && surface != DustEffectBehaviour.SurfaceType.Concrete ? tankFallingSoundEffect.FallingSourceAsset
                                      : tankFallingSoundEffect.CollisionSourceAsset;

                    break;
                }

                default:
                    throw new ArgumentException("Illegal type of falling");
            }

            AudioClip audioClip = !(audioSource == tankFallingSoundEffect.FallingSourceAsset) ? audioSource.clip : GetFallingAudioClip(tankFallingSoundEffect);
            float autoRecycleTime = audioClip.length + 0.2f;
            GetInstanceFromPoolEvent getInstanceFromPoolEvent = new();
            getInstanceFromPoolEvent.Prefab = audioSource.gameObject;
            getInstanceFromPoolEvent.AutoRecycleTime = autoRecycleTime;
            GetInstanceFromPoolEvent getInstanceFromPoolEvent2 = getInstanceFromPoolEvent;
            ScheduleEvent(getInstanceFromPoolEvent2, new EntityStub());
            Transform instance = getInstanceFromPoolEvent2.Instance;
            AudioSource component = instance.GetComponent<AudioSource>();
            instance.parent = root;
            instance.localPosition = Vector3.zero;
            instance.localRotation = Quaternion.identity;
            instance.gameObject.SetActive(true);
            component.Play();
            return component;
        }

        AudioClip GetFallingAudioClip(TankFallingSoundEffectComponent effect) {
            AudioClip[] fallingClips = effect.FallingClips;
            int num = fallingClips.Length;

            if (num == 0) {
                throw new ArgumentException("No audio clips for falling");
            }

            AudioClip result = fallingClips[effect.FallingClipIndex];
            effect.FallingClipIndex++;

            if (effect.FallingClipIndex >= num) {
                effect.FallingClipIndex = 0;
            }

            return result;
        }

        public class TankFallingSoundEffectNode : Node {
            public AssembledTankActivatedStateComponent assembledTankActivatedState;
            public TankFallingSoundEffectComponent tankFallingSoundEffect;

            public TankSoundRootComponent tankSoundRoot;
        }
    }
}