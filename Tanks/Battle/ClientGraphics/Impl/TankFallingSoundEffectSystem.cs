using System;
using Platform.Kernel.ECS.ClientEntitySystem.API;
using Tanks.Battle.ClientCore.API;
using Tanks.Battle.ClientGraphics.API;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Tanks.Battle.ClientGraphics.Impl {
    public class TankFallingSoundEffectSystem : ECSSystem {
        const float DESTROY_OFFSET_SEC = 0.2f;

        const string UNKNOWN_FALLING_EXCEPTON = "Illegal type of falling";

        const string NO_FALLING_CLIPS_EXCEPTON = "No audio clips for falling";

        [OnEventFire]
        public void PlayFallingSound(TankFallEvent evt, TankFallingSoundEffectNode tank,
            [JoinAll] SingleNode<MapDustComponent> map,
            [JoinAll] SingleNode<SoundListenerBattleStateComponent> soundListener) {
            TankFallingSoundEffectComponent tankFallingSoundEffect = tank.tankFallingSoundEffect;
            float fallingPower = evt.FallingPower;
            float minPower = tankFallingSoundEffect.MinPower;
            float maxPower = tankFallingSoundEffect.MaxPower;
            float num = Mathf.Clamp01((fallingPower - minPower) / (maxPower - minPower));

            if (num != 0f) {
                Transform soundRootTransform = tank.tankSoundRoot.SoundRootTransform;
                AudioSource audioSource = PrepareAudioSource(evt, tankFallingSoundEffect, map.component, soundRootTransform);
                AudioClip clip = audioSource.clip;
                audioSource.volume = num;
                audioSource.Play();
                Object.Destroy(audioSource.gameObject, clip.length + 0.2f);
            }
        }

        AudioSource PrepareAudioSource(TankFallEvent evt, TankFallingSoundEffectComponent tankFallingSoundEffect,
            MapDustComponent mapDust, Transform root) {
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
                    DustEffectBehaviour.SurfaceType surfaceType = surface;

                    audioSource =
                        surfaceType != DustEffectBehaviour.SurfaceType.Metal &&
                        surfaceType != DustEffectBehaviour.SurfaceType.Concrete ? tankFallingSoundEffect.FallingSourceAsset
                            : tankFallingSoundEffect.CollisionSourceAsset;

                    break;
                }

                default:
                    throw new ArgumentException("Illegal type of falling");
            }

            bool flag = audioSource == tankFallingSoundEffect.FallingSourceAsset;
            AudioSource audioSource2 = Object.Instantiate(audioSource);
            Transform transform = audioSource2.transform;
            transform.parent = root;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;

            if (flag) {
                UpdateFallingAudioClip(audioSource2, tankFallingSoundEffect);
            }

            return audioSource2;
        }

        void UpdateFallingAudioClip(AudioSource fallingSourceInstance,
            TankFallingSoundEffectComponent tankFallingSoundEffect) {
            AudioClip[] fallingClips = tankFallingSoundEffect.FallingClips;
            int num = fallingClips.Length;

            if (num == 0) {
                throw new ArgumentException("No audio clips for falling");
            }

            AudioClip clip = fallingClips[tankFallingSoundEffect.FallingClipIndex];
            tankFallingSoundEffect.FallingClipIndex++;

            if (tankFallingSoundEffect.FallingClipIndex >= num) {
                tankFallingSoundEffect.FallingClipIndex = 0;
            }

            fallingSourceInstance.clip = clip;
        }

        public class TankFallingSoundEffectNode : Node {
            public AssembledTankActivatedStateComponent assembledTankActivatedState;
            public TankFallingSoundEffectComponent tankFallingSoundEffect;

            public TankSoundRootComponent tankSoundRoot;
        }
    }
}